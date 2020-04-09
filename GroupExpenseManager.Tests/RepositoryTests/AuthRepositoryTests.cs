using System;
using System.Linq;
using System.Threading.Tasks;
using GroupExpenseManager.API.Database.Context;
using GroupExpenseManager.API.Database.Repository;
using GroupExpenseManager.API.Models;
using GroupExpenseManager.Tests.RepositoryTests.Helper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GroupExpenseManager.Tests.RepositoryTests
{
    public class AuthRepositoryTests
    {
        private DataContext _context;
        private readonly IAuthRepository _authRepository;
        private Mock<ILogger<AuthRepository>> _loggerMock;

        public AuthRepositoryTests()
        {
           InitializeMocks();
            _authRepository = new AuthRepository(_context,_loggerMock.Object);
        }

        private void InitializeMocks()
        {
            _loggerMock = new Mock<ILogger<AuthRepository>>();
            _context = new InMemoryDbContextFactory().GetInMemoryDbContext();
        }

        [Fact]
        public async void RegisterUserTest()
        {
            User newUser = GetDummyUser();
            var user = await _authRepository.Register(newUser, "testPassword");

            Assert.True(user.Id > 0);
        }

        [Fact]
        public async void GetUserByIdTest()
        {
            await AddDummyUsers();
            var userId = _context.Users.First().Id;
            var user = await _authRepository.GetUser(userId);
            Assert.NotNull(user);
        }

        [Fact]
        public async void GetUserByIdNotInDbTest()
        {
            await AddDummyUsers();
            var user = await _authRepository.GetUser(200);
            Assert.Null(user);
        }

        [Fact]
        public async void GetUserByUserNameTest()
        {
            await AddDummyUsers();
            var userName = _context.Users.First().UserName;
            var user = await _authRepository.GetUser(userName);
            Assert.NotNull(user);
        }

        [Fact]
        public async void GetUserByUserNameNotInDbTest()
        {
            await AddDummyUsers();
            var user = await _authRepository.GetUser("invalidUser@test.com");
            Assert.Null(user);
        }

        [Fact]
        public async void LoginSuccessTest()
        {
            await AddDummyUsers();
            var dummyUser = GetDummyUser();
             var user = await _authRepository.Login(dummyUser.UserName,"testPassword");
            Assert.NotNull(user);
        }

        [Fact]
        public async void LoginFailureTest()
        {
            var dummyUser = GetDummyUser();
            var user = await _authRepository.Login(dummyUser.UserName,"incorrectPassword");
            Assert.Null(user);
            user = await _authRepository.Login("invalidUser@test.com","testPassword");
            Assert.Null(user);
            user = await _authRepository.Login("invalidUser@test.com","incorrectPassword");
            Assert.Null(user);
        }

        private async Task AddDummyUsers()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            
            var users = new[] {
                new User{
                FirstName = "FirstPerson",
                LastName ="FirstPerson",
                UserName="firstUserName@test.com",
                DateOfBirth=DateTime.Now.AddYears(-18),
                Gender=Gender.Female,
                RegisteredDate=DateTime.Now
                },
                new User{
                FirstName = "SecondPerson",
                LastName ="SecondPerson",
                UserName="secondUserName@test.com",
                DateOfBirth=DateTime.Now.AddYears(-25),
                Gender=Gender.Male,
                RegisteredDate=DateTime.Now
                }
            };
            if (_context.Users.Count() == 0)
            {
               foreach(User user in users)
               {
                   await _authRepository.Register(user,"testPassword");
               }
            }
        }        

        private User GetDummyUser()
        {
            return new User()
            {
                FirstName = "FirstPerson",
                LastName = "FirstPerson",
                UserName = "firstUserName@test.com",
                DateOfBirth = DateTime.Now.AddYears(-18),
                Gender = Gender.Female,
                RegisteredDate = DateTime.Now
            };
        }
    }
}