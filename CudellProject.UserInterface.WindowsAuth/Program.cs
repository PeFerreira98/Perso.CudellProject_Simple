using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CudellProject.UserInterface.WindowsAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            //.UseIISIntegration()
            //.UseHttpSys(options =>
            //{
            //    options.Authentication.Schemes = AuthenticationSchemes.NTLM | AuthenticationSchemes.Negotiate;
            //    options.Authentication.AllowAnonymous = false;
            //})
            .Build();
    }
}
