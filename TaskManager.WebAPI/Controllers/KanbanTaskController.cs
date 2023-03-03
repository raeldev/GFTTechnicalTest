using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Model;
using TaskManager.Domain.Services;

namespace TaskManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KanbanTaskController : ControllerBase
    {
        private readonly ILogger<KanbanTaskController> _logger;
        private readonly IQueueService _queueService;
        private readonly IKanbanTaskService _kanbanTaskService;

        public KanbanTaskController(
            ILogger<KanbanTaskController> logger, 
            IQueueService queueService,
            IKanbanTaskService kanbanTaskService)
        {
            _logger = logger;
            _queueService = queueService;
            _kanbanTaskService = kanbanTaskService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] KanbanTask kanbanTask)
        {
            _queueService.InsertTask(kanbanTask);

            _logger.LogInformation($"Insert received.");

            return Ok();
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int kanbanTaskId)
        {
            var result = _kanbanTaskService.GetKanbanTaskAsync(kanbanTaskId);

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Update([FromBody] KanbanTask kanbanTask)
        {
            _queueService.UpdateTask(kanbanTask);

            _logger.LogInformation($"Update received. TaskId: {kanbanTask.TaskId}");

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int kanbanTaskId)
        {
            _queueService.DeleteTask(kanbanTaskId);

            _logger.LogInformation($"Delete received. TaskId: {kanbanTaskId}");

            return Ok();
        }
    }
}

