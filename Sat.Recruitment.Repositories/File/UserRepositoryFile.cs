using Microsoft.Extensions.Logging;
using Sat.Recruitment.Domain;

namespace Sat.Recruitment.Repositories.File
{
    public class UserRepositoryFile : IUserRepository
    {
        private readonly ILogger<UserRepositoryFile> _logger;
        private readonly string _path;

        private readonly List<User> _users = new();
        public UserRepositoryFile(ILogger<UserRepositoryFile> logger, string path)
        {
            this._path = Path.Combine(Directory.GetCurrentDirectory(), path);
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

            this._users.Add(newUser);
            await this.SaveToFile(newUser);

            return newUser;
        }


        private async Task<bool> UserIsDuplicated(User newUser)
        {
            var users = await this.GetUsers();
            return users.Any(user =>
                    (user.Email == newUser.Email || user.Phone == newUser.Phone) ||
                    (user.Name == newUser.NormalizedEmail && user.Address == newUser.Address)
                    );
        }

        private async Task SaveToFile(User newUser)
        {
            var path = this._path;

            // This text is always added, making the file longer over time
            // if it is not deleted.
            using var sw = System.IO.File.AppendText(path);

            var strUser = string.Join(',', new string[] {
                newUser.Name,
                newUser.NormalizedEmail,
                newUser.Phone,
                newUser.Address,
                newUser.UserType.ToString(),
                newUser.Money.ToString() });

            await sw.WriteAsync(strUser + sw.NewLine);
            sw.Close();
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await this.GetUsers();

        private async Task<List<User>> GetUsers()
        {
            if (this._users == null)
            {
                await this.LoadUsersFromFile();
            }
            return this._users!;
        }

        private async Task LoadUsersFromFile()
        {
            var path = this._path;

            using var fileStream = new FileStream(path, FileMode.Open);

            using var reader = new StreamReader(fileStream);

            while (reader.Peek() >= 0)
            {
                var line = await reader.ReadLineAsync();
                var user = new User
                (
                    line.Split(',')[0].ToString(),
                    line.Split(',')[1].ToString(),
                    line.Split(',')[3].ToString(),
                    line.Split(',')[2].ToString(),
                    Enum.Parse<UserType>(line.Split(',')[4].ToString()),
                    decimal.Parse(line.Split(',')[5].ToString())
                );
                this._users.Add(user);
            }
            reader.Close();
            fileStream.Close();
        }
    }
}
