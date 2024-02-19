using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Recipe_Api.Data;
using DotNet.Testcontainers;
using Testcontainers.MsSql;
using NUnit.Framework;
using Xunit;

namespace Test
{
    public class RecipeApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _dbMsSqlContainer = new MsSqlBuilder().Build();
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AppDBContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<AppDBContext>(options =>
                {
                    options
                        .UseSqlServer(_dbMsSqlContainer.GetConnectionString());
                });
            });
        }

        public Task InitializeAsync()
        {
            return _dbMsSqlContainer.StartAsync();
        }

        public new Task DisposeAsync()
        {
            return _dbMsSqlContainer.StopAsync();
        }
    }
}
