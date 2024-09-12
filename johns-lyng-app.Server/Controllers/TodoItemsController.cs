using johns_lyng_app.Server.Interface;
using johns_lyng_app.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace johns_lyng_app.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ILogger<TodoItemsController> _logger;
        private readonly ITodoItemsService _todoItemsService;

        public TodoItemsController(ILogger<TodoItemsController> logger, ITodoItemsService todoItemsService)
        {
            _logger = logger;
            _todoItemsService = todoItemsService;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var results = await _todoItemsService.GetTodoItems();
            return Ok(results);
        }


        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            var result = await _todoItemsService.GetTodoItem(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/TodoItems/... 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {

            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            var result = await _todoItemsService.PutTodoItem(todoItem);

            if (!result.Success)
            {
                if (result.ErrorMessage == "Not Found")
                {
                    return NotFound();
                }

                return StatusCode(500, result.ErrorMessage);
            }

            return NoContent();
        }

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostTodoItem(TodoItem todoItem)
        {
            if (string.IsNullOrEmpty(todoItem?.Description))
            {
                return BadRequest("Description is required");
            }
            else if (_todoItemsService.TodoItemDescriptionExists(todoItem.Description))
            {
                return BadRequest("Description already exists");
            }

            var result = await _todoItemsService.PostTodoItem(todoItem);

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

    }
}
