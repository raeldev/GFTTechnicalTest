using TaskManager.Domain.Enum;

namespace TaskManager.Domain.Model
{
    public  class KanbanTaskEvent
    {
        public EventType EventType { get; set; }
        public KanbanTask KanbanTask { get; set; }
    }
}
