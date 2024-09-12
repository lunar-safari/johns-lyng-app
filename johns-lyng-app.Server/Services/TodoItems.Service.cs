using johns_lyng_app.Server.Context;
using johns_lyng_app.Server.Interface;
using johns_lyng_app.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace johns_lyng_app.Server.Services
{

    public class TodoItemsService : ITodoItemsService
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoItemsService> _logger;
        private readonly IConfiguration _config;

        public TodoItemsService(TodoContext context, ILogger<TodoItemsService> logger, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _config = config;
        }


        public async Task<List<TodoItem>> GetTodoItems()
        {
            var results = await _context.TodoItems.ToListAsync();
            return results;
        }


        public async Task<TodoItem> GetTodoItem(Guid id)
        {
            var result = await _context.TodoItems.FindAsync(id);
            return result;
        }


        public async Task<OperationResult> PutTodoItem(TodoItem todoItem)
        {
            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemIdExists(todoItem.Id))
                {
                    return new OperationResult { Success = false, ErrorMessage = "Not Found" };
                }
                else
                {
                    throw;
                }
            }

            return new OperationResult { Success = true };
        }


        public async Task<TodoItem> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public bool TodoItemIdExists(Guid id)
        {
            return _context.TodoItems.Any(x => x.Id == id);
        }

        public bool TodoItemDescriptionExists(string description)
        {
            return _context.TodoItems
                   .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }
               
    }
}
