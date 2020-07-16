using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Godel.HelloWorld.IntegrationTests.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> GetResultData<T>(this HttpResponseMessage response)
        {
            string resultJson = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(resultJson, new JsonSerializerSettings());
            return result;
        }
    }
}
