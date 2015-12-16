using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using ZBlog.Models;

namespace ZBlog.Migrations
{
    [DbContext(typeof(ZBlogDbContext))]
    partial class ZBlogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("ZBlog.Models.Catalog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PRI");

                    b.Property<string>("Title");

                    b.Property<string>("Url")
                        .HasAnnotation("MaxLength", 32);

                    b.HasKey("Id");

                    b.HasIndex("PRI");
                });

            modelBuilder.Entity("ZBlog.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CatalogId");

                    b.Property<string>("Content");

                    b.Property<string>("Summary");

                    b.Property<DateTime>("Time");

                    b.Property<string>("Title");

                    b.Property<string>("Url")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Time");

                    b.HasIndex("Url")
                        .IsUnique();
                });

            modelBuilder.Entity("ZBlog.Models.PostTag", b =>
                {
                    b.Property<int>("TagId");

                    b.Property<Guid?>("PostId");

                    b.HasKey("TagId", "PostId");

                    b.HasIndex("PostId");

                    b.HasIndex("TagId");
                });

            modelBuilder.Entity("ZBlog.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("Id");
                });

            modelBuilder.Entity("ZBlog.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("About");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("NickName")
                        .HasAnnotation("MaxLength", 64);

                    b.Property<string>("Password")
                        .HasAnnotation("MaxLength", 32);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("ZBlog.Models.Post", b =>
                {
                    b.HasOne("ZBlog.Models.Catalog")
                        .WithMany()
                        .HasForeignKey("CatalogId");

                    b.HasOne("ZBlog.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ZBlog.Models.PostTag", b =>
                {
                    b.HasOne("ZBlog.Models.Post")
                        .WithMany()
                        .HasForeignKey("PostId");

                    b.HasOne("ZBlog.Models.Tag")
                        .WithMany()
                        .HasForeignKey("TagId");
                });
        }
    }
}
