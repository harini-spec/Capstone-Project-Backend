﻿using HealthTracker.Exceptions;
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
    public class UserPreferenceRepoTest
    {
        HealthTrackerContext context;
        IRepository<int, UserPreference> userPreferenceRepo;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("UserPreferenceDataDB");
            context = new HealthTrackerContext(optionsBuilder.Options);
            userPreferenceRepo = new UserPreferenceRepository(context);
            await userPreferenceRepo.Add(new UserPreference
            { UserId = 1, MetricId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now });
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddUserPreferenceSuccessTest()
        {
            // Action
            var result = await userPreferenceRepo.Add(new UserPreference
            { UserId = 2, MetricId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now });

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByUserPreferenceIdSuccessTest()
        {
            // Action
            var result = userPreferenceRepo.GetById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByUserPreferenceIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => userPreferenceRepo.GetById(100));
        }

        [Test]
        public async Task GetAllUserPreferencesSuccessTest()
        {
            // Action
            var result = await userPreferenceRepo.GetAll();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllUserPreferencesFailTest()
        {
            // Arrange 
            await userPreferenceRepo.Delete(1);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(() => userPreferenceRepo.GetAll());
        }

        [Test]
        public async Task DeleteUserPreferenceByIdSuccessTest()
        {
            // Action
            var result = await userPreferenceRepo.Delete(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task DeleteUserPreferenceByIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => userPreferenceRepo.Delete(100));
        }

        [Test]
        public async Task UpdateUserPreferenceFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => userPreferenceRepo.Update(new UserPreference
            { UserId = 200, MetricId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now }));
        }


        [Test]
        public async Task UpdateUserPreferenceSuccessTest()
        {
            // Arrange
            var entity = await userPreferenceRepo.Add(new UserPreference
            { UserId = 2, MetricId = 1, Created_at = DateTime.Now, Updated_at = DateTime.Now });
            entity.MetricId = 2;

            // Action
            var result = await userPreferenceRepo.Update(entity);

            // Assert
            Assert.That(result.MetricId, Is.EqualTo(2));

        }
    }
}