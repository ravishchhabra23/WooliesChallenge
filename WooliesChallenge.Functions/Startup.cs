using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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

            builder.Services
                .AddScoped<IUserService, UserService>();
        }
    }
}
