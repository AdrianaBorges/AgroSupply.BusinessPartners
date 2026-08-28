using System.Text;
using AgroSupply.BusinessPartners.Api.Authentication;
using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessRelationships;
using AgroSupply.BusinessPartners.Infrastructure.Persistence;
using AgroSupply.BusinessPartners.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var jwtIssuer =
    builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "A configuração Jwt:Issuer não foi encontrada.");

var jwtAudience =
    builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "A configuração Jwt:Audience não foi encontrada.");

var jwtKey =
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "A configuração Jwt:Key não foi encontrada.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile =
        $"{typeof(Program).Assembly.GetName().Name}.xml";

    var xmlPath =
        Path.Combine(
            AppContext.BaseDirectory,
            xmlFile);

    options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "Informe o token JWT no formato: Bearer {token}"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
        });
});

builder.Services.AddDbContext<BusinessPartnersDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration
                .GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<
    IBusinessPartnerRepository,
    BusinessPartnerRepository>();

builder.Services.AddScoped<
    IBusinessRelationshipRepository,
    BusinessRelationshipRepository>();

builder.Services.AddScoped<
    IAuthenticationService,
    AuthenticationService>();

builder.Services.AddScoped<CreateBusinessPartnerUseCase>();
builder.Services.AddScoped<GetBusinessPartnerByIdUseCase>();
builder.Services.AddScoped<GetAllBusinessPartnersUseCase>();
builder.Services.AddScoped<UpdateBusinessPartnerUseCase>();
builder.Services.AddScoped<DeactivateBusinessPartnerUseCase>();
builder.Services.AddScoped<AddPhoneNumberToBusinessPartnerUseCase>();
builder.Services.AddScoped<GetPhoneNumberFromBusinessPartnerUseCase>();
builder.Services.AddScoped<UpdatePhoneNumberFromBusinessPartnerUseCase>();
builder.Services.AddScoped<RemovePhoneNumberFromBusinessPartnerUseCase>();
builder.Services.AddScoped<CreateBusinessRelationshipUseCase>();
builder.Services.AddScoped<DeactivateBusinessRelationshipUseCase>();
builder.Services.AddScoped<JwtTokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
};