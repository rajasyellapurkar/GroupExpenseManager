using System.Threading.Tasks;
using GroupExpenseManager.API.Database.Context;
using GroupExpenseManager.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupExpenseManager.API.Database.Repository
{
    public class AuthRepository : BaseRepository,IAuthRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<AuthRepository> _logger;
        public AuthRepository(DataContext context,ILogger<AuthRepository> logger):base(context,logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> Register(User user, string password)
        {
            _logger.LogDebug(1,"Registering user with below details: ");
            LogProperties(user);
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUser(string userName)
        {
           return await _context.Users.FirstOrDefaultAsync(u=>u.UserName==userName);
        }

        public async Task<User> GetUser(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u=>u.Id==userId);
        }

        public async Task<User> Login(string userName, string password)
        {
           var userFromRepo = await GetUser(userName);
           if(userFromRepo !=null && !VerifyPasswordHash(password,userFromRepo.PasswordHash,userFromRepo.PasswordSalt))
           {
               return null;
           }
           return userFromRepo;
        }

        /// <summary>
        /// Method to get hashed password
        /// </summary>
        /// <param name="password">password to be hashed</param>
        /// <param name="passwordHash">return the hashed password as out parameter</param>
        /// <param name="passwordSalt">return the hash key as password salt</param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Method to verify the password Hash
        /// </summary>
        /// <param name="password">password by the user</param>
        /// <param name="passwordHash">hashed password from database</param>
        /// <param name="passwordSalt">salt key from database</param>
        /// <returns></returns>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}