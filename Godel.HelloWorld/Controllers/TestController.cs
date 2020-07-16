using Godel.HelloWorld.Models.TestController;
using Microsoft.AspNetCore.Mvc;

namespace Godel.HelloWorld.Controllers
{
    [Produces("application/json")]
    [Route("api/test")]
    public class TestController : Controller
    {
        [HttpGet]
        public ObjectResult Get()
        {
            return new ObjectResult(new Response
            {
                Action = "Test"
            });
        }
    }
}