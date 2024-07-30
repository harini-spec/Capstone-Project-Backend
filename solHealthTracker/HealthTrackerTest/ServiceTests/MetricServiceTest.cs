using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.Auth;
using HealthTracker.Models.ENUMs;
using HealthTracker.Models;
using HealthTracker.Repositories.Classes;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Classes;
using HealthTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthTracker.Exceptions;

namespace HealthTrackerTest.ServiceTests
{
    public class MetricServiceTest
    {
        IRepository<int, UserPreference> UserPreferenceRepository;
        IRepository<int, MonitorPreference> MonitorPreferenceRepository;
        IRepository<int, Metric> MetricRepository;
        IRepository<int, User> UserRepository;

        IUserService UserService;
        IMetricService MetricService;

        HealthTrackerContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("MetricDB");
            context = new HealthTrackerContext(optionsBuilder.Options);

            UserPreferenceRepository = new UserPreferenceRepository(context);
            MonitorPreferenceRepository = new MonitorPreferenceRepository(context);
            MetricRepository = new MetricRepository(context);
            UserRepository = new UserRepository(context);

            UserService = new UserService(UserRepository, null, null);
            MetricService = new MetricService(UserPreferenceRepository, MonitorPreferenceRepository, MetricRepository, UserService);

            Metric metric1 = new Metric()
            {
                Id = 1,
                MetricType = "Height",
                MetricUnit = "Meters",
                Created_at = DateTime.UtcNow,
                Updated_at = DateTime.UtcNow
            };
            await MetricRepository.Add(metric1);
            Metric metric3 = new Metric()
            {
                Id = 3,
                MetricType = "Weight",
                MetricUnit = "Kg",
                Created_at = DateTime.UtcNow,
                Updated_at = DateTime.UtcNow
            };
            await MetricRepository.Add(metric3);
            Metric metric2 = new Metric()
            {
                Id = 2,
                MetricType = "Sleep_Hours",
                MetricUnit = "Hours",
                Created_at = DateTime.UtcNow,
                Updated_at = DateTime.UtcNow
            };
            await MetricRepository.Add(metric2);
            User User = new User()
            {
                UserId = 1,
                Name = "Sam",
                Age = 30,
                Gender = GenderEnum.Gender.Male,
                Email = "sam@gmail.com",
                Phone = "8877887788",
                Role = RolesEnum.Roles.User,
                is_preferenceSet = false,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await UserRepository.Add(User);
            User Coach = new User()
            {
                UserId = 2,
                Name = "Sam",
                Age = 30,
                Gender = GenderEnum.Gender.Male,
                Email = "samu@gmail.com",
                Phone = "8877887788",
                Role = RolesEnum.Roles.Coach,
                is_preferenceSet = false,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await UserRepository.Add(Coach);
        }

        [TearDown]
        public void Teardown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task GetPrefDTOByPrefIdSuccessTest()
        {
            // Arrange
            List<string> prefs = new List<string>
            {
                "Height"
            };
            await MetricService.AddPreference(prefs, 1, "User");

            //Action
            var result = await MetricService.GetPreferenceDTOByPrefId(1);

            // Assert
            Assert.That(result.MetricType, Is.EqualTo("Height"));
        }

        [Test]
        public async Task GetPrefDTOByPrefIdFailTest()
        { 
            //Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await MetricService.GetPreferenceDTOByPrefId(100));
        }

        [Test]
        public async Task AddUserPreferenceSuccessTest()
        {
            // Arrange
            List<string> prefs = new List<string>
            {
                "Height"
            };

            // Action
            var result = await MetricService.AddPreference(prefs, 1, "User");

            // Assert
            Assert.That(result, Is.EqualTo("Successfully added"));
        }

        [Test]
        public async Task AddCoachPreferenceSuccessTest()
        {
            // Arrange
            List<string> prefs = new List<string>
            {
                "Height"
            };

            // Action
            var result = await MetricService.AddPreference(prefs, 1, "Coach");

            // Assert
            Assert.That(result, Is.EqualTo("Successfully added"));
        }

        [Test]
        public async Task AddPreferenceUserNotFoundFailTest()
        {
            // Arrange
            List<string> prefs = new List<string>
            {
                "Height"
            };

            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await MetricService.AddPreference(prefs, 100, "Coach"));
        }

        [Test]
        public async Task AddUserPreferenceAlreadyExistsFailTest()
        {
            // Arrange
            List<string> prefs = new List<string>
            {
                "Sleep_Hours"
            };
            await MetricService.AddPreference(prefs, 1, "User");

            List<string> Newprefs = new List<string>
            {
                "Sleep_Hours"
            };

            // Action
            var exception = Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await MetricService.AddPreference(Newprefs, 1, "User"));
        }

