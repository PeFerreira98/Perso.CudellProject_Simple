using CudellProject.Data.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CudellProject.UserInterface.WindowsAuth
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
            //services.Configure<IISOptions>(options => options.AutomaticAuthentication = true);

            //services.AddAuthentication(IISDefaults.AuthenticationScheme);
            //services.AddAuthentication(HttpSysDefaults.AuthenticationScheme);

            var connection = Configuration.GetConnectionString("DemoDbConnection");
            services.AddDbContext<DemoDbContext>(options => options.UseSqlServer(connection, b => b.UseRowNumberForPaging()));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Routing/Error");
            }

            app.UseStatusCodePagesWithReExecute("/Routing/Error");
            app.UseStaticFiles();

            loggerFactory.AddFile("../Logs/CudellProjectLogs-{Date}.txt");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Routing}/{action=Welcome}/{id?}");
            });
        }
    }
}
