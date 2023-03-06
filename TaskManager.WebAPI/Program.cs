using TaskManager.Domain.Repository;
using TaskManager.Domain.Services;
using TaskManager.Repository;
using TaskManager.Repository.Factory;
using TaskManager.Service;

var builder = WebApplication.CreateBuilder(args);

// Healthcheck
builder.Services.AddHealthChecks().AddRabbitMQ(RabbitConnectionFactory.GetRabbitMqConnection);

// Add services to the container.
builder.Services.AddScoped<IKanbanTaskRepository, KanbanTaskRepository>();
builder.Services.AddScoped<IKanbanTaskService, KanbanTaskService>();
builder.Services.AddScoped<IQueueService, QueueService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapHealthChecks("/healthcheck");

app.MapControllers();

app.Run();
