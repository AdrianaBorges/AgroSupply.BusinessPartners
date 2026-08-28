using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace AgroSupply.BusinessPartners.Api.Tests;

public class UnauthenticatedWebApplicationFactory
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
                    ["Jwt:Issuer"] = "AgroSupply.BusinessPartners.Api",
                    ["Jwt:Audience"] = "AgroSupply.BusinessPartners.Client",
                    ["Jwt:ExpirationMinutes"] = "60",
                    ["Jwt:Key"] = "AgroSupply.BusinessPartners-Integration-Test-Jwt-Key-2026"
                });
        });
    }
}