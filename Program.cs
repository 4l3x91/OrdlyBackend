using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using OrdlyBackend.HealthChecks;
using OrdlyBackend.HealthChecks.DTOs;
using OrdlyBackend.Infrastructure;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Services;
using OrdlyBackend.Utilities;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//Azure configurations
//builder.Configuration.AddAzureKeyVault(AzureUtils.GetUri(), new DefaultAzureCredential());
//builder.Services.AddApplicationInsightsTelemetry((options) => options.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

//var secret = AzureUtils.GetSecretFromVault("Ordly--DB");
//builder.Services.AddDbContext<OrdlyContext>(options =>
//options.UseSqlServer(secret));

var connectionString = builder.Configuration.GetConnectionString("OrdlyContext") ?? "Data Source=Ordly.db";
builder.Services.AddSqlite<OrdlyContext>(connectionString);

Settings settings = new()
{
    WordCategory = "all",
    ForceNewDaily = false
};

//Add HealthChecks
builder.Services.AddHealthChecks()
   .AddCheck(
            "OrderingDB-check",
            new SqlHealthCheck("Data Source=Ordly.db"),//AzureUtils.GetSecretFromVault("Ordly--DB") ??
            HealthStatus.Unhealthy,
            new string[] { "orderingdb" });

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddSingleton<Settings>(settings);
builder.Services.AddScoped<IDailyWordService, DailyWordService>();
builder.Services.AddScoped<IWordService, WordService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserGameService, UserGameService>();
builder.Services.AddScoped<IRankService, RankService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddHostedService<WorkerService>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "OrdlyBackend",
        Version = "v1"
    });
    config.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new HealthCheckReponse
        {
            Status = report.Status.ToString(),
            HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse
            {
                Component = x.Key,
                Status = x.Value.Status.ToString(),
                Description = x.Value.Description
            }),
            HealthCheckDuration = report.TotalDuration
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
});

// Uncomment to seed database with Words

using (var scope = app.Services.CreateScope())
{
   var services = scope.ServiceProvider;
   OrdlyContextSeed.Initialize(services);
}


//Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("./v1/swagger.json", "OrdlyAPI v1")); //originally "./swagger/v1/swagger.json");
//}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
