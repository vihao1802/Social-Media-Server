using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using SocialMediaServer.Data;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Middleware;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Implementations;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Implementations;
using SocialMediaServer.Services.Interfaces;
using SocialMediaServer.Utils;

using CloudinaryDotNet;
using DotNetEnv;
using SocialMediaServer.Configuration;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Facebook;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });
});
// Load environment variables from .env file
Env.Load();

// Configure Cloudinary settings
var cloudinarySettings = new CloudinarySettings
{
    CloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME") ?? throw new ArgumentException("CLOUDINARY_CLOUD_NAME is missing."),
    ApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY") ?? throw new ArgumentException("CLOUDINARY_API_KEY is missing."),
    ApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET") ?? throw new ArgumentException("CLOUDINARY_API_SECRET is missing.")

};


// Register Cloudinary as a singleton service
var cloudinaryAccount = new Account(cloudinarySettings.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);
var cloudinary = new Cloudinary(cloudinaryAccount);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });
});


builder.Services.AddSingleton(cloudinary);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<User, IdentityRole>(Options =>
{
    Options.Password.RequireDigit = true;
    Options.Password.RequireLowercase = true;
    Options.Password.RequireUppercase = true;
    Options.Password.RequireNonAlphanumeric = true;
    Options.Password.RequiredLength = 8;
    Options.User.AllowedUserNameCharacters = null; // allow all charater including VIE format
    Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(365 * 100);  // Lockout time duration
    Options.Lockout.AllowedForNewUsers = true;

})
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    // Cấu hình Cookie Authentication (chỉ cho Google Auth)
    options.LoginPath = "/api/auth/external-login/Google";
    options.LogoutPath = "/logout";
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value ?? throw new ArgumentException("Google ClientId is missing.");
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value ?? throw new ArgumentException("Google ClientSecret is missing.");
    // Yêu cầu thêm thông tin trong scope


    options.Scope.Add("email"); // Lấy ngày sinh
    options.Scope.Add("profile"); // Lấy ngày sinh
    options.Scope.Add("https://www.googleapis.com/auth/userinfo.profile"); // Lấy ngày sinh
    options.Scope.Add("https://www.googleapis.com/auth/userinfo.email"); // Lấy ngày sinh
    options.Scope.Add("https://www.googleapis.com/auth/user.birthday.read"); // Lấy ngày sinh
    options.Scope.Add("https://www.googleapis.com/auth/user.gender.read");

    // Lấy ảnh đại diện
    // Xử lý sự kiện khi xác thực thành công
    options.Events.OnCreatingTicket = async context =>
    {
        // Gọi API của Google để lấy thêm thông tin người dùng
        var userInfoEndpoint = "https://people.googleapis.com/v1/people/me?personFields=emailAddresses,names,birthdays,genders,photos";
        var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, userInfoEndpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
        var response = await context.Backchannel.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<JsonElement>(json);

            // TODO: xem coi authenticate xong nó có trả về email với name không, nếu không thì add claim thủ công
            // hoàn thành việc đăng kí và đăng nhập google TRONG HÔM NAY
            // hoàn thành thêm việc đăng kí và đăng nhập bằng facebook


            // Thêm claims tùy chỉnh
            var picture = user.GetProperty("photos")[0].GetProperty("url").GetString(); // Lấy ảnh đại diện
            var gender = user.GetProperty("genders")[0].GetProperty("value").GetString(); // Lấy giới tính
            var day = user.GetProperty("birthdays")[0].GetProperty("date").GetProperty("day").GetInt32().ToString(); // Lấy ngày sinh
            var month = user.GetProperty("birthdays")[0].GetProperty("date").GetProperty("month").GetInt32().ToString(); // Lấy ngày sinh
            var year = user.GetProperty("birthdays")[0].GetProperty("date").GetProperty("year").GetInt32().ToString(); // Lấy ngày sinh
            var birthday = $"{day}/{month}/{year}";

            context.Identity.AddClaim(new Claim("picture", picture ?? ""));
            context.Identity.AddClaim(new Claim("gender", gender ?? ""));
            context.Identity.AddClaim(new Claim("birthday", birthday ?? ""));
        }
    };

})
.AddFacebook(FacebookDefaults.AuthenticationScheme, options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.AppId = builder.Configuration.GetSection("FackbookKeys:AppId").Value ?? throw new ArgumentException("Facebook AppId is missing.");
    options.AppSecret = builder.Configuration.GetSection("FackbookKeys:AppSecret").Value ?? throw new ArgumentException("Facebook AppSecret is missing.");
    options.Scope.Add("email");
    options.Scope.Add("user_birthday");
    options.Scope.Add("user_gender");
    options.Events.OnCreatingTicket = async context =>
    {
        // Gọi API của Facebook để lấy thêm thông tin người dùng
        var userInfoEndpoint = "https://graph.facebook.com/v11.0/me?fields=id,name,email,birthday,gender";
        var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, userInfoEndpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
        var response = await context.Backchannel.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<JsonElement>(json);

            // Thêm claims tùy chỉnh
            var picture = $"https://graph.facebook.com/{user.GetProperty("id").GetString()}/picture?type=large"; // Lấy ảnh đại diện
            var birthday = user.GetProperty("birthday").GetString(); // Lấy ngày sinh
            var gender = user.GetProperty("gender").GetString(); // Lấy giới tính

            context.Identity.AddClaim(new Claim("picture", picture ?? ""));
            context.Identity.AddClaim(new Claim("birthday", birthday ?? ""));
            context.Identity.AddClaim(new Claim("gender", gender ?? ""));

        }
    };
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});


builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformationService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRelationshipRepository, RelationshipRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRelationshipService, RelationshipService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<IMessengeRepository, MessengeRepository>();
builder.Services.AddScoped<IMessengeService, MessengeService>();
builder.Services.AddScoped<IMessengeMediaContent, MessengeMediaContentRepository>();
builder.Services.AddScoped<IWebSocketService, WebSocketService>();




builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPostViewerRepository, PostViewerRepository>();
builder.Services.AddScoped<IPostViewerService, PostViewerService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IMediaContentRepository, MediaContentRepository>();
builder.Services.AddScoped<IMediaContentService, MediaContentService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICommentReactionRepository, CommentReactionRepository>();
builder.Services.AddScoped<ICommentReactionService, CommentReactionService>();
builder.Services.AddScoped<IMessengeFileService, MessengeFileService>();
builder.Services.AddScoped<IGroupChatRepository, GroupChatRepository>();
builder.Services.AddScoped<IGroupChatService, GroupChatService>();
builder.Services.AddScoped<IGroupMemberRepository, GroupMemberRepository>();
builder.Services.AddScoped<IGroupMemberService, GroupMemberService>();
builder.Services.AddScoped<IGroupMessengeRepository, GroupMessengeRepository>();
builder.Services.AddScoped<IGroupMessengeService, GroupMessengeService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IMessageReactionRepository, MessageReactionRepository>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddAuthorization();

//Register exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

// Enable WebSocket middleware
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(webSocketOptions);

// Map WebSocket requests
app.Map("/ws/messenge", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocketService = context.RequestServices.GetRequiredService<IWebSocketService>();
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await webSocketService.HandleWebSocketConnectionAsync(webSocket);
    }
    else
    {
        context.Response.StatusCode = 400; // Bad Request
    }
});

app.MapControllers();


app.Run();