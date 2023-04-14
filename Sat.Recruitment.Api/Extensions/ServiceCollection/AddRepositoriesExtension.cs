using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Repositories;
using Sat.Recruitment.Repositories.EF;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Sat.Recruitment.Api.Extensions.ServiceCollection
{
    [ExcludeFromCodeCoverage]
    public static class AddRepositoriesExtension
    {
        public static void AddRepositories(this IServiceCollection serviceCollection, ILoggerFactory loggerFactory)
        {
            //serviceCollection.AddSingleton<IUserRepository>(new UserRepositoryFile(loggerFactory.CreateLogger<UserRepositoryFile>(), @"Files\Users.txt"));

            serviceCollection.AddDbContext<SatRecruitmentDBContext>(options => options.UseSqlite(
                    connectionString: $@"FILENAME={Directory.GetCurrentDirectory()}\Files\users.db",
                    sqliteOptionsAction: option =>
                    {
                        option.MigrationsAssembly(typeof(UserRepositoryEF).Assembly.FullName);
                    }
                )
            );

            serviceCollection.AddTransient<IUserRepository, UserRepositoryEF>();
        }

    }
}
