using System.ComponentModel.DataAnnotations;

namespace CovidPass_API.Options
{
    public class ServerOptions
    {
        public const string Key = "Server";

        public string AllowedOrigins { get; set; } = "http://localhost:5000";
    }
}