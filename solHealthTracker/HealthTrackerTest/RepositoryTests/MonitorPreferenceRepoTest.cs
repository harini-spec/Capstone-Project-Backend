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
    public class MonitorPreferenceRepoTest
    {
        HealthTrackerContext context;
        IRepository<int, MonitorPreference> monitorPreferenceRepo;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("MonitorPreferenceDataDB");
            context = new HealthTrackerContext(optionsBuilder.Options);
            monitorPreferenceRepo = new MonitorPreferenceRepository(context);
            await monitorPreferenceRepo.Add(new MonitorPreference
            { CoachId = 1, MetricId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now });
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddMonitorPreferenceSuccessTest()
        {
            // Action
            var result = await monitorPreferenceRepo.Add(new MonitorPreference
            { CoachId = 2, MetricId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now });

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByMonitorPreferenceIdSuccessTest()
        {
            // Action
            var result = monitorPreferenceRepo.GetById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByMonitorPreferenceIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => monitorPreferenceRepo.GetById(100));
        }

        [Test]
        public async Task GetAllMonitorPreferencesSuccessTest()
        {
            // Action
            var result = await monitorPreferenceRepo.GetAll();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllMonitorPreferencesFailTest()
        {
            // Arrange 
            await monitorPreferenceRepo.Delete(1);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(() => monitorPreferenceRepo.GetAll());
        }

        [Test]
        public async Task DeleteMonitorPreferenceByIdSuccessTest()
        {
            // Action
            var result = await monitorPreferenceRepo.Delete(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task DeleteMonitorPreferenceByIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => monitorPreferenceRepo.Delete(100));
        }

        [Test]
        public async Task UpdateMonitorPreferenceFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => monitorPreferenceRepo.Update(new MonitorPreference
            { CoachId = 200, MetricId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now }));
        }


        [Test]
        public async Task UpdateMonitorPreferenceSuccessTest()
        {
            // Arrange
            var entity = await monitorPreferenceRepo.Add(new MonitorPreference
            { CoachId = 2, MetricId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now });
            entity.MetricId = 2;

            // Action
            var result = await monitorPreferenceRepo.Update(entity);

            // Assert
            Assert.That(result.MetricId, Is.EqualTo(2));

        }
    }
}