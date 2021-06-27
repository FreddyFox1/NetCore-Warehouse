using Xunit;
using Warehouse.Services.Integrator;
using System.IO;
using System;

namespace Warehouse.Tests.ServicesTests
{
    public class IntegratorTest : IntegrationTest
    {
        [Fact]
        public void CategoryTable_input_json_return_objects()
        {
            var json = "{\"Categories\": [{\"IDCategory\" : 1,\"CategoryName\" : \"Тротуарная плитка\"},{\"IDCategory\" : 2,\"CategoryName\" : \"Брусчатка\"}]}";
            IntegratorService IntegratorService = new IntegratorService();
            var objectsCategory = IntegratorService.CategoryTable(json);
            var assert = objectsCategory.Count > 0;
            Assert.True(assert);
        }
    }
}
