using HealthTracker.Exceptions;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.DTOs.HealthLog;
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
    public class GraphServiceTest
    {
        IRepository<int, HealthLog> HealthLogRepository;
        IRepository<int, UserPreference> UserPreference;
        IRepository<int, Metric> MetricRepository;
        IRepository<int, IdealData> IdealDataRepository;

        IMetricService MetricService;
        IGraphService GraphService;

        HealthTrackerContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("GraphDB");
            context = new HealthTrackerContext(optionsBuilder.Options);

            HealthLogRepository = new HealthLogRepository(context);
            UserPreference = new UserPreferenceRepository(context);
            MetricRepository = new MetricRepository(context);
            IdealDataRepository = new IdealDataRepository(context);

            MetricService = new MetricService(UserPreference, null, MetricRepository, null);
            GraphService = new GraphService(HealthLogRepository, MetricService, IdealDataRepository);

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

            IdealData idealData1 = new IdealData()
            {
                MetricId = 1,
                HealthStatus = HealthStatusEnum.HealthStatus.Excellent,
                MinVal = 7,
                MaxVal = 10,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await IdealDataRepository.Add(idealData1);
            IdealData idealData2 = new IdealData()
            {
                MetricId = 1,
                HealthStatus = HealthStatusEnum.HealthStatus.Fair,
                MinVal = 8,
                MaxVal = 9,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await IdealDataRepository.Add(idealData2);

            UserPreference userPreference1 = new UserPreference()
            {
                Id = 1,
                MetricId = 1,
                UserId = 1,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await UserPreference.Add(userPreference1);

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
            HealthLog healthLog2 = new HealthLog()
            {
                Id = 2,
                HealthStatus = HealthStatusEnum.HealthStatus.Fair,
                PreferenceId = 1,
                value = 8,
                Created_at = DateTime.Now.AddDays(-7),
                Updated_at = DateTime.Now.AddMonths(-7)
            };
            await HealthLogRepository.Add(healthLog2);
            HealthLog healthLog3 = new HealthLog()
            {
                Id = 3,
                HealthStatus = HealthStatusEnum.HealthStatus.Fair,
                PreferenceId = 1,
                value = 8,
                Created_at = DateTime.Now.AddMonths(-1),
                Updated_at = DateTime.Now.AddMonths(-1)
            };
            await HealthLogRepository.Add(healthLog3);
        }

        [TearDown]
        public void Teardown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }


        [Test]
        public async Task GetGraphDataThisWeekSuccessTest()
        {
            // Action
            var result = await GraphService.GetGraphData("Sleep_Hours", "This Week", 1);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetGraphDataLastWeekSuccessTest()
        {
            // Action
            var result = await GraphService.GetGraphData("Sleep_Hours", "Last Week", 1);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetGraphDataThisMonthSuccessTest()
        {
            // Action
            var result = await GraphService.GetGraphData("Sleep_Hours", "This Month", 1);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetGraphDataLastMonthSuccessTest()
        {
            // Action
            var result = await GraphService.GetGraphData("Sleep_Hours", "Last Month", 1);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetOverallGraphDataSuccessTest()
        {
            // Action
            var result = await GraphService.GetGraphData("Sleep_Hours", "Overall", 1);

            // Assert
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task GetOverallGraphDataExceptionTest()
        {
            // Arrange
            await HealthLogRepository.Delete(1);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await GraphService.GetGraphData("Sleep_Hours", "This Week", 1));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("No Log Records found!"));
        }

        [Test]
        public async Task GetGraphDataRangeSuccessTest()
        {
            // Action
            var result = await GraphService.GetGraphDataHealthyRange("Sleep_Hours", 1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MinValue, Is.EqualTo(7));
                Assert.That(result.MaxValue, Is.EqualTo(10));
            });
        }

        [Test]
        public async Task GetGraphDataRangeNoMetricsFoundExceptionTest()
        {

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(async () => await GraphService.GetGraphDataHealthyRange("Weight", 1));
        }
    }
}
