using johns_lyng_app.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace johns_lyng_app.Server.Context
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
