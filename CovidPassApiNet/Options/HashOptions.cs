using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace CovidPass_API.Options
{
    public class HashOptions
    {
        public const string Key = "Hashes";

        [Required] public string Icon { get; set; }
        [Required] public string Icon2X { get; set; }
        [Required] public string Logo { get; set; }
        [Required] public string Logo2X { get; set; }
    }
}