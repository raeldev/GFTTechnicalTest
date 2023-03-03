using TaskManager.Domain.Model;

namespace TaskManager.Domain.Services
{
    public interface IKanbanTaskService
    {
        public Task<KanbanTask> GetKanbanTaskAsync(int id);
        public Task<IEnumerable<KanbanTask>> GetAllKanbanTaskAsync();
    }
}
