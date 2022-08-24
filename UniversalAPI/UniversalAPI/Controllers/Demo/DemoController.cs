using Microsoft.AspNetCore.Mvc;
using UniversalAPI.Controllers.Core;
using UniversalAPI.Interfaces;

namespace UniversalAPI.Controllers.Demo
{
    /// <summary>
    /// this is a demo controller
    /// </summary>
    [ApiController]
    [Tags("This is my demo")]
    [ApiExplorerSettings(GroupName = "v1", IgnoreApi = false)]
    public class DemoController : MyControllerBase
    {
        public DemoController(ILogger myLogger, IControllerService service) : base(myLogger, service) { }

        /// <summary>
        /// Get list of all trains
        /// </summary>
        [HttpGet]
        [Route("api/trains")]
        [Produces(typeof(List<string>))]
        public async Task<IActionResult> GetTrains()
        {
            return await GetActionResultAndLoadDataAsync(async () => {
                return await GetTrainsAsyncFromDb();
            });
        }

        private async Task<List<string>> GetTrainsAsyncFromDb()
        {
            return new List<string> { "blue train", "yellow train", "red train" };
        }
    }
}
