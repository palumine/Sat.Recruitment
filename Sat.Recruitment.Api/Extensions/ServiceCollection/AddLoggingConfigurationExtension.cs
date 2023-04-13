using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Sat.Recruitment.Api.Extensions.ServiceCollection
{
    [ExcludeFromCodeCoverage]
    public static class AddLoggingConfigurationExtension
    {
        public static void AddLoggingConfiguration(this IServiceCollection services)
        {
            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestMethod & HttpLoggingFields.RequestPath & HttpLoggingFields.RequestProperties & HttpLoggingFields.RequestBody;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });
        }
    }
}
