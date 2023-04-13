using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Domain;
using Sat.Recruitment.Api.Repository;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Errors { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;

        public UsersController(
            ILogger<UsersController> logger,
            IUserRepository userRepository)
        {
            this._logger = logger;
            this._userRepository = userRepository;
        }

        [HttpPost]
        [Route("/create-user")]
        public async Task<Result> CreateUser(string name, string email, string address, string phone, string userType, string money)
        {
            var errors = "";

            var newUser = new User(name, email, address, phone, 
                Enum.Parse<UserType>(userType), decimal.Parse(money));

            newUser.ValidateErrors(ref errors);

            if (errors != null && errors != "")
                return new Result()
                {
                    IsSuccess = false,
                    Errors = errors
                };

            try
            {
                await this._userRepository.CreateAsync(newUser);
            }
            catch (DuplicateUserException e)
            {
                var message = e.Message;
                this._logger.LogDebug(message);

                return new Result()
                {
                    IsSuccess = false,
                    Errors = message
                };
            }

            this._logger.LogDebug("User Created");

            return new Result()
            {
                IsSuccess = true,
                Errors = "User Created"
            };
        }
    }

}
