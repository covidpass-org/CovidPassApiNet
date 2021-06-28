using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace CovidPass_API.Options
{
    public class HashOptions
    {
        public const string Key = "Hashes";

        [Required] public string IconBlack { get; set; }
        [Required] public string IconWhite { get; set; }
        [Required] public string Icon2XBlack { get; set; }
        [Required] public string Icon2XWhite { get; set; }
        [Required] public string LogoBlack { get; set; }
        [Required] public string LogoWhite { get; set; }
        [Required] public string Logo2XBlack { get; set; }
        [Required] public string Logo2XWhite { get; set; }
    }
}