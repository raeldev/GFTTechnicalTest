using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace TaskManager.Domain.Model
{
    public sealed class KanbanTask
    {
        [JsonIgnore]
        [BsonId]
        public ObjectId Id { get; set; }
        public int TaskId { get; set; }
        public string? Description { get; set; }
        public DateTime ConclusionDate { get; set; }
    }
}
