using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace AvManager.SignalR
{
    public class SignalRStartup
    {
        public SignalRStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var setting = SettingService.GetSetting().Result;

            services.AddSignalR();
            services.AddCors(option => option.AddPolicy("cors",
                policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
                    .WithOrigins("http://localhost:20003", setting.MvcSite)));

            services.AddSingleton(new LogHelper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("cors");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<JobHubs>("/jobs");
            });
        }
    }
}
