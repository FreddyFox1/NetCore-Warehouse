using Warehouse.Controllers;
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
        [Fact]
        public async void GetLogs_return_records()
        {
            LogsController logsController = new LogsController(GetContext());
            var actual = await logsController.GetAll();
            Assert.Contains("TestName", actual.ToString());
        }
    }
}
