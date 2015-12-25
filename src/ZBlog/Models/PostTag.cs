using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZBlog.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<PostTag> PostTags { get; set; }
    }
    
    public class PostTag
    {
        [ForeignKey("Post")]
        public int? PostId { get; set; }

        public virtual Post Post { get; set; }
        
        [ForeignKey("Tag")]
        public int? TagId { get; set; }

        public virtual Tag Tag { get; set; }
    }
}