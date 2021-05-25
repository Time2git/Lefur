using System.ComponentModel.DataAnnotations;

namespace LearnFurther.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
