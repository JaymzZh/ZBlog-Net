using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZBlog.Models
{
    public class Post
    {
        public int Id { get; set; }

        [MaxLength(256)]
        public string Url { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastEditTime { get; set; }

        public int Visits { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("Catalog")]
        [Display(Name = "Catalog")]
        public int? CatalogId { get; set; }

        public virtual Catalog Catalog { get; set; }

        public virtual ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();

        public Post()
        {
            CreateTime = DateTime.Now;
            LastEditTime = DateTime.Now;
        }
    }
}