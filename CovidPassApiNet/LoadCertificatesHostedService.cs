using System;
using System.Threading;
using System.Threading.Tasks;
using CovidPass_API.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CovidPass_API
{
    public class LoadCertificatesHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public LoadCertificatesHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var signingService = scope.ServiceProvider.GetRequiredService<SigningService>();

            await signingService.LoadAppleCaCertificate();
            await signingService.LoadAppleDeveloperCertificate();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}