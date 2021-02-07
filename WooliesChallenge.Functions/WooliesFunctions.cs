using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WooliesChallenge.Application.Interfaces;
using WooliesChallenge.Application.Helpers;
using System;
using WooliesChallenge.Application.Models;
using System.Collections.Generic;

namespace WooliesChallenge.Functions
{
    public class WooliesFunctions// : IFunctionExceptionFilter
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
            User result = null;
            try
            {
                log.LogInformation("Get User started");
                result = _userService.GetUser();
            }
            catch(Exception ex)
            {
                log.LogError("The error occured : -" + ex.Message + " - " + ex.StackTrace);
            }
                
                return new OkObjectResult(result);
        }

        [FunctionName("GetSortedProducts")]
        public async Task<IActionResult> GetSortedProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Constants.RouteProducts)] HttpRequest req,
            ILogger log)
        {
            List<Product> result = null;
            try
            {
                log.LogInformation("GetSortedProduct started");
                SortOption sortValue = Helper.GetEnumValue(Convert.ToString(req.Query["sortOption"]));
                var respResource = sortValue == SortOption.Recommended ? await _resourceService.GetShopperHistory() : await _resourceService.GetProducts();
                result = Helper.SortProducts(respResource, sortValue);
            }
            catch (Exception ex)
            {
                log.LogError("The error occured : -" + ex.Message + " - " + ex.StackTrace);
            }

            return new OkObjectResult(result);
        }

        [FunctionName("GetTrolleyCalculation")]
        public async Task<IActionResult> GetTrolleyCalculation(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Constants.RouteTrolleyCalculator)] HttpRequest req,
            ILogger log)
        {
            decimal result= decimal.Zero;
            try
            {
                log.LogInformation("GetTrolleyCalculation started");
                if (req == null || req.Body == null) throw new ArgumentNullException("requset");

                var trolleyRequest = await Helper.ReadRequestBody(req.Body);
                var respResource = await _resourceService.GetTrolleyCalculation(trolleyRequest);
                result = Helper.DeSerializeInput<decimal>(respResource);
            }
            catch (Exception ex)
            {
                log.LogError("The error occured : -" + ex.Message + " - " + ex.StackTrace);
            }

            return new OkObjectResult(result);
        }
        //public Task OnExceptionAsync(FunctionExceptionContext exceptionContext, CancellationToken cancellationToken)
        //{
        //    log.WriteLine($"Exception raised by the application {exceptionContext.Exception.ToString()}");
        //    return Task.CompletedTask;
        //}
    }
}
