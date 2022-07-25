using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using OrdlyBackend.Controllers;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Services;
using OrdlyBackend.Utilities;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAzureKeyVault(AzureUtils.GetUri(), new DefaultAzureCredential());

var secret = AzureUtils.GetSecretFromVault("Ordly--DB");

builder.Services.AddDbContext<OrdlyContext>(options =>
options.UseSqlServer(secret));

//builder.Configuration.AddAzureAppConfiguration(options =>
//{
//    options.Connect(Environment.GetEnvironmentVariable("AppConfig"))
//    .ConfigureRefresh(refresh =>
//        {
//            refresh.Register("Game:Settings:Sentinel", refreshAll: true).SetCacheExpiration(new TimeSpan(0, 0, 30));
//        });
//});
//builder.Services.Configure<Settings>(builder.Configuration.GetSection("Game:Settings:WordCategory"));


Settings settings = new()
{
    WordCategory = "all"
};

builder.Services.AddHealthChecks();
builder.Services.AddCors();
builder.Services.AddSingleton<Settings>();
builder.Services.AddScoped<IDailyWordService, DailyWordService>();
builder.Services.AddScoped<IWordService, WordService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHostedService<WorkerService>();

// Add services to the container.
builder.Services.AddControllers();

//var connectionString = builder.Configuration.GetConnectionString("OrdlyContext") ?? "Data Source=Ordly.db";
//builder.Services.AddSqlite<OrdlyContext>(connectionString);

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

builder.Services.AddApplicationInsightsTelemetry((options) => options.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);
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
    app.UseSwaggerUI(c=> c.SwaggerEndpoint("./v1/swagger.json", "OrdlyAPI V1")); //originally "./swagger/v1/swagger.json");
}
//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c=> c.SwaggerEndpoint("./v1/swagger.json", "OrdlyAPI v1")); //originally "./swagger/v1/swagger.json");
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

//app.UseAzureAppConfiguration();

app.UseAuthorization();

app.MapControllers();

app.Run();
