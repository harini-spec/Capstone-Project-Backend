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

namespace ATMTest.Repositories
{
    public class TargetRepoTest
    {
        HealthTrackerContext context;
        IRepository<int, Target> targetRepository;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("TargetDB");
            context = new HealthTrackerContext(optionsBuilder.Options);
            targetRepository = new TargetRepository(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddTargetSuccessTest()
        {
            // Action
            var result = await targetRepository.Add(new Target
            { Id = 1, PreferenceId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(2), TargetStatus = TargetStatusEnum.TargetStatus.Not_Achieved, TargetMinValue = 10, TargetMaxValue = 12 });

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByTargetIdSuccessTest()
        {
            // Arrange
            await targetRepository.Add(new Target
            { Id = 1, PreferenceId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(2), TargetStatus = TargetStatusEnum.TargetStatus.Not_Achieved, TargetMinValue = 10, TargetMaxValue = 12 });

            // Action
            var result = targetRepository.GetById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByTargetIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => targetRepository.GetById(100));
        }

        [Test]
        public async Task GetAllTargetsSuccessTest()
        {
            // Arrange
            await targetRepository.Add(new Target
            { Id = 1, PreferenceId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(2), TargetStatus = TargetStatusEnum.TargetStatus.Not_Achieved, TargetMinValue = 10, TargetMaxValue = 12 });

            // Action
            var result = await targetRepository.GetAll();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteTargetByIdSuccessTest()
        {
            // Arrange
            await targetRepository.Add(new Target
            { Id = 1, PreferenceId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(2), TargetStatus = TargetStatusEnum.TargetStatus.Not_Achieved, TargetMinValue = 10, TargetMaxValue = 12 });

            // Action
            var result = await targetRepository.Delete(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task DeleteTargetByIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => targetRepository.Delete(100));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Entity not found!"));
        }

        [Test]
        public async Task UpdateTargetFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => targetRepository.Update(new Target
            { Id = 3, PreferenceId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(2), TargetStatus = TargetStatusEnum.TargetStatus.Not_Achieved, TargetMinValue = 10, TargetMaxValue = 12 }));

        }


        [Test]
        public async Task UpdateTargetSuccessTest()
        {
            // Arrange
            var target = await targetRepository.Add(new Target
            { Id = 1, PreferenceId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(2), TargetStatus = TargetStatusEnum.TargetStatus.Not_Achieved, TargetMinValue = 10, TargetMaxValue = 12 });
            target.TargetMaxValue = 15;

            // Action
            var result = await targetRepository.Update(target);

            // Assert
            Assert.That(result.TargetMaxValue, Is.EqualTo(15));

        }
    }
}