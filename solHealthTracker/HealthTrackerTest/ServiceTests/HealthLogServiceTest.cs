using HealthTracker.Exceptions;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.Auth;
using HealthTracker.Models.DTOs.HealthLog;
using HealthTracker.Models.ENUMs;
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

namespace HealthTrackerTest.ServiceTests
{
    public class HealthLogServiceTest
    {
        IRepository<int, HealthLog> HealthLogRepository;
        IRepository<int, IdealData> IdealDataRepository;
        IRepository<int, UserPreference> UserPreference;
        IRepository<int, MonitorPreference> MonitorPreference;
        IRepository<int, Metric> MetricRepository;
        IRepository<int, Target> TargetRepository;

        IMetricService MetricService;
        ITargetService TargetService;
        IHealthLogService HealthLogService;

        HealthTrackerContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("HealthLogDB");
            context = new HealthTrackerContext(optionsBuilder.Options);

            HealthLogRepository = new HealthLogRepository(context);
            IdealDataRepository = new IdealDataRepository(context);
            UserPreference = new UserPreferenceRepository(context);
            MonitorPreference = new MonitorPreferenceRepository(context);
            MetricRepository = new MetricRepository(context);
            TargetRepository = new TargetRepository(context);

            MetricService = new MetricService(UserPreference, MonitorPreference, MetricRepository, null);
            TargetService = new TargetService(TargetRepository, MetricService);
            HealthLogService = new HealthLogService(HealthLogRepository, IdealDataRepository, MetricService, TargetService);

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
                MetricType = "Weight",
                MetricUnit = "Kg",
                Created_at = DateTime.UtcNow,
                Updated_at = DateTime.UtcNow
            };
            await MetricRepository.Add(metric2);
            Metric metric3 = new Metric()
            {
                Id = 3,
                MetricType = "Height",
                MetricUnit = "Meters",
                Created_at = DateTime.UtcNow,
                Updated_at = DateTime.UtcNow
            };
            await MetricRepository.Add(metric3);
            Metric metric4 = new Metric()
            {
                Id = 4,
                MetricType = "BMI",
                MetricUnit = "Kg/m2",
                Created_at = DateTime.UtcNow,
                Updated_at = DateTime.UtcNow
            };
            await MetricRepository.Add(metric4);

            UserPreference userPreference1 = new UserPreference()
            {
                Id = 1,
                MetricId = 1,
                UserId = 1,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await UserPreference.Add(userPreference1);
            UserPreference userPreference2 = new UserPreference()
            {
                Id = 2,
                MetricId = 2,
                UserId = 1,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await UserPreference.Add(userPreference2);
            UserPreference userPreference3 = new UserPreference()
            {
                Id = 3,
                MetricId = 3,
                UserId = 1,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await UserPreference.Add(userPreference3);

            IdealData idealData1 = new IdealData()
            {
                MetricId = 1,
                HealthStatus = HealthStatusEnum.HealthStatus.Fair,
                MinVal = 8,
                MaxVal = 9,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await IdealDataRepository.Add(idealData1);
            IdealData idealData4 = new IdealData()
            {
                MetricId = 4,
                HealthStatus = HealthStatusEnum.HealthStatus.Fair,
                MinVal = 18,
                MaxVal = 23,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await IdealDataRepository.Add(idealData4);
        }

        [TearDown]
        public void Teardown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddHealthLogSuccessTest()
        {
            // Arrange
            AddHealthLogInputDTO addHealthLogInputDTO = new AddHealthLogInputDTO()
            {
                PreferenceId = 1,
                value = 8
            };

            // Action
            var result = await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1);

            // Assert
            Assert.That(result.HealthStatus, Is.EqualTo("Fair"));
        }

        [Test]
        public async Task AddHealthLogNoIdealValueFoundForWeightUpperLimitExceptionTest()
        {
            // Arrange
            AddHealthLogInputDTO addHealthLogInputDTO1 = new AddHealthLogInputDTO()
            {
                PreferenceId = 3,
                value = float.Parse("1.5")
            };
            await HealthLogService.AddHealthLog(addHealthLogInputDTO1, 1);
            AddHealthLogInputDTO addHealthLogInputDTO2 = new AddHealthLogInputDTO()
            {
                PreferenceId = 2,
                value = 70
            };

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await HealthLogService.AddHealthLog(addHealthLogInputDTO2, 1));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No Ideal Value data found!"));
        }

        [Test]
        public async Task AddHealthLogNoIdealValueFoundUpperLimitExceptionTest()
        {
            // Arrange
            AddHealthLogInputDTO addHealthLogInputDTO = new AddHealthLogInputDTO()
            {
                PreferenceId = 1,
                value = 12
            };

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No Ideal Value data found!"));
        }

        [Test]
        public async Task AddHealthLogNoIdealValueFoundForWeightLowerLimitExceptionTest()
        {
            // Arrange
            AddHealthLogInputDTO addHealthLogInputDTO1 = new AddHealthLogInputDTO()
            {
                PreferenceId = 3,
                value = float.Parse("1.5")
            };
            await HealthLogService.AddHealthLog(addHealthLogInputDTO1, 1);
            AddHealthLogInputDTO addHealthLogInputDTO2 = new AddHealthLogInputDTO()
            {
                PreferenceId = 2,
                value = 30
            };

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await HealthLogService.AddHealthLog(addHealthLogInputDTO2, 1));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No Ideal Value data found!"));
        }

        [Test]
        public async Task AddHealthLogNoIdealValueFoundLowerLimitExceptionTest()
        {
            // Arrange
            AddHealthLogInputDTO addHealthLogInputDTO = new AddHealthLogInputDTO()
            {
                PreferenceId = 1,
                value = 6
            };

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No Ideal Value data found!"));
        }

        [Test]
        public async Task AddHealthLogAlreadyEnteredExceptionTest()
        {
            // Arrange
            AddHealthLogInputDTO addHealthLogInputDTO = new AddHealthLogInputDTO()
            {
                PreferenceId = 1,
                value = 8
            };
            await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1);

            // Action
            var exception = Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Health Log already entered!"));
        }