        [Test]
        public async Task AddCoachPreferenceAlreadyExistsFailTest()
        {
            // Arrange
            List<string> prefs = new List<string>
            {
                "Height"
            };
            await MetricService.AddPreference(prefs, 2, "Coach");

            // Action
            var exception = Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await MetricService.AddPreference(prefs, 2, "Coach"));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Some monitor Preferences already exist. Choose again!"));
        }

        [Test]
        public async Task FindMetricByMetricTypeSuccessTest()
        {
            // Action
            var result = await MetricService.FindMetricByMetricType("Height");

            // Assert
            Assert.That(result.MetricType, Is.EqualTo("Height"));
        }

        [Test]
        public async Task FindMetricByMetricTypeFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await MetricService.FindMetricByMetricType("BMI"));
        }

        [Test]
        public async Task FindMetricByMetricTypeNoMetricsExceptionTest()
        {
            // Arrange
            await MetricRepository.Delete(1);
            await MetricRepository.Delete(2);
            await MetricRepository.Delete(3);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await MetricService.FindMetricByMetricType("BMI"));
        }

        [Test]
        public async Task GetPreferencesListOfUserSuccessTest()
        {
            // Arrange
            List<string> prefs = new List<string>
            {
                "Height"
            };
            await MetricService.AddPreference(prefs, 1, "User");

            // Action
            var result = await MetricService.GetPreferencesListOfUser(1, "User");

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetPreferencesListOfCoachSuccessTest()
        {
            // Arrange
            List<string> prefs = new List<string>
            {
                "Height"
            };
            await MetricService.AddPreference(prefs, 2, "Coach");

            // Action
            var result = await MetricService.GetPreferencesListOfUser(2, "Coach");

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetPreferencesListOfUserFailTest()
        {
            User User = new User()
            {
                UserId = 3,
                Name = "Sam",
                Age = 30,
                Gender = GenderEnum.Gender.Male,
                Email = "sam@gmail.com",
                Phone = "8877887788",
                Role = RolesEnum.Roles.User,
                is_preferenceSet = false,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await UserRepository.Add(User);
            List<string> prefs = new List<string>
            {
                "Height"
            };
            await MetricService.AddPreference(prefs, 3, "User");

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await MetricService.GetPreferencesListOfUser(1, "User"));
        }

        [Test]
        public async Task GetPreferencesListOfCoachFailTest()
        {
            User Coach = new User()
            {
                UserId = 4,
                Name = "Sam",
                Age = 30,
                Gender = GenderEnum.Gender.Male,
                Email = "samu@gmail.com",
                Phone = "8877887788",
                Role = RolesEnum.Roles.Coach,
                is_preferenceSet = false,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await UserRepository.Add(Coach);
            List<string> prefs = new List<string>
            {
                "Height"
            };
            await MetricService.AddPreference(prefs, 4, "Coach");

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await MetricService.GetPreferencesListOfUser(2, "Coach"));
        }

        [Test]
        public async Task GetMetricByIdSuccessTest()
        {
            var result = await MetricService.GetMetricById(1);

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetMetricByIdFailTest()
        {
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await MetricService.GetMetricById(100));
        }

        [Test]
        public async Task FindUserPreferenceByPreferenceIdSuccessTest()
        {
            List<string> prefs = new List<string>
            {
                "Height"
            };
            await MetricService.AddPreference(prefs, 1, "User");

            var result = await MetricService.GetMetricById(1);

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task FindUserPreferenceByPreferenceIdFailTest()
        {
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await MetricService.FindUserPreferenceByPreferenceId(100));
        }

        [Test]
        public async Task GetAllMetricsSuccessTest()
        {
            List<string> prefs = new List<string>
            {
                "Sleep_Hours"
            };
            await MetricService.AddPreference(prefs, 1, "User");

            var result = await MetricService.GetAllNotSelectedPrefs(1, "User");

            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllMetricsNoPrefSuccessTest()
        {
            var result = await MetricService.GetAllNotSelectedPrefs(1, "User");

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllMetricsFailTest()
        {
            await MetricRepository.Delete(1);
            await MetricRepository.Delete(2);
            await MetricRepository.Delete(3);

            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await MetricService.GetAllNotSelectedPrefs(1, "User"));
        }

        [Test]
        public async Task GetMetricIdFromPreferenceIdSuccessTest()
        {
            List<string> prefs = new List<string>
            {
                "Height"
            };
            await MetricService.AddPreference(prefs, 1, "User");

            var result = await MetricService.GetMetricIdFromPreferenceId(1);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task GetMetricIdFromPreferenceIdFailTest()
        {
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await MetricService.GetMetricIdFromPreferenceId(100));
        }

        [Test]
        public async Task FindPreferenceIdFromMetricTypeAndUserIdSuccessTest()
        {
            List<string> prefs = new List<string>
            {
                "Height"
            };
            await MetricService.AddPreference(prefs, 1, "User");

            var result = await MetricService.FindPreferenceIdFromMetricTypeAndUserId("Height", 1);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task FindPreferenceIdFromMetricTypeAndUserIdNoMetricFailTest()
        {
            List<string> prefs = new List<string>
            {
                "Height"
            };
            await MetricService.AddPreference(prefs, 1, "User");
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await MetricService.FindPreferenceIdFromMetricTypeAndUserId("BMI", 1));
        }

        [Test]
        public async Task FindPreferenceIdFromMetricTypeAndUserIdNoUserFailTest()
        {
            List<string> prefs = new List<string>
            {
                "Height"
            };
            await MetricService.AddPreference(prefs, 1, "User");
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await MetricService.FindPreferenceIdFromMetricTypeAndUserId("Height", 100));
        }
    }
}
