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

            MetricService = new MetricService(UserPreference, null, MetricRepository, null);
            GraphService = new GraphService(HealthLogRepository, MetricService);

            Metric metric1 = new Metric()
            {
                Id = 1,
                MetricType = "Sleep_Hours",
                MetricUnit = "Hours",
                Created_at = DateTime.UtcNow,
                Updated_at = DateTime.UtcNow
            };
            await MetricRepository.Add(metric1);

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
    }
}
