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
    public class SuggestionRepoTest
    {
        HealthTrackerContext context;
        IRepository<int, Suggestion> suggestionRepo;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("SuggestionDB");
            context = new HealthTrackerContext(optionsBuilder.Options);
            suggestionRepo = new SuggestionRepository(context);
            await suggestionRepo.Add(new Suggestion
            { Id = 1, CoachId = 1, UserId = 1, Description = "Walk more",Created_at = DateTime.Now, Updated_at = DateTime.Now });
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddSuggestionSuccessTest()
        {
            // Action
            var result = await suggestionRepo.Add(new Suggestion
            { Id = 2, CoachId = 1, UserId = 1, Description = "Walk more", Created_at = DateTime.Now, Updated_at = DateTime.Now });

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetBySuggestionIdSuccessTest()
        {
            // Action
            var result = suggestionRepo.GetById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetBySuggestionIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => suggestionRepo.GetById(100));
        }

        [Test]
        public async Task GetAllSuggestionsSuccessTest()
        {
            // Action
            var result = await suggestionRepo.GetAll();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteSuggestionByIdSuccessTest()
        {
            // Action
            var result = await suggestionRepo.Delete(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task DeleteSuggestionByIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => suggestionRepo.Delete(100));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Entity not found!"));
        }

        [Test]
        public async Task UpdateSuggestionFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => suggestionRepo.Update(new Suggestion
            { Id = 100, CoachId = 1, UserId = 1, Description = "Walk more", Created_at = DateTime.Now, Updated_at = DateTime.Now }));
        }


        [Test]
        public async Task UpdateSuggestionSuccessTest()
        {
            // Arrange
            var entity = await suggestionRepo.Add(new Suggestion
            { Id = 2, CoachId = 1, UserId = 1, Description = "Walk more", Created_at = DateTime.Now, Updated_at = DateTime.Now });
            entity.Description = "Eat";

            // Action
            var result = await suggestionRepo.Update(entity);

            // Assert
            Assert.That(result.Description, Is.EqualTo("Eat"));

        }
    }
}