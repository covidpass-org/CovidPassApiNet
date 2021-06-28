using System.Text.Json.Serialization;

namespace CovidPass_API.Records
{
    public class Manifest
    {
        [JsonPropertyName("pass.json")] public string PassJsonHash { get; set; }

        [JsonPropertyName("icon.png")] public string IconHash { get; set; }

        [JsonPropertyName("icon@2x.png")] public string Icon2XHash { get; set; }

        [JsonPropertyName("logo.png")] public string LogoHash { get; set; }

        [JsonPropertyName("logo@2x.png")] public string Logo2XHash { get; set; }
    }
}