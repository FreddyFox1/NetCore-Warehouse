using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Warehouse.Controllers;
using Warehouse.Model;
using Xunit;

namespace Warehouse.Tests.ControllersTests
{

    public class ItemsControllerTests : IntegrationTest
    {
        [Fact]
        public async Task Get_SecurePageRedirectsAnUnauthenticatedUser()
        {
            // Arrange
            var client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            // Act
            var response = await client.GetAsync("/Items");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login",
                response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Get_SecurePageIsReturnedForAnAuthenticatedUser()
        {
            //Act
            var response = await TestClient.GetAsync("/Items");
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void GetItems_return_json()
        {
            // Act
            var response = await TestClient.GetAsync("api/Items");

            // Assert
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
