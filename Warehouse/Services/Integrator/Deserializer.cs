using Newtonsoft.Json;

namespace Warehouse.Services.Integrator
{
    public partial class IntegratorService
    {
        public static class Deserializer<T>
        {
            public static T Deserialize(string json)
            {
                var data = JsonConvert.DeserializeObject<T>(json);
                return data;
            }
        }
    }
}
