using System.Threading.Tasks;
using GroupExpenseManager.API.Models;

namespace GroupExpenseManager.API.Database.Repository
{
    public interface IAuthRepository
    {
         Task<User> Register(User user,string password);
         Task<User> Login(string userName,string password);   
         Task<User> GetUser(string userName);
         Task<User> GetUser(int userId);           
    }
}