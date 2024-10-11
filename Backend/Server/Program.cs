using Backend.DB;
using Backend.Server.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSignalR();
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("MyDB");
builder.Services.AddDbContext<UserContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseEndpoints(endPoints =>
{
    endPoints.MapControllers();
    app.MapHub<GameHub>("/GameHub");
    app.MapHub<PlayerHub>("/PlayerHub");
});

app.Run();