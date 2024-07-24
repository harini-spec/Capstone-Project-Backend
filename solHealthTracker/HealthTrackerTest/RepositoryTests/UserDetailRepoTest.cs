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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HealthTrackerTest.RepositoryTests
{
    public class UserDetailRepoTest
    {
        HealthTrackerContext context;
        IRepository<int, UserDetail> userDetailRepo;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("UserDetailDB");
            context = new HealthTrackerContext(optionsBuilder.Options);
            userDetailRepo = new UserDetailRepository(context);
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[32];
                rng.GetBytes(randomBytes);
                await userDetailRepo.Add(new UserDetail
                { Id = 1, PasswordEncrypted = System.Text.Encoding.UTF8.GetBytes("password123"), PasswordHashKey = randomBytes, Status = UserStatusEnum.UserStatus.Active, Created_at = DateTime.Now, Updated_at = DateTime.Now });
            }
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Test]
        public async Task AddUserDetailSuccessTest()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                // Arrange
                byte[] randomBytes = new byte[32];
                rng.GetBytes(randomBytes);

                // Action
                var result = await userDetailRepo.Add(new UserDetail
                { Id = 2, PasswordEncrypted = System.Text.Encoding.UTF8.GetBytes("password123"), PasswordHashKey = randomBytes, Status = UserStatusEnum.UserStatus.Active, Created_at = DateTime.Now, Updated_at = DateTime.Now });

                // Assert
                Assert.That(result, Is.Not.Null);
            }
        }

        [Test]
        public async Task GetByUserDetailIdSuccessTest()
        {
            // Action
            var result = userDetailRepo.GetById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByUserDetailIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => userDetailRepo.GetById(100));
        }

        [Test]
        public async Task GetAllUserDetailsSuccessTest()
        {
            // Action
            var result = await userDetailRepo.GetAll();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllUserDetailsFailTest()
        {
            // Arrange 
            await userDetailRepo.Delete(1);

            // Action
            var exception = Assert.ThrowsAsync<NoItemsFoundException>(() => userDetailRepo.GetAll());
        }

        [Test]
        public async Task DeleteUserDetailByIdSuccessTest()
        {
            // Action
            var result = await userDetailRepo.Delete(1);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task DeleteUserDetailByIdFailTest()
        {
            // Action
            var exception = Assert.ThrowsAsync<EntityNotFoundException>(() => userDetailRepo.Delete(100));
        }

        [Test]
        public async Task UpdateUserDetailFailTest()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                // Arrange
                byte[] randomBytes = new byte[32];
                rng.GetBytes(randomBytes);

                // Action
                var exception = Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => userDetailRepo.Update(new UserDetail
                { Id = 2, PasswordEncrypted = System.Text.Encoding.UTF8.GetBytes("password123"), PasswordHashKey = randomBytes, Status = UserStatusEnum.UserStatus.Active, Created_at = DateTime.Now, Updated_at = DateTime.Now }));
            }
        }


        [Test]
        public async Task UpdateUserDetailSuccessTest()
        {
            // Arrange
            using (var rng = new RNGCryptoServiceProvider())
            {
                // Arrange
                byte[] randomBytes = new byte[32];
                rng.GetBytes(randomBytes);
                var entity = await userDetailRepo.Add(new UserDetail
                { Id = 2, PasswordEncrypted = System.Text.Encoding.UTF8.GetBytes("password123"), PasswordHashKey = randomBytes, Status = UserStatusEnum.UserStatus.Active, Created_at = DateTime.Now, Updated_at = DateTime.Now });
                entity.Status = UserStatusEnum.UserStatus.Inactive;

                // Action
                var result = await userDetailRepo.Update(entity);

                // Assert
                Assert.That(result.Status, Is.EqualTo(UserStatusEnum.UserStatus.Inactive));
            }
        }
    }
}