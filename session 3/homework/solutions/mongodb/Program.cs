using mongodb.apis;
using mongodb.documents;
using mongodb.repositories;
using mongodb.services;
using System.Text.Json.Serialization;

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

          if(connectionString == null)
          {
            throw new ArgumentException($"MONGODB_URI environment variable is missing and is required.");
          }

          return new MongoDbContext(connectionString, "bank", true);
        })
    .AddScoped<IAccountService, AccountService>()
    .AddScoped<IAccountRepository<Account>, AccountRepository>()
    .AddScoped<ITransactionRepository, TransactionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAccountsApis();

app.Run();
