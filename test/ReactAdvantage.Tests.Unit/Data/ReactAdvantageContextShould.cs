using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ReactAdvantage.Data;
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

            //When
            using (var db = new ReactAdvantageContext(options, dbLogger.Object))
            {
                //Then
                Assert.NotNull(db.Logger);
                Assert.Equal(dbLogger.Object, db.Logger);
            }
        }
    }
}
