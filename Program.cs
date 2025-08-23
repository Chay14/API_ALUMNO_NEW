using ApiMaestrosAlumnos.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Conexión MySQL
builder.Services.AddDbContext<EscuelaContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("EscuelaConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("EscuelaConnection"))
    )
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("PermitirTodo");
app.UseAuthorization();
app.MapControllers();
app.Run();
