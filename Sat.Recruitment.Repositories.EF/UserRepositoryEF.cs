using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Domain;

namespace Sat.Recruitment.Repositories.EF
{
    public class UserRepositoryEF : IUserRepository
    {
        private readonly SatRecruitmentDBContext _dbContext;
        private readonly ILogger<UserRepositoryEF> _logger;

        public UserRepositoryEF(SatRecruitmentDBContext dbContext, ILogger<UserRepositoryEF> logger) 
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }


        public async Task<User> CreateAsync(User newUser)
        {
            var isDuplicated = await this.UserIsDuplicated(newUser);

            if (isDuplicated)
            {
                this._logger.LogDebug("User is duplicated");
                throw new DuplicateUserException();
            }

            newUser.Email = newUser.NormalizedEmail;
            await this._dbContext.Users.AddAsync(newUser);
            await this._dbContext.SaveChangesAsync();
            return newUser;
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await _dbContext.Users.ToListAsync();

        private async Task<bool> UserIsDuplicated(User newUser)
        {
            return await _dbContext.Users.AnyAsync(user =>
                    (user.Email == newUser.NormalizedEmail || user.Phone == newUser.Phone) ||
                    (user.Name == newUser.Name && user.Address == newUser.Address)
                    );
        }

    }
}
