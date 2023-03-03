using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using TaskManager.Domain.Enum;

namespace TaskManager.Domain.Model
{
    public sealed class KanbanTask
    {
        [JsonIgnore]
        [BsonId]
        public ObjectId Id { get; set; }
        public int TaskId { get; set; }
        public string? Description { get; set; }
        public KanbanTaskStatus Status { get; set; }
        public DateTime ConclusionDate { get; set; }
    }
}
