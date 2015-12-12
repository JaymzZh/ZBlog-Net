using System.ComponentModel.DataAnnotations;

namespace ZBlog.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
