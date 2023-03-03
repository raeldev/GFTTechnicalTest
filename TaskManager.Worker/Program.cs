using System.ComponentModel;
using TaskManager.Domain.Repository;
using TaskManager.Domain.Services;
using TaskManager.Repository;
using TaskManager.Service;
using TaskManager.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IKanbanTaskRepository, KanbanTaskRepository>();
        services.AddSingleton<IQueueService, QueueService>();
    })
    .Build();

await host.RunAsync();
