using MongoDB.Driver;
using TaskManager.Domain.Model;

namespace TaskManager.Domain.Repository
{
    public interface IKanbanTaskRepository
    {
        void Insert(KanbanTask kanbanTask);
        void Update(KanbanTask kanbanTask);
        void Delete(int kanbanTaskId);
        Task<KanbanTask> GetById(int kanbanTaskId);
    }
}
