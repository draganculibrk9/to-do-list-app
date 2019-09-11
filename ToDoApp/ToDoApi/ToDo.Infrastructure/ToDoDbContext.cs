using Core;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Infrastructure
{
    public class ToDoDbContext : DbContext
    {
        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<ToDoListShare> ToDoListShares { get; set; }

        public ToDoDbContext(DbContextOptions options) : base(options) { }

        protected ToDoDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ToDoListConfiguration());
            modelBuilder.ApplyConfiguration(new ToDoItemConfiguration());
            modelBuilder.ApplyConfiguration(new ToDoListShareConfiguration());
        }
    }
}
