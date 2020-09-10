using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Authorization.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var assemblyName = typeof(Startup).GetTypeInfo().Assembly.FullName;
            BuildWebHost(args, assemblyName).Run();
            //BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args, string assemblyName) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup(assemblyName)
                //.UseUrls("http://*:5000")
                   .Build();
    }
}