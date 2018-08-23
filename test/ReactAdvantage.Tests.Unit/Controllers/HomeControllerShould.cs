using Microsoft.AspNetCore.Mvc;
using ReactAdvantage.Api.Controllers;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Controllers
{
    public class HomeControllerShould
    {
        private readonly HomeController _homeController;

        public HomeControllerShould()
        {
            // Given
            _homeController = new HomeController();
        }

        [Fact]
        public void ReturnRedirectResultFromDefaultRouteResult()
        {
            // When
            var result = _homeController.Index() as RedirectToActionResult;

            // Then
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void ReturnNotNullGraphqlPlaygroundViewResult()
        {
            // When
            var result = _homeController.GraphqlPlayground() as ViewResult;

            // Then
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
