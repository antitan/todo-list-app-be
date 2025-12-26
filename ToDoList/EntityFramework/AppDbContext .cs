using Microsoft.EntityFrameworkCore;
using ToDoList.EntityFramework.Entities;

namespace ToDoList.EntityFramework
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

         public DbSet<TodoItem> Todos => Set<TodoItem>();
    }
}
