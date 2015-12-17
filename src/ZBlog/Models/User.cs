using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZBlog.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [MaxLength(64)]
        public string NickName { get; set; }

        public string Email { get; set; }

        [MaxLength(32)]
        public string Password { get; set; }

        public string About { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}