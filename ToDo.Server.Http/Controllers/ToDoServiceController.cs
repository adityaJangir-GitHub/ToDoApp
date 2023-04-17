using Microsoft.AspNetCore.Mvc;
using ToDo.Common.DTOs;
using ToDo.DB;

namespace ToDo.Server.Http.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoServiceController : ControllerBase
    {
        private IToDoRepository _toDoRepository;
        private readonly ILogger<ToDoServiceController> _logger;

        public ToDoServiceController(ILogger<ToDoServiceController> logger, IToDoRepository toDoRepository)
        {
            _logger = logger;
            _toDoRepository = toDoRepository;
        }

        [HttpPost]
        public void Post([FromBody] ToDoItem toDoItem)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"{BadRequest(ModelState)}");
            }
            _toDoRepository.Add(toDoItem);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var toDoItemList = _toDoRepository.Get();

            return Ok(toDoItemList);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Guid guid = Guid.Parse(id);
            var toDoItem = _toDoRepository.GetById(guid);
            return Ok(toDoItem);
        }

        [HttpPut("{id}")]
        public void Put(string id, [FromBody] string UpdatedToDo)
        {
            var guid = Guid.Parse(id);
            if (!ModelState.IsValid)
            {
                _logger.LogError($"{BadRequest(ModelState)}");
            }
            _toDoRepository.Update(guid, UpdatedToDo);
        }

        [HttpPatch("{id}")]
        public void Patch(string id)
        {
            Guid guid = Guid.Parse(id);
            _toDoRepository.MarkAsComplete(guid);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            Guid guid = Guid.Parse(id);
            _toDoRepository.Delete(guid);
        }

    }
}