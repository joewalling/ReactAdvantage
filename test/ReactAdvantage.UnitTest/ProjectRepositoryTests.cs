using Xunit;

namespace ReactAdvantage.UnitTest
{
    public class RepositoryTests
    {
        [Fact]
        public async void GetAllProjects_ReturnsCorrectList()
        {
            //var inMemoryRepository = new Data.InMemory.Repositories.ProjectRepository();
            //var expected = await inMemoryRepository.GetAllAsync();

            ////var dbRepository = new Data.EntityFramework.Repositories.ProjectRepository();
            ////var actual = await dbRepository.GetAllAsync();

            //actual.Count.Should().Be(expected.Count);
            //Assert.Equal(expected[0].Id, actual[0].Id);
            //Assert.Equal(expected[1].Id, actual[1].Id);
            //Assert.Equal(expected[2].Id, actual[2].Id);

        }

        [Fact]
        public async void GetAllProjects_IsNotEmptyOrNull()
        {
            //var dbRepository = new Data.EntityFramework.Repositories.ProjectRepository();
            //var actual = await dbRepository.GetAllAsync();

            //actual.Should().NotBeNull();
        }

    }
}
