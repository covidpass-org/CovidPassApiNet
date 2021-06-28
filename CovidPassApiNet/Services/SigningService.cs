using System;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CovidPass_API.Exceptions;
using CovidPass_API.Options;
using CovidPass_API.Records;
using Microsoft.Extensions.Options;

namespace CovidPass_API.Services
{
    public class SigningService
    {
        private readonly IOptions<CertificateOptions> _certificateOption;
        private X509Certificate2 _appleDeveloperCertificate;
        private X509Certificate2 _appleCaCertificate;

        public SigningService(IOptions<CertificateOptions> certificateOption)
        {
            _certificateOption = certificateOption;
        }

        public async Task LoadAppleDeveloperCertificate()
        {
            var password =
                await Utils.ReadPasswordFromSecret(_certificateOption.Value.AppleDeveloperPasswordPath);

            _appleDeveloperCertificate = Utils.LoadX509EncryptedCertificate(
                _certificateOption.Value.AppleDeveloperPath,
                password);

            if (!_appleDeveloperCertificate.HasPrivateKey)
                throw new CertificateNotLoadedException(
                    "Cannot load Apple Developer Certificate due to missing Private Key. Is the password correct?");
        }

        public async Task LoadAppleCaCertificate()
        {
            _appleCaCertificate = await Utils.LoadX509CertificateAsync(
                _certificateOption.Value.AppleCaPath);
        }

        public byte[] SignManifest(Manifest manifest)
        {
            _ = _appleDeveloperCertificate ??
                throw new CertificateNotLoadedException("The Apple developer certificate isn't loaded");
            _ = _appleCaCertificate ??
                throw new CertificateNotLoadedException("The Apple ca certificate isn't loaded");

            var manifestJson = JsonSerializer.Serialize(manifest);

            // Inspired by https://github.com/tomasmcguinness/dotnet-passbook/blob/master/Passbook.Generator/PassGenerator.cs#L255
            var signing = new SignedCms(new ContentInfo(Encoding.UTF8.GetBytes(manifestJson)), true);

            var signer = new CmsSigner(SubjectIdentifierType.SubjectKeyIdentifier, _appleDeveloperCertificate)
            {
                IncludeOption = X509IncludeOption.None
            };
            signer.Certificates.Add(_appleDeveloperCertificate);
            signer.Certificates.Add(_appleCaCertificate);

            signer.SignedAttributes.Add(new Pkcs9SigningTime());
            signing.ComputeSignature(signer);

            // Return the signature file
            return signing.Encode();
        }
    }
}