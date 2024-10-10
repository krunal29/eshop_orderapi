using System;
using System.ComponentModel.DataAnnotations;

namespace eshop_orderapi.Business.ViewModels
{
    public class ResetPasswordViewModel
    {
        public Guid ResetId { get; set; }

        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Your password must be at least 8 characters long")]
        [Display(Name = "Password (8 or more characters)")]
        [Required(ErrorMessage = "Enter a password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Enter Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords doesn't match")]
        [Required(ErrorMessage = "Enter a Confirm password")]
        public string PasswordConfirm { get; set; }
    }
}