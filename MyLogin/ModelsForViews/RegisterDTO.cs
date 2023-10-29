using Microsoft.AspNetCore.Mvc;
using MyLogin.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyLogin.ModelsForViews
{
    public class RegisterDTO
    {
        [Required]
        public string? PersonName { get; set; }
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailAlreadyRegisterd",controller:"Account",ErrorMessage ="Email already taken")]
        public string? Email { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        public UserTypesOptions UserType { get; set; }=UserTypesOptions.User;
    }
}