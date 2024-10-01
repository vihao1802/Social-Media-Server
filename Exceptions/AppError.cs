namespace SocialMediaServer.Exceptions
{
    public class AppError: Exception
    {
        public int StatusCode { get; }

        public AppError(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
