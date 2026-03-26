using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoListApi.Application.Services;
using TodoListApi.Application.Validators;
using TodoListApi.Domain.Interfaces;
using TodoListApi.Infrastructure.Persistence;
using TodoListApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ── Base de datos ──────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Inyección de dependencias ──────────────────────────────
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<TodoService>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTodoItemValidator>();

// ── CORS (para que Angular pueda consumir la API) ──────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

app.Run();
