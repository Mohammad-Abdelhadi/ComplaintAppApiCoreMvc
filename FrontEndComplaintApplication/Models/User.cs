using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FrontEndComplaintApplication.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(100, ErrorMessage = "The Max Length Is 100")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [MaxLength(100, ErrorMessage = "The Max Length Is 100")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required.")]
        [MaxLength(10, ErrorMessage = "The Max Length Is 10 Numbers")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(100, ErrorMessage = "The Max Length Is 100")]
        public string Password { get; set; }


        [NotMapped]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }



        [JsonIgnore]
        public string Role { get; set; } = "user"; // Set default role to "user"
        //
    }
}
