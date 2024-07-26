using HealthTracker.Exceptions;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;
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
    public class TargetServiceTest
    {
        IRepository<int, Target> TargetRepository;
        IRepository<int, UserPreference> UserPreferenceRepository;
        IRepository<int, Metric> MetricRepository;

        IMetricService MetricService;
        ITargetService TargetService;

        HealthTrackerContext context;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("TargetServiceDB");
            context = new HealthTrackerContext(optionsBuilder.Options);

            TargetRepository = new TargetRepository(context);
            UserPreferenceRepository = new UserPreferenceRepository(context);
            MetricRepository = new MetricRepository(context);

            MetricService = new MetricService(UserPreferenceRepository, null, MetricRepository, null);
            TargetService = new TargetService(TargetRepository, MetricService);

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
            await UserPreferenceRepository.Add(userPreference1);
            UserPreference userPreference2 = new UserPreference()
            {
                Id = 2,
                MetricId = 1,
                UserId = 2,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };
            await UserPreferenceRepository.Add(userPreference2);
        }

        [TearDown]
        public void Teardown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddTargetSuccessTest()
        {
            // Arrange 
            TargetInputDTO targetInputDTO = new TargetInputDTO()
            {
                PreferenceId = 1,
                TargetMinValue = 8,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now.AddDays(3)
            };

            // Action
            var result = await TargetService.AddTarget(targetInputDTO);

            // Assert
            Assert.That(result, Is.EqualTo("Successfully added!"));
        }

        [Test]
        public async Task AddTargetAlreadyExistsExceptionTest()
        {
            // Arrange 
            TargetInputDTO targetInputDTO = new TargetInputDTO()
            {
                PreferenceId = 1,
                TargetMinValue = 8,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now.AddDays(3)
            };
            await TargetService.AddTarget(targetInputDTO);

            // Action
            var exception = Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await TargetService.AddTarget(targetInputDTO));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Target already exists for this date!"));
        }

        [Test]
        public async Task AddTargetFailTest()
        {
            // Arrange 
            TargetInputDTO targetInputDTO = new TargetInputDTO()
            {
                PreferenceId = 1,
                TargetMinValue = 8,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now.AddDays(-1)
            };

            // Action
            var exception = Assert.ThrowsAsync<InvalidActionException>(async () => await TargetService.AddTarget(targetInputDTO));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Can't create targets in the past"));
        }

        [Test]
        public async Task AddTargetNoUserPreferenceFoundFailTest()
        {
            // Arrange 
            TargetInputDTO target1 = new TargetInputDTO()
            {
                PreferenceId = 100,
                TargetMinValue = 8,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now
            };
            await TargetService.AddTarget(target1);

            TargetInputDTO target2 = new TargetInputDTO()
            {
                PreferenceId = 1,
                TargetMinValue = 8,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now
            };
            await UserPreferenceRepository.Delete(2);

            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await TargetService.AddTarget(target2));
        }

        [Test]
        public async Task GetTargetByIDFailTest()
        { 
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(async () => await TargetService.GetTargetById(100));
        }

        [Test]
        public async Task GetTargetByIDSuccessTest()
        {
            // Arrange 
            TargetInputDTO target1 = new TargetInputDTO()
            {
                PreferenceId = 1,
                TargetMinValue = 8,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now
            };
            await TargetService.AddTarget(target1);

            // Action
            var result = await TargetService.GetTargetById(1);

            // Assert
            Assert.That(result.TargetMinValue, Is.EqualTo(8));
        }

        [Test]
        public async Task UpdateTargetRepoFailTest()
        {
            // Arrange
            Target target = new Target()
            {
                PreferenceId = 100,
                TargetMinValue = 8,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now.AddDays(1),
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };

            // Action
            var exception = Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await TargetService.UpdateTargetRepo(target));
        }

        [Test]
        public async Task UpdateTargetSuccessTest()
        {
            // Arrange 
            TargetInputDTO target1 = new TargetInputDTO()
            {
                PreferenceId = 1,
                TargetMinValue = 8,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now
            };
            await TargetService.AddTarget(target1);

            UpdateTargetInputDTO updateTargetInputDTO = new UpdateTargetInputDTO()
            {
                TargetId = 1,
                PreferenceId = 1,
                TargetMinValue = 7,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now.AddDays(2)
            };

            // Action
            var result = await TargetService.UpdateTarget(updateTargetInputDTO);

            // Assert
            Assert.That(result, Is.EqualTo("Successfully updated!"));
        }

        [Test]
        public async Task UpdateTargetFailTest()
        {
            // Arrange
            TargetInputDTO target1 = new TargetInputDTO()
            {
                PreferenceId = 1,
                TargetMinValue = 8,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now
            };
            await TargetService.AddTarget(target1);
            TargetInputDTO target2 = new TargetInputDTO()
            {
                PreferenceId = 1,
                TargetMinValue = 8,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now.AddDays(2)
            };
            await TargetService.AddTarget(target2);

            UpdateTargetInputDTO updateTargetInputDTO = new UpdateTargetInputDTO()
            {
                TargetId = 1,
                PreferenceId = 1,
                TargetMinValue = 7,
                TargetMaxValue = 10,
                TargetDate = DateTime.Now.AddDays(2)
            };

            // Action
            var exception = Assert.ThrowsAsync<EntityAlreadyExistsException>(async () => await TargetService.UpdateTarget(updateTargetInputDTO));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Target already exists for this date!"));
        }
    }
}
