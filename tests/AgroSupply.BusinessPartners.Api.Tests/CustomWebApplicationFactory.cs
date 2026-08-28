using AgroSupply.BusinessPartners.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AgroSupply.BusinessPartners.Api.Tests;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor =
                services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<BusinessPartnersDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<BusinessPartnersDbContext>(
                options =>
                    options.UseInMemoryDatabase(
                        "AgroSupply.BusinessPartners.Tests"));
        });
    }
}