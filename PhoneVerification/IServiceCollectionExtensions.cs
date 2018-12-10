using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using PhoneVerification.Models;

namespace PhoneVerification
{
    public static class IServiceCollectionExtensions
    {
        private const string Identifier = "PhoneVerification";

        public static IServiceCollection AddPhoneVerification(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<PhoneVerificationOptions>(configuration.GetSection(Identifier));
            
            services.AddHttpClient(Identifier);
            
            services.TryAddTransient<IPhoneVerificationClient>((sp) =>
            {
                var factory = sp.GetService<IHttpClientFactory>();
                var options = sp.GetService<IOptions<PhoneVerificationOptions>>().Value;
                return new PhoneVerificationClient(options, factory.CreateClient(Identifier));
            });

            return services;
        }
    }
}