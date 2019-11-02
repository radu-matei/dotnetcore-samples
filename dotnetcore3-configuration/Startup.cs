using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleConfiguration
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup()
        {
            var cb = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("greetings.json", optional: false, reloadOnChange: true);

            Configuration = cb.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            var rb = new RouteBuilder(app);
            rb.MapGet("{route}", ctx =>
            {
                var routeMessage = Configuration.AsEnumerable()
                    .FirstOrDefault(r => r.Key == ctx.GetRouteValue("route")
                    .ToString())
                    .Value;

                var defaultMessage = Configuration.AsEnumerable()
                    .FirstOrDefault(r => r.Key == "default")
                    .Value;

                return ctx.Response.WriteAsync(routeMessage ?? defaultMessage);
            });

            app.UseRouter(rb.Build());
        }
    }
}
