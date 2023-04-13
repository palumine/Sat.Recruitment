using Sat.Recruitment.Api.Domain;
using Sat.Recruitment.Api.Repository;
using Sat.Recruitment.Api.Repository.File;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sat.Recruitment.Test.Repositories
{
    public class UserRepositoryFileTest
    {
        public UserRepositoryFileTest()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), @"Repositories\TestFiles\testFile.txt");
            File.WriteAllText(path, String.Empty);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldAddUserToRepository()
        {
            var user = new User { Name = "Juan", Email = "Juan @marmol.com", Phone = "+5491154762312", Address = "Peru 2464", UserType = UserType.Normal, Money = 1234m };
            user.ApplyGif();

            var repository = new UserRepositoryFile(@"Repositories\TestFiles\testFile.txt");

            await repository.CreateAsync(user);

            repository = new UserRepositoryFile(@"Repositories\TestFiles\testFile.txt");
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


        [Fact]
        public async void CreateUserAsync_ShouldThrowDuplicateUserException()
        {
            var user = new User { Name = "Juan", Email = "Juan @marmol.com", Phone = "+5491154762312", Address = "Peru 2464", UserType = UserType.Normal, Money = 1234m };
            user.ApplyGif();

            var repository = new UserRepositoryFile(@"Repositories\TestFiles\defaultFile.txt");

            await Assert.ThrowsAsync<DuplicateUserException>(() => repository.CreateAsync(user));
        }

        [Fact]
        public async void CreateUserAsync_ShouldGetAllUsersFromRepository()
        {
            var user = new User { Name = "Juan", Email = "Juan @marmol.com", Phone = "+5491154762312", Address = "Peru 2464", UserType = UserType.Normal, Money = 1234m };
            user.ApplyGif();

            var repository = new UserRepositoryFile(@"Repositories\TestFiles\defaultFile.txt");

            var users = await repository.GetAllAsync();

            Assert.NotEmpty(users);
            Assert.Collection(users, item => Assert.Contains(item.Name, "Juan"),
                                     item => Assert.Contains(item.Name, "Franco"),
                                     item => Assert.Contains(item.Name, "Agustina"));

        }

    }
}
