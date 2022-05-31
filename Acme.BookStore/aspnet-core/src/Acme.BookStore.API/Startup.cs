using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Acme.BookStore.API
{
    public class Startup
    {
        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddApplication<MaterialApiModule>(options =>
                {
                    // Integrate Autofac!
                    options.UseAutofac();
                });
            }

            public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
            {
                app.InitializeApplication();
            }
        }
    }
}
