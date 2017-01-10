using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace FluentMvcTesting.Extensions.Mocks
{
    public class ControllerPropertiesMock
    {
        private const string OriginHeader = "Origin";
        private readonly Mock<ControllerContext> _controllerContext;

        public ControllerPropertiesMock(string defaultUrl = "http://www.example.com")
        {
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.SetupGet(r => r.Url).Returns(new Uri(defaultUrl));
            mockRequest.Setup(r => r.Headers).Returns(new NameValueCollection {{OriginHeader, defaultUrl}});

            var mockResponse = Mock.Of<HttpResponseBase>();

            _controllerContext = new Mock<ControllerContext>();
            _controllerContext.Setup(x => x.HttpContext.Request).Returns(mockRequest.Object);
            _controllerContext.Setup(x => x.HttpContext.Response).Returns(mockResponse);
            _controllerContext
                .Setup(x => x.HttpContext.Response.ApplyAppPathModifier(It.IsAny<string>()))
                .Returns((string s) => s);
        }

        /// <summary>
        /// The controller context
        /// </summary>
        public ControllerContext ControllerContext => _controllerContext.Object;

        /// <summary>
        /// The URL helper
        /// </summary>
        /// <example>
        /// <code>
        /// Url = mockControllerProperties.Url(RouteConfig.RegisterRoutes)
        /// </code>
        /// </example>
        /// <param name="registerRoutes">Action for registering routes</param>
        /// <returns>The URL helper</returns>
        public UrlHelper Url(Action<RouteCollection> registerRoutes)
        {
            var routes = new RouteCollection();
            registerRoutes(routes);

            return new UrlHelper(
                new RequestContext(_controllerContext.Object.HttpContext, new RouteData()),
                routes);
        }
    }
}
