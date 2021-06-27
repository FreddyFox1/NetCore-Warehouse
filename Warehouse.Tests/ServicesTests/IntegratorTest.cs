using Xunit;
using static Warehouse.Services.Integrator.IntegratorService;
using static Warehouse.Services.Integrator.Models.IntegratorService;

namespace Warehouse.Tests.ServicesTests
{
    public class IntegratorTest : IntegrationTest
    {
        [Fact]
        public void TestGenericFunction_input_json_return_object()
        {
            Deserializer<CategoriesModel> deserializer = new Deserializer<CategoriesModel>();
            var json = "{\"Categories\": [{\"IDCategory\" : 1,\"CategoryName\" : \"Тротуарная плитка\"},{\"IDCategory\" : 2,\"CategoryName\" : \"Брусчатка\"}]}";
            var objectsCategory = deserializer.Deserialize(json);
            var assert = objectsCategory.Categories.Count > 0;
            Assert.True(assert);
        }
    }
}
