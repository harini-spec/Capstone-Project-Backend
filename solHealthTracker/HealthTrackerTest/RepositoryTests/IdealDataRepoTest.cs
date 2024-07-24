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
    public class IdealDataRepoTest
    {
        HealthTrackerContext context;
        IRepository<int, IdealData> idealDataRepository;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("IdealDataDB");
            context = new HealthTrackerContext(optionsBuilder.Options);
            idealDataRepository = new IdealDataRepository(context);
            await idealDataRepository.Add(new IdealData
            { ID = 1, MetricId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, MinVal = 10, MaxVal = 12, HealthStatus = HealthStatusEnum.HealthStatus.Good });
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddIdealDataSuccessTest()
        {
            // Action
            var result = await idealDataRepository.Add(new IdealData
            { ID = 2, MetricId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, MinVal = 10, MaxVal = 12, HealthStatus = HealthStatusEnum.HealthStatus.Good });

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByIdealDataIdSuccessTest()
        {
            // Action
            var result = idealDataRepository.GetById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByIdealDataIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => idealDataRepository.GetById(100));
        }

        [Test]
        public async Task GetAllIdealDataSuccessTest()
        {
            // Action
            var result = await idealDataRepository.GetAll();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteIdealDataByIdSuccessTest()
        {
            // Action
            var result = await idealDataRepository.Delete(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task DeleteIdealDataByIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => idealDataRepository.Delete(100));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Entity not found!"));
        }

        [Test]
        public async Task UpdateIdealDataFailTest()
        {
            // Arrange
            await idealDataRepository.Add(new IdealData
            { ID = 2, MetricId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, MinVal = 10, MaxVal = 12, HealthStatus = HealthStatusEnum.HealthStatus.Good });

            // Action
            var exception = Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => idealDataRepository.Update(new IdealData
            { ID = 100, MetricId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, MinVal = 10, MaxVal = 12, HealthStatus = HealthStatusEnum.HealthStatus.Good }));
        }


        [Test]
        public async Task UpdateIdealDataSuccessTest()
        {
            // Arrange
            var idealData = await idealDataRepository.Add(new IdealData
            { ID = 2, MetricId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, MinVal = 10, MaxVal = 12, HealthStatus = HealthStatusEnum.HealthStatus.Good });
            idealData.MaxVal = 20;

            // Action
            var result = await idealDataRepository.Update(idealData);

            // Assert
            Assert.That(result.MaxVal, Is.EqualTo(20));

        }
    }
}