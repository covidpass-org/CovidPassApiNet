using System;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CovidPass_API
{
    public static class Utils
    {
        public static async Task<string> ReadPasswordFromSecret(string secretPath)
        {
            return (await File.ReadAllTextAsync(secretPath)).Trim();
        }

        public static async Task<X509Certificate2> LoadX509CertificateAsync(string filePath)
        {
            var certificateBytes = await File.ReadAllBytesAsync(filePath);
            return new X509Certificate2(certificateBytes);
        }

        public static X509Certificate2 LoadX509EncryptedCertificate(string filePath, string password)
        {
            return X509Certificate2.CreateFromEncryptedPemFile(filePath, password);
        }
    }
}