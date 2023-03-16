using TaskManager.Domain.Model;

namespace TaskManager.Domain.Services
{
    public interface IQueueService
    {
        public Task StartProcess(CancellationToken stoppingToken);
        public void InsertTask(KanbanTask kanbanTask);
        public void UpdateTask(int kanbanTaskId, KanbanTask kanbanTask);
        public void DeleteTask(int kanbanTaskId);
    }
}
