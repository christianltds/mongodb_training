using System.Text.Json.Serialization;
using mongodb.apis;
using mongodb.documents;
using mongodb.repositories;
using mongodb.services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services
    .AddSingleton(sp =>
        {
            var connectionString = Environment.GetEnvironmentVariable("MONGODB_URI");
            return new MongoDbContext(connectionString, "bank");
        })
    .AddScoped<IAccountService, AccountService>()
    .AddScoped<IAccountRepository<Account>, AccountRepository>()
    .AddScoped<ITransactionRepository, TransactionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAccountsApis();

app.Run();