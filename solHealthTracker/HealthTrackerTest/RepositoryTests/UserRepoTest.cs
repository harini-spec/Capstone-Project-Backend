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
    public class UserRepoTest
    {
        HealthTrackerContext context;
        IRepository<int, User> userRepo;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("UserDB");
            context = new HealthTrackerContext(optionsBuilder.Options);
            userRepo = new UserRepository(context);
            await userRepo.Add(new User
            { UserId = 1, Name = "Han", Age = 20, Email = "han@gmail.com", Gender = GenderEnum.Gender.Male, Phone = "9999999999", Role = RolesEnum.Roles.User, Created_at = DateTime.Now, Updated_at = DateTime.Now });
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddUserSuccessTest()
        {
            // Action
            var result = await userRepo.Add(new User
            { UserId = 2, Name = "Han", Age = 20, Email = "hana@gmail.com", Gender = GenderEnum.Gender.Male, Phone = "9999999999", Role = RolesEnum.Roles.User, Created_at = DateTime.Now, Updated_at = DateTime.Now });

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByUserIdSuccessTest()
        {
            // Action
            var result = userRepo.GetById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByUserIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => userRepo.GetById(100));
        }

        [Test]
        public async Task GetAllUsersSuccessTest()
        {
            // Action
            var result = await userRepo.GetAll();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllUsersFailTest()
        {
            // Arrange 
            await userRepo.Delete(1);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(() => userRepo.GetAll());
        }

        [Test]
        public async Task DeleteUserByIdSuccessTest()
        {
            // Action
            var result = await userRepo.Delete(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task DeleteUserByIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => userRepo.Delete(100));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Entity not found!"));
        }

        [Test]
        public async Task UpdateUserFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => userRepo.Update(new User
            { UserId = 100, Name = "Han", Age = 20, Email = "han@gmail.com", Gender = GenderEnum.Gender.Male, Phone = "9999999999", Role = RolesEnum.Roles.User, Created_at = DateTime.Now, Updated_at = DateTime.Now }));
        }


        [Test]
        public async Task UpdateUserSuccessTest()
        {
            // Arrange
            var entity = await userRepo.Add(new User
            { UserId = 2, Name = "Han", Age = 20, Email = "han@gmail.com", Gender = GenderEnum.Gender.Male, Phone = "9999999999", Role = RolesEnum.Roles.User, Created_at = DateTime.Now, Updated_at = DateTime.Now });
            entity.Name = "Hana";

            // Action
            var result = await userRepo.Update(entity);

            // Assert
            Assert.That(result.Name, Is.EqualTo("Hana"));

        }
    }
}