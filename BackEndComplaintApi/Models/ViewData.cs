using MessagePack;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackEndComplaintApi.Models
{
    public class ViewData
    {

        public UserLoginModel? UserLoginModels { get; set; }
        public User? Users { get; set; }

    }
}
