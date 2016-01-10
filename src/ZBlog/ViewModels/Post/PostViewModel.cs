using System;
using System.ComponentModel.DataAnnotations;
using ZBlog.Models;

namespace ZBlog.ViewModels.Post
{
    public class PostViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required]
        public string Content { get; set; }
        
        public DateTime CreateTime { get; set; }

        public DateTime LastEditTime { get; set; }

        public int Visits { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
        
        public int CatalogId { get; set; }

        public virtual Catalog Catalog { get; set; }

        public bool IsNew { get; set; }
    }
}
