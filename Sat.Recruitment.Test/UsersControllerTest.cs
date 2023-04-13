using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Repository.File;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UsersControllerTest
    {
        private readonly LoggerFactory _loggerFacotry;


        public UsersControllerTest()
        {
            _loggerFacotry = new LoggerFactory();
        }
        
        
        [Fact]
        public void CreateUser_ShouldReturnSuccessTrue()
        {
            var repository = new UserRepositoryFile(_loggerFacotry.CreateLogger<UserRepositoryFile>(), @"Repositories\TestFiles\defaultFile.txt");
            var userController = new UsersController(_loggerFacotry.CreateLogger<UsersController>(), repository);

            var result = userController.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124").Result;


            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Errors);
        }

        [Fact]
        public void CreateUser_ShouldReturnDuplicateUser()
        {
            var repository = new UserRepositoryFile(_loggerFacotry.CreateLogger<UserRepositoryFile>(), @"Repositories\TestFiles\defaultFile.txt");
            var userController = new UsersController(_loggerFacotry.CreateLogger<UsersController>(), repository);

            var result = userController.CreateUser("Agustina", "Agustina@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124").Result;


            Assert.False(result.IsSuccess);
            Assert.Equal("User is duplicated", result.Errors);
        }
    }
}
