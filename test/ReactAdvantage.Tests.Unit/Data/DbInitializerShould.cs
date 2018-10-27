using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ReactAdvantage.Api.Services;
using ReactAdvantage.Data;
using ReactAdvantage.Domain.Configuration;
using ReactAdvantage.Domain.Models;
using ReactAdvantage.Domain.Services;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Data
{
    public class DbInitializerShould
    {
        private readonly ReactAdvantageContext _db;
        private readonly Mock<IHostingEnvironment> _envMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<RoleManager<Role>> _roleManagerMock;
        private readonly DbInitializer _dbInitializer;

        public DbInitializerShould()
        {
            //Given

            var dbOptions = new DbContextOptionsBuilder<ReactAdvantageContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var loggerMock = new Mock<ILogger<ReactAdvantageContext>>();
            var tenantProviderMock = new Mock<ITenantProvider>();
            tenantProviderMock.Setup(x => x.GetTenantId()).Returns((int?)null);

            _db = new ReactAdvantageContext(dbOptions, loggerMock.Object, tenantProviderMock.Object);

            _envMock = new Mock<IHostingEnvironment>();
            _envMock.Setup(x => x.EnvironmentName).Returns("Test");

            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var roleStore = new Mock<IRoleStore<Role>>();
            _roleManagerMock = new Mock<RoleManager<Role>>(roleStore.Object, null, null, null, null);

            _dbInitializer = new DbInitializer(
                _db,
                _envMock.Object,
                _userManagerMock.Object,
                _roleManagerMock.Object
            );
        }

        [Fact]
        public void SeedUsersAndRoles()
        {
            //When

            _dbInitializer.Initialize();

            //Then

            _userManagerMock.Verify(x => x.CreateAsync(It.Is<User>(u => u.UserName == "admin" && u.TenantId == null), It.IsAny<string>()));
            _roleManagerMock.Verify(x => x.CreateAsync(It.Is<Role>(r => r.TenantId == null && r.Name == RoleNames.HostAdministrator)));
            _roleManagerMock.Verify(x => x.CreateAsync(It.Is<Role>(r => r.TenantId != null && r.Name == RoleNames.Administrator)));
            _roleManagerMock.Verify(x => x.CreateAsync(It.Is<Role>(r => r.TenantId != null && r.Name == RoleNames.User)));
            _userManagerMock.Verify(x => x.AddToRoleAsync(It.Is<User>(u => u.UserName == "admin" && u.TenantId == null), RoleNames.HostAdministrator));
        }

        [Fact]
        public void SeedProjectsAndTasks()
        {
            //When

            _dbInitializer.Initialize();

            //Then

            using (_db.DisableTenantFilter())
            {
                Assert.True(_db.Projects.AnyAsync().GetAwaiter().GetResult());
                Assert.True(_db.Tasks.AnyAsync().GetAwaiter().GetResult());
            }
        }

        [Fact]
        public void NotSeedExistingRoles()
        {
            //Given


            var tenant = new Tenant { Name = "Test Tenant" };
            _db.Tenants.Add(tenant);
            _db.SaveChanges();

            _db.Roles.Add(new Role { TenantId = null, Name = RoleNames.HostAdministrator });
            _db.Roles.Add(new Role { TenantId = tenant.Id, Name = RoleNames.Administrator });
            _db.Roles.Add(new Role { TenantId = tenant.Id, Name = RoleNames.User });
            _db.SaveChanges();

            //When

            _dbInitializer.Initialize();

            //Then

            _roleManagerMock.Verify(x => x.CreateAsync(It.Is<Role>(r => r.Name == RoleNames.HostAdministrator)), Times.Never);
            _roleManagerMock.Verify(x => x.CreateAsync(It.Is<Role>(r => r.Name == RoleNames.Administrator)), Times.Never);
            _roleManagerMock.Verify(x => x.CreateAsync(It.Is<Role>(r => r.Name == RoleNames.User)), Times.Never);
        }

        [Fact]
        public void NotSeedExistingUsers()
        {
            //Given

            var tenant = new Tenant { Name = "Test Tenant" };
            _db.Tenants.Add(tenant);
            _db.SaveChanges();

            _db.Users.Add(new User { UserName = "Test" });
            _db.Users.Add(new User { UserName = "Test2", TenantId = tenant.Id });
            _db.SaveChanges();

            //When

            _dbInitializer.Initialize();

            //Then

            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void NotSeedExistingTasks()
        {
            //Given
            using (_db.SetTenantFilterValue(1))
            {
                var task = new Task { Name = "Test Task", TenantId = 1 };
                _db.Tasks.Add(task);
                _db.SaveChanges();

                //When

                _dbInitializer.Initialize();

                //Then

                Assert.Collection(_db.Tasks,
                    dbTask => Assert.Same(task, dbTask)
                );
            }
        }

        [Fact]
        public void TryToMigrateDatabase()
        {
            //Given

            _envMock.Setup(x => x.EnvironmentName).Returns("NotTest");

            //Then

            //db.Database.Migrate() throws an exception if being run agains InMemory database
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                //When

                _dbInitializer.Initialize();
            });

            Assert.Equal("Relational-specific methods can only be used when the context is using a relational database provider.", ex.Message);
        }
    }
}
