using johns_lyng_app.Server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace johns_lyng_app.Server.Interface
{
    public interface ITodoItemsService
    {
        Task<List<TodoItem>> GetTodoItems();
        Task<TodoItem> GetTodoItem(Guid id);
        Task<OperationResult> PutTodoItem(TodoItem todoItem);
        Task<TodoItem> PostTodoItem(TodoItem todoItem);
        bool TodoItemIdExists(Guid id);
        bool TodoItemDescriptionExists(string description);
    }
}
