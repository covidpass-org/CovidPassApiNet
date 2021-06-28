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
                IconHash = signPass.UseBlackVersion ? _hashOptions.Value.IconBlack : _hashOptions.Value.IconWhite,
                Icon2XHash = signPass.UseBlackVersion ? _hashOptions.Value.Icon2XBlack : _hashOptions.Value.Icon2XWhite,
                LogoHash = signPass.UseBlackVersion ? _hashOptions.Value.LogoBlack : _hashOptions.Value.LogoWhite,
                Logo2XHash = signPass.UseBlackVersion ? _hashOptions.Value.Logo2XBlack : _hashOptions.Value.Logo2XWhite
            };

            // Sign the manifest and get the signature
            byte[] signature = _signingService.SignManifest(manifest);

            return new FileStreamResult(new MemoryStream(signature), "application/pkcs7-signature");
        }
    }
}