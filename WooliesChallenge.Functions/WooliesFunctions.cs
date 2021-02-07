using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WooliesChallenge.Application.Interfaces;
using WooliesChallenge.Application.Helpers;
using System;
using System.Reflection;

namespace WooliesChallenge.Functions
{
    public class WooliesFunctions
    {
        private readonly IUserService _userService;
        private readonly IResourceService _resourceService;

        public WooliesFunctions(IUserService userService,IResourceService resourceService)
        {
            _userService = userService;
            _resourceService = resourceService;
        }

        [FunctionName("GetUser")]
        public async Task<IActionResult> GetUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Constants.RouteAnswers)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation(Constants.OperationStarted + MethodInfo.GetCurrentMethod().Name);
            var result = await _userService.GetUser();
            log.LogInformation(Constants.OperationCompleted + MethodInfo.GetCurrentMethod().Name);
            return new OkObjectResult(result);
        }

        [FunctionName("GetSortedProducts")]
        public async Task<IActionResult> GetSortedProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Constants.RouteProducts)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation(Constants.OperationStarted + MethodInfo.GetCurrentMethod().Name);
            SortOption sortValue = Helper.GetEnumValue(Convert.ToString(req.Query["sortOption"]));
            var respResource = sortValue == SortOption.Recommended ? await _resourceService.GetShopperHistory() : await _resourceService.GetProducts();
            var result = Helper.SortProducts(respResource, sortValue);
            log.LogInformation(Constants.OperationCompleted + MethodInfo.GetCurrentMethod().Name);
            return new OkObjectResult(result);
        }

        [FunctionName("GetTrolleyCalculation")]
        public async Task<IActionResult> GetTrolleyCalculation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Constants.RouteTrolleyCalculator)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation(Constants.OperationStarted + MethodInfo.GetCurrentMethod().Name);
            var trolleyRequest = await Helper.ReadRequestBody(req.Body);
            var respResource = await _resourceService.GetTrolleyCalculation(trolleyRequest);
            var result = Helper.DeSerializeInput<decimal>(respResource);
            log.LogInformation(Constants.OperationCompleted + MethodInfo.GetCurrentMethod().Name);
            return new OkObjectResult(result);
        }
    }
}
