using System.ComponentModel.DataAnnotations;

namespace CovidPass_API.Options
{
    public class CertificateOptions
    {
        public const string Key = "Certificates";

        [Required] public string AppleCaPath { get; set; }

        [Required] public string AppleDeveloperPath { get; set; }

        [Required] public string AppleDeveloperPasswordPath { get; set; }
    }
}