        [Test]
        public async Task AddHealthLogForWeightSuccessTest()
        {
            // Arrange
            AddHealthLogInputDTO addHealthLogInputDTO1 = new AddHealthLogInputDTO()
            {
                PreferenceId = 3,
                value = float.Parse("1.5")
            };
            await HealthLogService.AddHealthLog(addHealthLogInputDTO1, 1);
            AddHealthLogInputDTO addHealthLogInputDTO2 = new AddHealthLogInputDTO()
            {
                PreferenceId = 2,
                value = 50
            };

            // Action
            var result = await HealthLogService.AddHealthLog(addHealthLogInputDTO2, 1);

            // Assert
            Assert.That(result.HealthStatus, Is.EqualTo("Fair"));
        }

        [Test]
        public async Task AddHealthLogForWeightNoLogsFoundFailTest()
        {
            // Arrange
            AddHealthLogInputDTO addHealthLogInputDTO = new AddHealthLogInputDTO()
            {
                PreferenceId = 2,
                value = 50
            };

            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Height Log not entered"));
        }

        [Test]
        public async Task AddHealthLogForWeightNoHeightLogFoundFailTest()
        {
            // Arrange
            AddHealthLogInputDTO addHealthLogInputDTO = new AddHealthLogInputDTO()
            {
                PreferenceId = 1,
                value = 8
            };
            await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1);

            addHealthLogInputDTO = new AddHealthLogInputDTO()
            {
                PreferenceId = 2,
                value = 50
            };

            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Height Log not entered"));
        }

        [Test]
        public async Task AddHealthLogForHeightSuccessTest()
        {
            // Arrange
            AddHealthLogInputDTO addHealthLogInputDTO = new AddHealthLogInputDTO()
            {
                PreferenceId = 3,
                value = 150
            };
            var result = await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1);

            // Assert
            Assert.That(result.HealthStatus, Is.EqualTo("No_Status"));
        }

        [Test]
        public async Task GetHealthLogTargetReachedSuccessTest()
        {
            // Arrange
            Target target = new Target()
            {
                PreferenceId = 1,
                TargetMinValue = 7,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now.AddDays(3),
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
                
            };
            await TargetRepository.Add(target);
            AddHealthLogInputDTO addHealthLogInputDTO = new AddHealthLogInputDTO()
            {
                PreferenceId = 1,
                value = 8
            };
            await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1);
            var result = await HealthLogService.GetHealthLog(1, 1);

            // Assert
            Assert.That(result.HealthStatus, Is.EqualTo("Fair"));
        }

        [Test]
        public async Task GetHealthLogTargetNotReachedSuccessTest()
        {
            // Arrange
            Target target = new Target()
            {
                PreferenceId = 1,
                TargetMinValue = 9,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now.AddDays(3),
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now

            };
            await TargetRepository.Add(target);
            AddHealthLogInputDTO addHealthLogInputDTO = new AddHealthLogInputDTO()
            {
                PreferenceId = 1,
                value = 8
            };
            await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1);

            // Action
            var result = await HealthLogService.GetHealthLog(1, 1);

            // Assert
            Assert.That(result.HealthStatus, Is.EqualTo("Fair"));
        }

        [Test]
        public async Task GetHealthLogNoFilteredTargetSuccessTest()
        {
            // Arrange
            Target target = new Target()
            {
                PreferenceId = 1,
                TargetMinValue = 9,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now.AddDays(-3),
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now

            };
            await TargetRepository.Add(target);
            AddHealthLogInputDTO addHealthLogInputDTO = new AddHealthLogInputDTO()
            {
                PreferenceId = 1,
                value = 8
            };
            ;

            // Action
            var result = await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1);

            // Assert
            Assert.That(result.TargetStatus, Is.Null);
        }

        [Test]
        public async Task GetHealthLogNotFoundExceptionTest()
        {
            // Arrange
            AddHealthLogInputDTO addHealthLogInputDTO = new AddHealthLogInputDTO()
            {
                PreferenceId = 1,
                value = 8
            };
            await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await HealthLogService.GetHealthLog(3, 1));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No Health logs found!"));
        }

        [Test]
        public async Task UpdateHealthLogSuccessTest()
        {
            // Arrange
            AddHealthLogInputDTO addHealthLogInputDTO = new AddHealthLogInputDTO()
            {
                PreferenceId = 1,
                value = 8
            };
            await HealthLogService.AddHealthLog(addHealthLogInputDTO, 1);

            // Action
            var result = await HealthLogService.UpdateHealthLog(1, 9, 1);

            // Assert
            Assert.That(result.HealthStatus, Is.EqualTo("Fair"));
        }

        [Test]
        public async Task UpdateHealthLogFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await HealthLogService.UpdateHealthLog(100, 9, 1));

        }
    }
}
