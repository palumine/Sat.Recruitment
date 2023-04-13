using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Repository;
using Sat.Recruitment.Api.Repository.File;
using System.Diagnostics.CodeAnalysis;

namespace Sat.Recruitment.Api.Extensions.ServiceCollection
{
    [ExcludeFromCodeCoverage]
    public static class AddRepositoriesExtension
    {
        public static void AddRepositories(this IServiceCollection serviceCollection, ILoggerFactory loggerFactory)
        {
            serviceCollection.AddSingleton<IUserRepository>(new UserRepositoryFile(loggerFactory.CreateLogger<UserRepositoryFile>(), @"Files\Users.txt"));
        }

    }
}
