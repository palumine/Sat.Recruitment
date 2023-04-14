using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Domain;
using Sat.Recruitment.Repositories;
using Sat.Recruitment.Repositories.EF;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Sat.Recruitment.Test.Repositories
{
    public class UserRepositoryEFTest
    {
        protected readonly SatRecruitmentDBContext _dbContext;
        protected readonly ILogger<UserRepositoryEF> _logger;


        public UserRepositoryEFTest()
        {
            var dbContextOptions = new DbContextOptionsBuilder<SatRecruitmentDBContext>()
            .UseSqlite(connectionString: @"FILENAME=Repositories\TestFiles\recruitment.db",
                sqliteOptionsAction: option =>
                {
                    option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                })
            .Options;

            this._dbContext = new SatRecruitmentDBContext(dbContextOptions);


            
            


            this._dbContext.Database.EnsureCreated();
            this._dbContext.Users.RemoveRange(this._dbContext.Users);
            this._dbContext.SaveChanges();

            this._logger = new LoggerFactory().CreateLogger<UserRepositoryEF>();
        }

        protected IEnumerable<User> InitialUsers =>
            new List<User>
            {
                new User("Juan", "Juan@marmol.com", "Peru 2464", "+5491154762312", UserType.Normal, 1234m),
                new User("Franco", "Franco.Perez@gmail.com", "Alvear y Colombres", "+534645213542", UserType.Premium, 112234m),
                new User("Agustina", "Agustina@gmail.com", "Garay y Otra Calle", "+534645213542", UserType.SuperUser, 112234m),
            };

    }

    [CollectionDefinition("Sequential", DisableParallelization = true)]
    public class CreateUserEFTest : UserRepositoryEFTest
    {

        public CreateUserEFTest(): base() { }

        [Fact]
        public async Task CreateUserAsync_ShouldAddUserToRepository()
        {
            var user = new User("Juan", "Juan @marmol.com", "Peru 2464", "+5491154762312", UserType.Normal, 1234m);

            var repository = new UserRepositoryEF(this._dbContext, this._logger);

            await repository.CreateAsync(user);

            var users = await repository.GetAllAsync();

            Assert.NotEmpty(users);

            var persistedUser = users.First();
            Assert.Equal(user.Name, persistedUser.Name);
            Assert.Equal(user.NormalizedEmail, persistedUser.Email); //Persisted Email is normalized
            Assert.Equal(user.Phone, persistedUser.Phone);
            Assert.Equal(user.Address, persistedUser.Address);
            Assert.Equal(user.UserType, persistedUser.UserType);
            Assert.Equal(user.Money, persistedUser.Money);
        } 
    }

    [CollectionDefinition("Sequential", DisableParallelization = true)]

    public class CreateDuplicateUserEFTest : UserRepositoryEFTest
    {
        public CreateDuplicateUserEFTest() : base() { }


        [Fact]
        public async void CreateUserAsync_ShouldThrowDuplicateUserException()
        {
            await this._dbContext.Users.AddRangeAsync(this.InitialUsers);
            await this._dbContext.SaveChangesAsync();

            var user = new User("Juan", "Juan @marmol.com", "Peru 2464", "+5491154762312", UserType.Normal, 1234m);

            var repository = new UserRepositoryEF(this._dbContext, this._logger);

            await Assert.ThrowsAsync<DuplicateUserException>(() => repository.CreateAsync(user));
        }
    }

    [CollectionDefinition("Sequential", DisableParallelization = true)]

    public class GetAllEFTest : UserRepositoryEFTest
    {
        public GetAllEFTest() : base() { }

        [Fact]
        public async void GetAllAsync_ShouldGetAllUsersFromRepository()
        {
            await this._dbContext.Users.AddRangeAsync(this.InitialUsers);
            await this._dbContext.SaveChangesAsync();

            var repository = new UserRepositoryEF(this._dbContext, this._logger);

            var users = await repository.GetAllAsync();

            Assert.NotEmpty(users);
            Assert.Contains(users, item => item.Name == "Juan");
            Assert.Contains(users, item => item.Name == "Franco");
            Assert.Contains(users, item => item.Name == "Agustina");
        }

    }
}
