using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Domain;
using Sat.Recruitment.Repositories;
using System;
using System.Collections.Generic;
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


        /// <summary>
        /// I didn't removed this method because it could be being used.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="address"></param>
        /// <param name="phone"></param>
        /// <param name="userType"></param>
        /// <param name="money"></param>
        /// <returns></returns>
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

        [HttpPost]
        public async Task<Result> CreateUserFromBody([FromBody] User newUser)
        {
            var errors = "";

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


        [HttpGet]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await this._userRepository.GetAllAsync();
        }
    }

}
