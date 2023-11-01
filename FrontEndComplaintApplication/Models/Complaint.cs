using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FrontEndComplaintApplication.Models
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Complaint text is required.")]
        public string ComplaintText { get; set; }

        public string FileName { get; set; } // Store the file path for the attached PDF
        [NotMapped]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Language is required.")]
        public string Language { get; set; } // Should be either "Arabic" or "English"


        public bool IsApproved { get; set; } // Indicates whether the complaint is approved by the administrator

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; } // Foreign key to link with the User table
        public List<Demand> Demands { get; set; }

    }
}
