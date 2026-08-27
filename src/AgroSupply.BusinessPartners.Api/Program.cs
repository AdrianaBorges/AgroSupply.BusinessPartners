using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;
using AgroSupply.BusinessPartners.Infrastructure.Persistence;
using AgroSupply.BusinessPartners.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<BusinessPartnersDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBusinessPartnerRepository, BusinessPartnerRepository>();

builder.Services.AddScoped<CreateBusinessPartnerUseCase>();
builder.Services.AddScoped<GetBusinessPartnerByIdUseCase>();
builder.Services.AddScoped<GetAllBusinessPartnersUseCase>();
builder.Services.AddScoped<UpdateBusinessPartnerUseCase>();
builder.Services.AddScoped<DeactivateBusinessPartnerUseCase>();
builder.Services.AddScoped<AddPhoneNumberToBusinessPartnerUseCase>();
builder.Services.AddScoped<GetPhoneNumberFromBusinessPartnerUseCase>();
builder.Services.AddScoped<UpdatePhoneNumberFromBusinessPartnerUseCase>();
builder.Services.AddScoped<RemovePhoneNumberFromBusinessPartnerUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
};