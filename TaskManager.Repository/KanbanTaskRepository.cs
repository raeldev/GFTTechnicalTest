using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TaskManager.Domain.Model;
using TaskManager.Domain.Repository;
using TaskManager.Repository.Factory;

namespace TaskManager.Repository
{
    public class KanbanTaskRepository : IKanbanTaskRepository
    {
        private readonly ILogger<KanbanTaskRepository> _logger;
        public KanbanTaskRepository(ILogger<KanbanTaskRepository> logger)
        {
            _logger = logger;
        }

        public void Insert(KanbanTask kanbanTask)
        {
            try
            {
                var collection = MongoDBConnectionFactory<KanbanTask>.GetCollection();
                kanbanTask.TaskId = GetNextValidId(collection);
                collection.InsertOne(kanbanTask);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Erro ao inserir um nova tarefa. TaskId: {kanbanTask.TaskId}", ex);
                throw;
            }
        }

        public void Update(KanbanTask kanbanTask)
        {
            try
            {
                var collection = MongoDBConnectionFactory<KanbanTask>.GetCollection();
                
                var filter = Builders<KanbanTask>.Filter
                    .Eq(t => t.TaskId, kanbanTask.TaskId);

                var update = Builders<KanbanTask>.Update
                    .Set(t => t.Description, kanbanTask.Description)
                    .Set(t => t.ConclusionDate, kanbanTask.ConclusionDate);

                collection.UpdateOne(filter, update);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao inserir um nova tarefa. TaskId: {kanbanTask.TaskId}", ex);
                throw;
            }
        }

        public void Delete(int kanbanTaskId)
        {
            try
            {
                var collection = MongoDBConnectionFactory<KanbanTask>.GetCollection();
                var filter = Builders<KanbanTask>.Filter.Eq(t => t.TaskId, kanbanTaskId);
                collection.DeleteOne(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao excluir um tarefa. TaskId: {kanbanTaskId}", ex);
                throw;
            }
        }

        public async Task<KanbanTask> GetById(int kanbanTaskId)
        {
            try
            {
                var collection = MongoDBConnectionFactory<KanbanTask>.GetCollection();
                var filter = Builders<KanbanTask>.Filter.Eq(t => t.TaskId, kanbanTaskId);
                var result = await collection.FindAsync(filter);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar uma tarefa. OrderId: {kanbanTaskId}", ex);
                throw;
            }
        }

        private int GetNextValidId(IMongoCollection<KanbanTask> collection)
        {
            var sort = Builders<KanbanTask>.Sort.Descending(x => x.TaskId);
            var result = collection.Find(x => x.TaskId > 0).Sort(sort).Limit(1).FirstOrDefault();
            return (result?.TaskId ?? 0)+1;
        }
    }
}
