using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BruswBlog.Models
{
    public static class SampleData
    {
        public static async Task InitializeBruswBlog(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<BlogContext>();
                await db.Database.EnsureCreatedAsync();
            }
        }
    }
}
