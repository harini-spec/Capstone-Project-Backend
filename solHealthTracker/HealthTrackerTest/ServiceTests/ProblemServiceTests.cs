using HealthTracker.Exceptions;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.Suggestions;
using HealthTracker.Models.DTOs.Target;
using HealthTracker.Models.ENUMs;
using HealthTracker.Repositories.Classes;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Classes;
using HealthTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthTrackerTest.ServiceTests
{
    public class ProblemServiceTests
    {
        IRepository<int, HealthLog> HealthLogRepository;
        IRepository<int, MonitorPreference> MonitorPreferenceRepository;
        IRepository<int, Suggestion> SuggestionRepository;
        IRepository<int, UserPreference> UserPreferenceRepository;
        IRepository<int, Metric> MetricRepository;
        IRepository<int, User> UserRepository;

        IMetricService MetricService;
        IUserService UserService;
        IProblemService ProblemService;

        HealthTrackerContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("ProblemServiceDB");
            context = new HealthTrackerContext(optionsBuilder.Options);

            HealthLogRepository = new HealthLogRepository(context);
            MonitorPreferenceRepository = new MonitorPreferenceRepository(context);
            SuggestionRepository = new SuggestionRepository(context);
            UserPreferenceRepository = new UserPreferenceRepository(context);
            MetricRepository = new MetricRepository(context);
            UserRepository = new UserRepository(context);

            UserService = new UserService(UserRepository, null, null);
            MetricService = new MetricService(UserPreferenceRepository, null, MetricRepository, null);
            ProblemService = new ProblemService(HealthLogRepository, MonitorPreferenceRepository, MetricService, SuggestionRepository, UserService);

            User User1 = new User()
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
            await UserRepository.Add(User1);
            User coach1 = new User()
            {
                UserId = 2,
                Name = "Pam",
                Age = 30,
                Gender = GenderEnum.Gender.Female,
                Email = "pam@gmail.com",
                Phone = "8877887788",
                Role = RolesEnum.Roles.Coach,
                is_preferenceSet = false,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await UserRepository.Add(coach1);

            Metric metric1 = new Metric()
            {
                Id = 1,
                MetricType = "Sleep_Hours",
                MetricUnit = "Hours",
                Created_at = DateTime.UtcNow,
                Updated_at = DateTime.UtcNow
            };
            await MetricRepository.Add(metric1);
            Metric metric2 = new Metric()
            {
                Id = 2,
                MetricType = "Height",
                MetricUnit = "Meters",
                Created_at = DateTime.UtcNow,
                Updated_at = DateTime.UtcNow
            };
            await MetricRepository.Add(metric2);

            UserPreference userPreference1 = new UserPreference()
            {
                Id = 1,
                MetricId = 1,
                UserId = 1,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await UserPreferenceRepository.Add(userPreference1);
            UserPreference userPreference2 = new UserPreference()
            {
                Id = 2,
                MetricId = 2,
                UserId = 1,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await UserPreferenceRepository.Add(userPreference2);

            HealthLog healthLog1 = new HealthLog()
            {
                Id = 1,
                HealthStatus = HealthStatusEnum.HealthStatus.Fair,
                PreferenceId = 1,
                value = 8,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await HealthLogRepository.Add(healthLog1);

            MonitorPreference monitorPref = new MonitorPreference()
            {
                CoachId = 2,
                MetricId = 1,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await MonitorPreferenceRepository.Add(monitorPref);
        }

        [TearDown]
        public void Teardown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task GetUserIdsWithProblemsSuccessTest()
        {
            // Arrange
            HealthLog healthLog = new HealthLog()
            {
                HealthStatus = HealthStatusEnum.HealthStatus.Poor,
                PreferenceId = 1,
                value = 8,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await HealthLogRepository.Add(healthLog);

            // Action
            var result = await ProblemService.GetUserIdsWithProblems(2);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetUserIdsWithProblemsSuccessTestWithPreExistingSuggestionForUser()
        {
            // Arrange
            HealthLog healthLog = new HealthLog()
            {
                HealthStatus = HealthStatusEnum.HealthStatus.Poor,
                PreferenceId = 1,
                value = 8,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await HealthLogRepository.Add(healthLog);
            SuggestionInputDTO suggestionInputDTO = new SuggestionInputDTO()
            {
                UserId = 1,
                Suggestion = "Sleep Early"
            };
            await ProblemService.AddSuggestion(suggestionInputDTO, 2);

            // Action
            var result = await ProblemService.GetUserIdsWithProblems(2);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetUserIdsWithProblemsSuccessTestWithPreExistingSuggestionForDiffDate()
        {
            // Arrange
            HealthLog healthLog = new HealthLog()
            {
                HealthStatus = HealthStatusEnum.HealthStatus.Poor,
                PreferenceId = 1,
                value = 8,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await HealthLogRepository.Add(healthLog);
            SuggestionInputDTO suggestionInputDTO = new SuggestionInputDTO()
            {
                UserId = 1,
                Suggestion = "Sleep Early"
            };
            await ProblemService.AddSuggestion(suggestionInputDTO, 2);
            var suggestion = await SuggestionRepository.GetById(1);
            suggestion.Created_at = DateTime.Now.AddDays(-1);
            suggestion.Updated_at = DateTime.Now.AddDays(-1);
            await SuggestionRepository.Update(suggestion);

            // Action
            var result = await ProblemService.GetUserIdsWithProblems(2);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetUserIdsWithProblemsNoLogsForTodayExceptionTest()
        {
            // Arrange 
            HealthLog healthLog = new HealthLog()
            {
                HealthStatus = HealthStatusEnum.HealthStatus.Poor,
                PreferenceId = 1,
                value = 8,
                Created_at = DateTime.Now.AddDays(-1),
                Updated_at = DateTime.Now.AddDays(-1)
            };
            await HealthLogRepository.Add(healthLog);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await ProblemService.GetUserIdsWithProblems(2));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No Problems Logs for today!"));
        }

        [Test]
        public async Task GetUserIdsWithProblemsNoProblemsForTodayExceptionTest()
        {
            // Arrange 
            HealthLog healthLog1 = new HealthLog()
            {
                HealthStatus = HealthStatusEnum.HealthStatus.Poor,
                PreferenceId = 2,
                value = 8,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await HealthLogRepository.Add(healthLog1);
            HealthLog healthLog2 = new HealthLog()
            {
                HealthStatus = HealthStatusEnum.HealthStatus.Excellent,
                PreferenceId = 1,
                value = 8,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await HealthLogRepository.Add(healthLog2);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await ProblemService.GetUserIdsWithProblems(2));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No Problems Logs for today!"));
        }

        [Test]
        public async Task GetUserIdsWithProblemsNoMonitorPrefsSetExceptionTest()
        {
            // Arrange 
            await MonitorPreferenceRepository.Delete(1);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await ProblemService.GetUserIdsWithProblems(2));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No Monitor Preferences found!"));
        }

        [Test]
        public async Task GetUserIdsWithProblemsMonitorPrefNotSetForCoachExceptionTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await ProblemService.GetUserIdsWithProblems(3));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Coach Preferences not set!"));
        }

        [Test]
        public async Task AddSuggestionSuccessTest()
        {
            // Arrange
            SuggestionInputDTO suggestionInputDTO = new SuggestionInputDTO()
            {
                UserId = 1,
                Suggestion = "Sleep Early"
            };

            // Action
            var result = await ProblemService.AddSuggestion(suggestionInputDTO, 2);

            // Assert
            Assert.That(result, Is.EqualTo("Successfully Added!"));
        }

        [Test]
        public async Task AddSuggestionForCoachFailTest()
        {
            // Arrange
            SuggestionInputDTO suggestionInputDTO = new SuggestionInputDTO()
            {
                UserId = 2,
                Suggestion = "Sleep Early"
            };

            // Action
            var exception = Assert.ThrowsAsync<InvalidActionException>(async () => await ProblemService.AddSuggestion(suggestionInputDTO, 2));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Can't add suggestions for Coach!"));
        }

        [Test]
        public async Task GetUserSuggestionsSuccessTest()
        {
            // Arrange
            SuggestionInputDTO suggestionInputDTO = new SuggestionInputDTO()
            {
                UserId = 1,
                Suggestion = "Sleep Early"
            };
            await ProblemService.AddSuggestion(suggestionInputDTO, 2);

            // Action
            var result = await ProblemService.GetUserSuggestions(1);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetUserSuggestionsFailTest()
        {
            // Arrange
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
            SuggestionInputDTO suggestionInputDTO = new SuggestionInputDTO()
            {
                UserId = 3,
                Suggestion = "Sleep Early"
            };
            await ProblemService.AddSuggestion(suggestionInputDTO, 2);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await ProblemService.GetUserSuggestions(1));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No Suggestions Found!"));
        }

        [Test]
        public async Task GetCoachSuggestionsSuccessTest()
        {
            // Arrange
            SuggestionInputDTO suggestionInputDTO = new SuggestionInputDTO()
            {
                UserId = 1,
                Suggestion = "Sleep Early"
            };
            await ProblemService.AddSuggestion(suggestionInputDTO, 2);

            // Action
            var result = await ProblemService.GetCoachSuggestionsForUser(1, 2);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetCoachSuggestionsFailTest()
        {
            // Arrange
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
            SuggestionInputDTO suggestionInputDTO = new SuggestionInputDTO()
            {
                UserId = 3,
                Suggestion = "Sleep Early"
            };
            await ProblemService.AddSuggestion(suggestionInputDTO, 2);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await ProblemService.GetCoachSuggestionsForUser(1, 2));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No Suggestions Found!"));
        }
    }
}
