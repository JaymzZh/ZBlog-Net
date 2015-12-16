using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZBlog.Models
{
    public class Catalog
    {
        public int Id { get; set; }

        [MaxLength(32)]
        public string Url { get; set; }

        public string Title { get; set; }

        public int PRI { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}