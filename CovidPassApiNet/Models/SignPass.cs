using System.ComponentModel.DataAnnotations;

namespace CovidPass_API.Models
{
    public class SignPass
    {
        [Required] public string PassJsonHash { get; set; }
        [Required] public bool UseBlackVersion { get; set; }
    }
}