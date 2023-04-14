using Sat.Recruitment.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Repositories
{
    public interface IUserRepository
    {
        public Task<User> CreateAsync(User user);

        public Task<IEnumerable<User>> GetAllAsync();
    }
}
