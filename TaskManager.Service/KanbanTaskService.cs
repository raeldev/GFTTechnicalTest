using TaskManager.Domain.Model;
using TaskManager.Domain.Repository;
using TaskManager.Domain.Services;

namespace TaskManager.Service
{
    public class KanbanTaskService : IKanbanTaskService
    {
        private readonly IKanbanTaskRepository _kanbanTaskRepository;

        public KanbanTaskService(IKanbanTaskRepository kanbanTaskRepository) 
        {
            _kanbanTaskRepository = kanbanTaskRepository;
        }

        public Task<IEnumerable<KanbanTask>> GetAllKanbanTaskAsync()
        {
            throw new NotImplementedException();
        }

        public Task<KanbanTask> GetKanbanTaskAsync(int id)
        {
            return _kanbanTaskRepository.GetById(id);
        }
    }
}
