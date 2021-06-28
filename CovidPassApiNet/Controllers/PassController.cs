using System.IO;
using System.Net.Mime;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CovidPass_API.Models;
using CovidPass_API.Options;
using CovidPass_API.Records;
using CovidPass_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Passbook.Generator;

namespace CovidPass_API.Controllers
{
    [ApiController]
    [Route("sign")]
    public class PassController : ControllerBase
    {
        private readonly SigningService _signingService;
        private readonly IOptions<HashOptions> _hashOptions;

        public PassController(SigningService signingService, IOptions<HashOptions> hashOptions)
        {
            _signingService = signingService;
            _hashOptions = hashOptions;
        }

        [HttpPost]
        public IActionResult SignPass([FromBody] SignPass signPass)
        {
            var manifest = new Manifest
            {
                PassJsonHash = signPass.PassJsonHash,
                IconHash = _hashOptions.Value.Icon,
                Icon2XHash = _hashOptions.Value.Icon2X,
                LogoHash = _hashOptions.Value.Logo,
                Logo2XHash = _hashOptions.Value.Logo2X
            };

            // Sign the manifest and get the signature
            byte[] signature = _signingService.SignManifest(manifest);

            return new FileStreamResult(new MemoryStream(signature), "application/pkcs7-signature");
        }
    }
}