using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using WooliesChallenge.Application.Interfaces;
using WooliesChallenge.Application.Services;
using WooliesChallenge.Functions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace WooliesChallenge.Functions
{
    public class Startup : FunctionsStartup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public Startup()
        {

        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            //IHttpClientfactory to call the http service
            builder.Services
             .AddScoped<IUserService, UserService>()
             .AddHttpClient<IResourceService, ResourceService>(client =>
             {
                 client.BaseAddress = new Uri(config["ResourceBaseUrl"]);
                 client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 client.DefaultRequestHeaders.Add("User-Agent", "WooliesChallenge");
             })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to two minutes
            .AddPolicyHandler(GetJitterRetryPolicy());
        }
        //For high concurrency and scalable systems under high contention, it improves the performance by adding a jitter strategy (randomness) to exponential retry 
        //policy.
        static IAsyncPolicy<HttpResponseMessage> GetJitterRetryPolicy()
        {
            Random jitterer = new Random();
            var retryWithJitterPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3,    // exponential back-off plus some jitter
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                  + TimeSpan.FromMilliseconds(jitterer.Next(0, 100))
                );
            return retryWithJitterPolicy;
        }
    }
}
