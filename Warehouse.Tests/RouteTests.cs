﻿using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace Warehouse.Tests
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
        [InlineData("/")]
        [InlineData("/items")]
        [InlineData("/items/Create")]
        [InlineData("/items/Move")]
        [InlineData("/items/Eventlog")]
        [InlineData("/admin/telegram")]
        [InlineData("/admin/roles")]
        [InlineData("/api/archive")]
        [InlineData("/api/items")]
        [InlineData("/api/MoveItems")]

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
