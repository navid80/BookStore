using System.ComponentModel.DataAnnotations;

namespace BookStore.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل نادرست است")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [MinLength(6, ErrorMessage = "رمز عبور باید حداقل ۶ کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "تکرار رمز عبور الزامی است")]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن یکسان نیستند")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}