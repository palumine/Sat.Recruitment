using Microsoft.Extensions.DependencyInjection;
using Sat.Recruitment.Api.Repository;
using Sat.Recruitment.Api.Repository.File;

namespace Sat.Recruitment.Api.Extensions.ServiceCollection
{
    public static class AddRepositoriesExtension
    {
        public static void AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IUserRepository>(new UserRepositoryFile(@"Files\Users.txt"));
        }

    }
}
