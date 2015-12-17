using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZBlog.Common;

namespace ZBlog.Models
{
    public static class SampleData
    {
        public static async Task InitializeZBlog(IServiceProvider serviceProvider, IConfigurationRoot configuration, bool createUsers = true)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var serverProvider = serviceScope.ServiceProvider;
                var dbContext = serverProvider.GetService<ZBlogDbContext>();

                if (await dbContext.Database.EnsureCreatedAsync())
                {
                    await InsertTestData(serverProvider);
                    if (createUsers)
                    {
                        await CreateAdminUser(dbContext, configuration);
                    }
                }
            }
        }

        private static Task InsertTestData(IServiceProvider serviceProvider)
        {
            return Task.FromResult(0);
        }

        private static async Task CreateAdminUser(ZBlogDbContext dbContext, IConfiguration configuration)
        {
            var user = await dbContext.Users.Where(u => u.Name.Equals("zhangmm", StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
            if (user == null)
            {
                user = new User
                {
                    Name = configuration["User:Name"],
                    NickName = configuration["User:NickName"],
                    Email = configuration["User:Email"],
                    Password = Util.GetMd5(configuration["User:Password"])
                };
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}