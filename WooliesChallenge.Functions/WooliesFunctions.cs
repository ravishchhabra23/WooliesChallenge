using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WooliesChallenge.Application.Interfaces;
using WooliesChallenge.Application.Helpers;

namespace WooliesChallenge.Functions
{
    public class WooliesFunctions
    {
        private readonly IUserService _userService;
        public WooliesFunctions(IUserService userService)
        {
            _userService = userService;            
        }

        [FunctionName("GetUser")]
        public async Task<IActionResult> GetUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Constants.RouteAnswers)] HttpRequest req,
            ILogger log)
        {
            var result = _userService.GetUser();
            return new OkObjectResult(result);
        }
    }
}
