using Sat.Recruitment.Api.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Repository
{
    public interface IUserRepository
    {
        public Task<User> CreateAsync(User user);

        public Task<IEnumerable<User>> GetAllAsync();
    }
}
