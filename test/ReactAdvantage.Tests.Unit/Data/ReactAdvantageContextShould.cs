using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ReactAdvantage.Api.Services;
using ReactAdvantage.Data;
using ReactAdvantage.Domain.Services;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Data
{
    public class ReactAdvantageContextShould
    {
        private readonly Mock<ILogger<ReactAdvantageContext>> _dbLoggerMock;
        private readonly Mock<ITenantProvider> _tenantProviderMock;

        public ReactAdvantageContextShould()
        {
            //Given
            _dbLoggerMock = new Mock<ILogger<ReactAdvantageContext>>();
            _tenantProviderMock = new Mock<ITenantProvider>();
            _tenantProviderMock.Setup(x => x.GetTenantId()).Returns(5);
        }

        private ReactAdvantageContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ReactAdvantageContext>()
                .UseInMemoryDatabase(databaseName: "ReactAdvantage")
                .Options;
            return new ReactAdvantageContext(options, _dbLoggerMock.Object, _tenantProviderMock.Object);
        }

        [Fact]
        public void HaveAssignedLogger()
        {
            //When
            using (var db = GetDbContext())
            {
                //Then
                Assert.NotNull(db.Logger);
                Assert.Same(_dbLoggerMock.Object, db.Logger);
            }
        }

        [Fact]
        public void HaveEnabledTenantFilterByDefault()
        {
            //When
            using (var db = GetDbContext())
            {
                //Then
                Assert.True(db.IsTenantFilterEnabled);
            }
        }

        [Fact]
        public void DisableTenantFilter()
        {
            //When
            using (var db = GetDbContext())
            {
                db.DisableTenantFilter();

                //Then
                Assert.False(db.IsTenantFilterEnabled);
            }
        }

        [Fact]
        public void RestorePreviousTrueValueToTenantFilter()
        {
            //When
            using (var db = GetDbContext())
            {
                using (db.DisableTenantFilter())
                {
                }

                //Then
                Assert.True(db.IsTenantFilterEnabled);
            }
        }

        [Fact]
        public void RestorePreviousFalseValueToTenantFilter()
        {
            //When
            using (var db = GetDbContext())
            {
                using (db.DisableTenantFilter())
                {
                    using (db.DisableTenantFilter())
                    {
                    }

                    //Then
                    Assert.False(db.IsTenantFilterEnabled);
                }

            }
        }
        
        [Fact]
        public void HaveAssignedTenantFilterValueFromTenantProvider()
        {
            //When
            using (var db = GetDbContext())
            {
                //Then
                Assert.Equal(5, db.TenantFilterValue);
            }
        }

        [Fact]
        public void SetTenantFilterValueToNewValue()
        {
            //When
            using (var db = GetDbContext())
            {
                db.SetTenantFilterValue(8);

                //Then
                Assert.Equal(8, db.TenantFilterValue);
            }
        }

        [Fact]
        public void RestoreInitialValueToTenantFilter()
        {
            //When
            using (var db = GetDbContext())
            {
                using (db.SetTenantFilterValue(8))
                {
                }

                //Then
                Assert.Equal(5, db.TenantFilterValue);
            }
        }

        [Fact]
        public void RestoreLastValueToTenantFilter()
        {
            //When
            using (var db = GetDbContext())
            {
                using (db.SetTenantFilterValue(6))
                {
                    using (db.SetTenantFilterValue(7))
                    {
                    }

                    //Then
                    Assert.Equal(6, db.TenantFilterValue);
                }
            }
        }
    }
}
