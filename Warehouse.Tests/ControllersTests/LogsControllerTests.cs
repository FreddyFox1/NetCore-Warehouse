using Warehouse.Controllers;
using Warehouse.Model;
using Xunit;

namespace Warehouse.Tests.ControllersTests
{
    public class LogsControllerTests : IntegrationTest
    {
        [Fact]
        public async void GetLogs_return_json()
        {
            //Act
            var response = await TestClient.GetAsync("api/Logs");

            // Assert
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

    }
}
