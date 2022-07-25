using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Services;

var builder = WebApplication.CreateBuilder(args);
var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

var client = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());
var secret = client.GetSecret("Ordly--DB").Value.Value;

builder.Services.AddDbContext<OrdlyContext>(options =>
options.UseSqlServer(secret));

builder.Services.AddHealthChecks();
builder.Services.AddCors();
builder.Services.AddScoped<IDailyWordService, DailyWordService>();
builder.Services.AddScoped<IWordService, WordService>();
builder.Services.AddHostedService<WorkerService>();

// Add services to the container.
builder.Services.AddControllers();

//var connectionString = builder.Configuration.GetConnectionString("OrdlyContext") ?? "Data Source=Ordly.db";
//builder.Services.AddSqlite<OrdlyContext>(connectionString);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

var app = builder.Build();

app.MapHealthChecks("/health");

//Uncomment for seed
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    OrdlyContextSeed.Initialize(services);
//}

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c=> c.SwaggerEndpoint("./v1/swagger.json", "OrdlyAPI v1")); //originally "./swagger/v1/swagger.json");
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
