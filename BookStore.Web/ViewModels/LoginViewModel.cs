using System.ComponentModel.DataAnnotations;

namespace BookStore.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل نادرست است")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}