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

        public async Task<IEnumerable<KanbanTask>> GetAllKanbanTaskAsync()
        {
            return await _kanbanTaskRepository.GetAll();
        }

        public async Task<KanbanTask> GetKanbanTaskAsync(int id)
        {
            return await _kanbanTaskRepository.GetById(id);
        }
    }
}
