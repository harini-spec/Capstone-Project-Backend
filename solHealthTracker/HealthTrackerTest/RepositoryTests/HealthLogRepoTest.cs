using HealthTracker.Exceptions;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;
using HealthTracker.Models.ENUMs;
using HealthTracker.Repositories.Classes;
using HealthTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthTrackerTest.RepositoryTests
{
    public class HealthLogRepoTest
    {
        HealthTrackerContext context;
        IRepository<int, HealthLog> healthLogRepository;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("HealthLogDB");
            context = new HealthTrackerContext(optionsBuilder.Options);
            healthLogRepository = new HealthLogRepository(context);
            await healthLogRepository.Add(new HealthLog
            { Id = 1, PreferenceId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, value = 10, HealthStatus = HealthStatusEnum.HealthStatus.Good });


        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddHealthLogSuccessTest()
        {
            // Action
            var result = await healthLogRepository.Add(new HealthLog
            { Id = 2, PreferenceId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, value = 10, HealthStatus = HealthStatusEnum.HealthStatus.Good });

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByHealthLogIdSuccessTest()
        {
            // Action
            var result = healthLogRepository.GetById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByHealthLogIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => healthLogRepository.GetById(100));
        }

        [Test]
        public async Task GetAllHealthLogsSuccessTest()
        {
            // Action
            var result = await healthLogRepository.GetAll();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllHealthLogsFailTest()
        {
            // Arrange 
            await healthLogRepository.Delete(1);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(() => healthLogRepository.GetAll());
        }

        [Test]
        public async Task DeleteHealthLogByIdSuccessTest()
        {
            // Action
            var result = await healthLogRepository.Delete(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task DeleteHealthLogByIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => healthLogRepository.Delete(100));
        }

        [Test]
        public async Task UpdateHealthLogFailTest()
        {
            // Arrange
            await healthLogRepository.Add(new HealthLog
            { Id = 2, PreferenceId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, value = 10, HealthStatus = HealthStatusEnum.HealthStatus.Good });

            // Action
            var exception = Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => healthLogRepository.Update(new HealthLog
            { Id = 3, PreferenceId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, value = 10, HealthStatus = HealthStatusEnum.HealthStatus.Good }));

        }


        [Test]
        public async Task UpdateHealthLogSuccessTest()
        {
            // Arrange
            var healthLog = await healthLogRepository.Add(new HealthLog
            { Id = 2, PreferenceId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, value = 10, HealthStatus = HealthStatusEnum.HealthStatus.Good });
            healthLog.value = 10;

            // Action
            var result = await healthLogRepository.Update(healthLog);

            // Assert
            Assert.That(result.value, Is.EqualTo(10));

        }
    }
}