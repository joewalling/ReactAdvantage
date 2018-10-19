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
        [Fact]
        public void HaveAssignedLogger()
        {
            //Given
            var dbLogger = new Mock<ILogger<ReactAdvantageContext>>();
            var options = new DbContextOptionsBuilder<ReactAdvantageContext>()
                .UseInMemoryDatabase(databaseName: "ReactAdvantage")
                .Options;
            var tenantProviderMock = new Mock<ITenantProvider>();
            tenantProviderMock.Setup(x => x.GetTenantId()).Returns(1);

            //When
            using (var db = new ReactAdvantageContext(options, dbLogger.Object, tenantProviderMock.Object))
            {
                //Then
                Assert.NotNull(db.Logger);
                Assert.Same(dbLogger.Object, db.Logger);
            }
        }
    }
}
