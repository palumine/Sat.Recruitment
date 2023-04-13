using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpPost]
        [Route("/create-user")]
        public async Task<Result> CreateUser(string name, string email, string address, string phone, string userType, string money)
        {
            var errors = "";

            var newUser = new User
            {
                Name = name,
                Email = email,
                Address = address,
                Phone = phone,
                UserType = Enum.Parse<UserType>(userType),
                Money = decimal.Parse(money)
            };

            newUser.ValidateErrors(ref errors);

            if (errors != null && errors != "")
                return new Result()
                {
                    IsSuccess = false,
                    Errors = errors
                };


            newUser.ApplyGif();

            try
            {
                await this._userRepository.CreateAsync(newUser);
            }
            catch (DuplicateUserException e)
            {
                Debug.WriteLine(e.Message);

                return new Result()
                {
                    IsSuccess = false,
                    Errors = e.Message
                };
            }

            Debug.WriteLine("User Created");

            return new Result()
            {
                IsSuccess = true,
                Errors = "User Created"
            };
        }
    }

}
