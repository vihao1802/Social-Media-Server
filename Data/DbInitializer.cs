using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaServer.Data
{
    public class DbInitializer
    {
        public static void SeedFromSql(ApplicationDBContext context, string sqlFilePath)
        {
            // Đảm bảo cơ sở dữ liệu đã tồn tại
            context.Database.EnsureCreated();

            // Kiểm ra xem đã có dữ liệu chưa
            if (context.Users.Any()
                || context.Relationships.Any()
                || context.Posts.Any()
                || context.PostViewers.Any()
                || context.MediaContents.Any()
                || context.Comments.Any()
                || context.CommentReactions.Any()
                || context.Groups.Any()
                || context.GroupMembers.Any()
                || context.GroupMessenges.Any()
                || context.Messenges.Any()
                || context.MessengeMediaContents.Any()
                || context.MessageReactions.Any()
                || context.Notifications.Any())
            {
                return;
            }
            Console.WriteLine("Seeding data from SQL file: " + sqlFilePath);
            // Đọc file SQL
            var sql = File.ReadAllText(sqlFilePath);

            // Thực thi lệnh SQL
            context.Database.ExecuteSqlRaw(sql);
        }
    }
}