using System;
using System.ComponentModel.DataAnnotations;

namespace ZBlog.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [MaxLength(64)]
        public string NickName { get; set; }

        public string Email { get; set; }

        [MaxLength(64)]
        public string Password { get; set; }

        public string About { get; set; }
    }
}
