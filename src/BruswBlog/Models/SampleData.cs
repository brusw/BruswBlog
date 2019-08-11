using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BruswBlog.Models
{
    public static class SampleData
    {
        public static async Task InitializeBruswBlog(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetService<BlogContext>();
            await db.Database.EnsureCreatedAsync();
        }
    }
}
