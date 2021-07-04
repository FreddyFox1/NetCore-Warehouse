using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace Warehouse.Tests.ControllersTests
{
    public class RouteTests
    : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public RouteTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        //Pages
        [InlineData("/")]
        [InlineData("/items")]
        [InlineData("/items/Create")]
        [InlineData("/items/Move")]
        [InlineData("/items/Eventlog")]
        [InlineData("/items/Groups")]

        //API
        [InlineData("/api/archive")]
        [InlineData("/api/items")]
        [InlineData("/api/MoveItems")]
        
        //Manage Account
        [InlineData("/Profile")]
        [InlineData("/ChangePassword")]
        
        [InlineData("/Users")]
        [InlineData("/Users/Roles")]
        [InlineData("/Users/Edit")]
        [InlineData("/Users/Roles/Create")]
        
        [InlineData("/Telegram")]
        [InlineData("/Telegram/Edit")]
        [InlineData("/Telegram/Delete")]
        
        [InlineData("/Bitrix")]
        [InlineData("/Bitrix/Edit")]
        [InlineData("/Bitrix/Delete")]
        
        [InlineData("/Integrator")]

        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
