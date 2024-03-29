﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ThAmCo.Catering.Data;

namespace ThAmCo.Catering
{
    /// <summary>
    /// The class used to drive the program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Initialises the database and other project values
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var env = services.GetRequiredService<IHostingEnvironment>();
                if (env.IsDevelopment())
                {
                    var context = services.GetRequiredService<CateringDbContext>();
                    context.Database.EnsureDeleted();
                    context.Database.Migrate();
                }
            }

            host.Run();
        }

        /// <summary>
        /// Creates a web host builder for the project
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>A new Web Host Builder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
