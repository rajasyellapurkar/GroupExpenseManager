using System;
using AutoMapper;
using FluentAssertions;
using GroupExpenseManager.API.Controllers;
using GroupExpenseManager.API.Database.Repository;
using GroupExpenseManager.API.Dtos;
using GroupExpenseManager.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace GroupExpenseManager.Tests.ApiControllerTests
{
    public class AuthControllerTests
    {        
        public AuthControllerTests()
        {
            InitializeAutoMapper();
            InitializeMocks();
        }
        private IMapper _mapper;
        private Mock<IConfiguration> _configMock;
        private Mock<IAuthRepository> _authRepoMock;
        private void InitializeAutoMapper()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.CreateMap<UserForCreationDto,User>();
                opts.CreateMap<User,UserToReturnDto>();
            });

            _mapper = config.CreateMapper();
        }
        private void InitializeMocks()
        {
            _configMock = new Mock<IConfiguration>();
            _authRepoMock = new Mock<IAuthRepository>();
        }
        private User GetDummyUser()
        {
            return new User()
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                UserName = "testUserName@test.com",
                DateOfBirth = DateTime.Now.AddYears(-18),
                PasswordHash = new byte[100],
                PasswordSalt = new byte[100],
                Gender = Gender.Female,
                RegisteredDate = DateTime.Now
            };
        }

        [Fact]
        public async void RegisterTest()
        {
            var dummyUser = GetDummyUser();
            _authRepoMock.Setup(auth=>auth.Register(It.IsAny<User>(),It.IsAny<string>())).ReturnsAsync(dummyUser);
            _authRepoMock.Setup(a=>a.GetUser(It.IsAny<string>())).ReturnsAsync(()=>{return null;});
            var authController = new AuthController(_authRepoMock.Object,_mapper,_configMock.Object);

            var result = await authController.Register(new UserForCreationDto{
                FirstName = "FirstName",
                LastName = "LastName",
                UserName = "testUserName@test.com",
                DateOfBirth = DateTime.Now.AddYears(-18),
                Password = "testPassword",
                Gender =  Gender.Female
            });

            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async void RegisterExisitngUserTest()
        {
            var dummyUser = GetDummyUser();
            _authRepoMock.Setup(a=>a.GetUser(It.IsAny<string>())).ReturnsAsync(dummyUser);
            _authRepoMock.Setup(auth=>auth.Register(It.IsAny<User>(),It.IsAny<string>())).ReturnsAsync(()=>{return null;});            
            var authController = new AuthController(_authRepoMock.Object,_mapper,_configMock.Object);

            var result = await authController.Register(new UserForCreationDto{
                FirstName = "FirstName",
                LastName = "LastName",
                UserName = "testUserName@test.com",
                DateOfBirth = DateTime.Now.AddYears(-18),
                Password = "testPassword",
                Gender =  Gender.Female
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void RegisterExceptionTest()
        {
            _authRepoMock.Setup(a=>a.GetUser(It.IsAny<string>())).ReturnsAsync(()=>{return null;});
            _authRepoMock.Setup(auth=>auth.Register(It.IsAny<User>(),It.IsAny<string>())).ReturnsAsync(()=>{return null;});            
            var authController = new AuthController(_authRepoMock.Object,_mapper,_configMock.Object);

            await Assert.ThrowsAsync<Exception>(()=>
            {
                return authController.Register(new UserForCreationDto{
                FirstName = "FirstName",
                LastName = "LastName",
                UserName = "testUserName@test.com",
                DateOfBirth = DateTime.Now.AddYears(-18),
                Password = "testPassword",
                Gender =  Gender.Female });
            });
        } 

        [Fact]
        public async void LoginSuccessTest()
        {
            _authRepoMock.Setup(a=>a.Login(It.IsAny<string>(),It.IsAny<string>())).ReturnsAsync(GetDummyUser());
            _configMock.Setup(c=>c.GetSection("AppSettings:Token").Value).Returns("Testing Secret Key");
            var authController = new AuthController(_authRepoMock.Object,_mapper,_configMock.Object);
            var result = await authController.Login(new UserForLoginDto{
                UserName = "testUserName@test.com",
                Password = "testPassword"});
            
            Assert.IsType<OkObjectResult>(result);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async void LoginFailureTest()
        {
            _authRepoMock.Setup(a=>a.Login(It.IsAny<string>(),It.IsAny<string>())).ReturnsAsync(()=>{return null;});
            _configMock.Setup(c=>c.GetSection("AppSettings:Token").Value).Returns("Testing Secret Key");
            var authController = new AuthController(_authRepoMock.Object,_mapper,_configMock.Object);
            var result = await authController.Login(new UserForLoginDto{
                UserName = "testUserName@test.com",
                Password = "testPassword"});
            
            Assert.IsType<UnauthorizedObjectResult>(result);
        }        
    }
}