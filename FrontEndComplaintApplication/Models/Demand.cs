namespace FrontEndComplaintApplication.Models
{
    public class Demand
    {
        public int Id { get; set; }
        public string DemandText { get; set; }

        public int ComplaintId { get; set; } // Foreign key to link with the User table


    }
}
