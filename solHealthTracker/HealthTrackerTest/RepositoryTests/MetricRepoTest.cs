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
    public class MetricRepoTest
    {
        HealthTrackerContext context;
        IRepository<int, Metric> metricRepository;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("MetricDataDB");
            context = new HealthTrackerContext(optionsBuilder.Options);
            metricRepository = new MetricRepository(context);
            await metricRepository.Add(new Metric
            { Id = 1, MetricType = "Weight", MetricUnit = "Kg", Created_at = DateTime.Now, Updated_at = DateTime.Now });
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddMetricDataSuccessTest()
        {
            // Action
            var result = await metricRepository.Add(new Metric
            { Id = 2, MetricType = "Weight", MetricUnit = "Kg", Created_at = DateTime.Now, Updated_at = DateTime.Now });

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByMetricIdSuccessTest()
        {
            // Action
            var result = metricRepository.GetById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByMetricIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => metricRepository.GetById(100));
        }

        [Test]
        public async Task GetAllMetricsSuccessTest()
        {
            // Action
            var result = await metricRepository.GetAll();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllMetricsFailTest()
        {
            // Arrange 
            await metricRepository.Delete(1);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(() => metricRepository.GetAll());
        }

        [Test]
        public async Task DeleteMetricByIdSuccessTest()
        {
            // Action
            var result = await metricRepository.Delete(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task DeleteMetricByIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => metricRepository.Delete(100));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Entity not found!"));
        }

        [Test]
        public async Task UpdateMetricFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => metricRepository.Update(new Metric
            { Id = 100, MetricType = "Weight", MetricUnit = "Kg", Created_at = DateTime.Now, Updated_at = DateTime.Now }));
        }


        [Test]
        public async Task UpdateMetricSuccessTest()
        {
            // Arrange
            var entity = await metricRepository.Add(new Metric
            { Id = 2, MetricType = "Weight", MetricUnit = "Kg", Created_at = DateTime.Now, Updated_at = DateTime.Now });
            entity.MetricUnit = "g";

            // Action
            var result = await metricRepository.Update(entity);

            // Assert
            Assert.That(result.MetricUnit, Is.EqualTo("g"));

        }
    }
}