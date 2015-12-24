using Microsoft.Data.Entity;

namespace ZBlog.Models
{
    public class ZBlogDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<Catalog> Catalogs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Catalog>(e =>
            {
                e.HasIndex(x => x.PRI);
            });

            builder.Entity<Post>(e =>
            {
                e.HasIndex(x => x.CreateTime);
                e.HasIndex(x => x.Url).IsUnique();
            });

            builder.Entity<PostTag>(e =>
            {
                e.HasIndex(x => x.TagId);
                e.HasIndex(x => x.PostId);
                e.HasKey(x => new {x.TagId, x.PostId});
            });

            builder.Entity<Tag>(e =>
            {
                e.HasIndex(x => x.Id);
            });
        }
    }
}
