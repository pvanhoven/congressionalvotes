using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace congress
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CongressDataContext>(options =>
            {
                foreach (DictionaryEntry e in System.Environment.GetEnvironmentVariables())
                {
                    Console.WriteLine(e.Key + ":C:" + e.Value);
                    Debug.WriteLine(e.Key + ":D:" + e.Value);
                    Trace.TraceError(e.Key + ":T:" + e.Value);
                }

                // need to print environment variables, figure out if this is a casing issue or doesn't exist
                string connectionString = Configuration.GetValue<string>("CongressionalVotesDbConnectionString");
                options.UseSqlServer(connectionString);
            });

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // make api PascalCase, ugh
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}