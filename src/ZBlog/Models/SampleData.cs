using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using ZBlog.Common;

namespace ZBlog.Models
{
    public static class SampleData
    {
        public static async Task InitializeZBlog(IServiceProvider serviceProvider, bool createUsers = true)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ZBlogDbContext>();

                if (await db.Database.EnsureCreatedAsync())
                {
                    await InsertTestData(serviceProvider);
                    if (createUsers)
                    {
                        await CreateAdminUser(db);
                    }
                }
            }
        }

        private static Task InsertTestData(IServiceProvider serviceProvider)
        {
            return Task.FromResult(0);
        }

        private static async Task CreateAdminUser(ZBlogDbContext dbContext)
        {
            var user = await dbContext.Users.Where(u => u.Name.Equals("zhangmm", StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();

            if (user == null)
            {
                user = new User
                {
                    Name = "zhangmm",
                    NickName = "Jeffiy",
                    Email = "zhangmin6105@qq.com",
                    Password = Util.GetMd5("123150")
                };
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}