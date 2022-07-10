using ApplicationCore.Services;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddHostedService<WorkerService>();
// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("OrdlyContext") ?? "Data Source=Ordly.db";
builder.Services.AddSqlite<OrdlyContext>(connectionString);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    OrdlyContextSeed.Initialize(services);
}
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
