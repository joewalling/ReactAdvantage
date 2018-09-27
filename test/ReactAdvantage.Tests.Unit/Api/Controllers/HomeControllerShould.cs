using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReactAdvantage.Api.Controllers;
using Xunit;

namespace ReactAdvantage.Tests.Unit.Api.Controllers
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

        [Fact]
        public void ReturnNotNullGraphqlPlaygroundAuthCallbackViewResult()
        {
            // When
            var result = _homeController.GraphqlPlaygroundAuthCallback() as ViewResult;

            // Then
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ReturnNotNullErrorViewResult()
        {
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.TraceIdentifier).Returns("some request id");

            //Given
            _homeController.ControllerContext = new ControllerContext { HttpContext = httpContextMock.Object };

            // When
            var result = _homeController.Error() as ViewResult;

            // Then
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
