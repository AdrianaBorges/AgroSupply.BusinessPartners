using AgroSupply.BusinessPartners.Api.Tests.Authentication;
using AgroSupply.BusinessPartners.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AgroSupply.BusinessPartners.Api.Tests;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["Authentication:Username"] = "admin",
                    ["Authentication:Password"] = "AgroSupply@2026!",
                    ["Authentication:Role"] = "Administrator",
                    ["Jwt:Issuer"] = "AgroSupply.BusinessPartners.Api",
                    ["Jwt:Audience"] = "AgroSupply.BusinessPartners.Client",
                    ["Jwt:ExpirationMinutes"] = "60",
                    ["Jwt:Key"] = "AgroSupply.BusinessPartners-Integration-Test-Jwt-Key-2026"
                });
        });

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

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        TestAuthenticationHandler.AuthenticationScheme;

                    options.DefaultChallengeScheme =
                        TestAuthenticationHandler.AuthenticationScheme;
                })
                .AddScheme<
                    AuthenticationSchemeOptions,
                    TestAuthenticationHandler>(
                    TestAuthenticationHandler.AuthenticationScheme,
                    options => { });
        });
    }
}