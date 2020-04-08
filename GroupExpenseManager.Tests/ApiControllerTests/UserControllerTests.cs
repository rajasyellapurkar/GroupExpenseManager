using System;
using AutoMapper;
using GroupExpenseManager.API.Controllers;
using GroupExpenseManager.API.Database.Repository;
using GroupExpenseManager.API.Dtos;
using GroupExpenseManager.API.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GroupExpenseManager.Tests.ApiControllerTests
{
    public class UserControllerTests
    {        
        private IMapper _mapper;
        private Mock<IAuthRepository> _authRepoMock;
        public UserControllerTests()
        {
            InitializeAutoMapper();
            InitializeMocks();
        }
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
        public async void GetUserTest()
        {
            _authRepoMock.Setup(a=>a.GetUser(It.IsAny<int>())).ReturnsAsync(GetDummyUser());
            var userController = new UsersController(_authRepoMock.Object,_mapper);
            var result = await userController.GetUser(1);
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void GetInvalidResultTest()
        {
            _authRepoMock.Setup(a=>a.GetUser(It.IsAny<int>())).ReturnsAsync(()=>{return null;});
            var userController = new UsersController(_authRepoMock.Object,_mapper);
            var result = await userController.GetUser(1);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}