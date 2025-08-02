var builder = WebApplication.CreateBuilder(args);

// AGREGA CONTROLADORES
builder.Services.AddControllers();

// HABILITA SWAGGER PARA GENERAR DOCUMENTACIÓN AUTOMÁTICA
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ACTIVA SWAGGER EN TIEMPO DE DESARROLLO
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("PermitirTodo");
app.UseAuthorization();
app.MapControllers();
app.Run();
