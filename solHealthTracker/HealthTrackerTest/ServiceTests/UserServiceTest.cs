using HealthTracker.Exceptions;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.Auth;
using HealthTracker.Models.ENUMs;
using HealthTracker.Repositories.Classes;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Classes;
using HealthTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HealthTrackerTest.ServiceTests
{
    public class UserServiceTest
    {
        IRepository<int, User> UserRepo;
        IRepository<int, UserDetail> UserDetailRepo;

        ITokenService tokenService;
        IUserService userService;

        HealthTrackerContext context;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("UserServiceDB");
            context = new HealthTrackerContext(optionsBuilder.Options);

            UserRepo = new UserRepository(context);
            UserDetailRepo = new UserDetailRepository(context);

            Mock<IConfigurationSection> configurationJWTSection = new Mock<IConfigurationSection>();
            configurationJWTSection.Setup(x => x.Value).Returns("This is the Secret Key to generate JWT Token for SHA256");
            Mock<IConfigurationSection> configTokenSection = new Mock<IConfigurationSection>();
            configTokenSection.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSection.Object);
            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(configTokenSection.Object);
            tokenService = new TokenService(mockConfig.Object);

            userService = new UserService(UserRepo, UserDetailRepo, tokenService);

            RegisterInputDTO User = new RegisterInputDTO()
            {
                Name = "Sam",
                Age = 30,
                Gender = GenderEnum.Gender.Male.ToString(),
                Email = "sam@gmail.com",
                Phone = "8877887788",
                Password = "samaroot",
                Role = "User"
            };

            userService.RegisterUser(User);
        }

        [TearDown]
        public void Teardown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }


        [Test]
        public async Task RegisterUserSuccessTest()
        {
            // Arrange
            RegisterInputDTO User = new RegisterInputDTO()
            {
                Name = "Sam",
                Age = 30,
                Gender = GenderEnum.Gender.Male.ToString(),
                Email = "samu@gmail.com",
                Phone = "8877887788",
                Password = "samaroot",
                Role = "User"
            };

            // Action
            var result = await userService.RegisterUser(User);

            // Assert
            Assert.That(result, Is.EqualTo("Registered Successfully!"));
        }

        [Test]
        public async Task RegisterUserFailTest()
        {
            // Arrange
            RegisterInputDTO User = new RegisterInputDTO()
            {
                Name = "Sam",
                Age = 30,
                Gender = GenderEnum.Gender.Male.ToString(),
                Email = "samu@gmail.com",
                Password = "samaroot",
                Role = "User"
            };

            // Action
            var exception = Assert.ThrowsAsync<UnableToRegisterException>(async () => await userService.RegisterUser(User));
        }

        [Test]
        public async Task RegisterUserEmailExistsFailTest()
        {
            // Arrange
            RegisterInputDTO User = new RegisterInputDTO()
            {
                Name = "Sam",
                Age = 30,
                Gender = GenderEnum.Gender.Male.ToString(),
                Email = "samu@gmail.com",
                Phone = "8877887788",
                Password = "samaroot",
                Role = "User"
            };
            await userService.RegisterUser(User);

            // Action
            var exception = Assert.ThrowsAsync<UnableToRegisterException>(async () => await userService.RegisterUser(User));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Email ID already exists"));
        }

        [Test]
        public async Task RegisterUserNoPasswordFailTest()
        {
            // Arrange
            RegisterInputDTO User = new RegisterInputDTO()
            {
                Name = "Sam",
                Age = 30,
                Gender = GenderEnum.Gender.Male.ToString(),
                Email = "samu@gmail.com",
                Phone = "8877887788",
                Role = "User"
            };

            // Action
            var exception = Assert.ThrowsAsync<UnableToRegisterException>(async () => await userService.RegisterUser(User));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Not able to register at this moment"));
        }

        [Test]
        public async Task LoginUserSuccessTest()
        {
            // Arrange
            RegisterInputDTO User = new RegisterInputDTO()
            {
                Name = "Sam",
                Age = 30,
                Gender = GenderEnum.Gender.Male.ToString(),
                Email = "samu@gmail.com",
                Phone = "8877887788",
                Password = "samaroot",
                Role = "User"
            };
            await userService.RegisterUser(User);
            LoginInputDTO userLogiin = new LoginInputDTO()
            {
                Email = "samu@gmail.com",
                Password = "samaroot"
            };

            // Action
            var result = await userService.LoginUser(userLogiin);

            // Assert
            Assert.That(result.Role, Is.EqualTo("User"));
        }

        [Test]
        public async Task LoginUserIncorrectEmailExceptionTest()
        {
            // Arrange

            LoginInputDTO userLogin = new LoginInputDTO()
            {
                Email = "dana@gmail.com",
                Password = "samaroot"
            };

            // Action
            var exception = Assert.ThrowsAsync<UnauthorizedUserException>(async () => await userService.LoginUser(userLogin));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Invalid username or password"));
        }

        [Test]
        public async Task LoginUserIncorrectPasswordExceptionTest()
        {
            // Arrange

            LoginInputDTO userLogin = new LoginInputDTO()
            {
                Email = "sam@gmail.com",
                Password = "danahroot"
            };

            // Action
            var exception = Assert.ThrowsAsync<UnauthorizedUserException>(async () => await userService.LoginUser(userLogin));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Invalid username or password"));
        }

        [Test]
        public async Task UpdateUserSuccessTest()
        {
            RegisterInputDTO Coach = new RegisterInputDTO()
            {
                Name = "Sam",
                Age = 30,
                Gender = GenderEnum.Gender.Male.ToString(),
                Email = "samu@gmail.com",
                Phone = "8877887788",
                Password = "samaroot",
                Role = "Coach"
            };
            await userService.RegisterUser(Coach);
            User GetCoach = await userService.GetUserById(2);
            GetCoach.is_preferenceSet = true;

            var result = userService.UpdateUser(GetCoach);
        }

        [Test]
        public async Task UpdateUserFailTest()
        {
            User NewCoach = new User()
            {
                UserId = 10,
                Name = "Ram",
                Age = 30,
                Gender = GenderEnum.Gender.Male,
                Email = "Ramu@gmail.com",
                Phone = "8877887788",
                Role = RolesEnum.Roles.Coach,
                is_preferenceSet = false,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };

            var exception = Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await userService.UpdateUser(NewCoach));
        }

        [Test]
        public async Task GetUserByIDSuccessTest()
        {
            var User = await userService.GetUserById(1);

            Assert.That(User.Role, Is.EqualTo(RolesEnum.Roles.User));
        }

        [Test]
        public async Task GetUserByIDFailTest()
        {

            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await userService.GetUserById(100));
        }
    }
}