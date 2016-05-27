using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ZBlog.Models
{
    public class ZBlogDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<Catalog> Catalogs { get; set; }

        public ZBlogDbContext(DbContextOptions<ZBlogDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }


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

            builder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            builder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);

            builder.Entity<Tag>(e =>
            {
                e.HasIndex(x => x.Id);
            });
        }
    }
}
