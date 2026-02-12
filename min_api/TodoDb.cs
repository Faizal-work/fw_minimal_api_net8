using Microsoft.EntityFrameworkCore;
namespace min_api
{
    // DbContext is the database manager between C# to any database
    class TodoDb: DbContext
    {
        // Initialisation for DBContext options
        public TodoDb(DbContextOptions<TodoDb> options): base(options) {}

        // Setting the DB queries i.e. Todos.Where, Todos.FindAllSync etc
        public DbSet<Todo> Todos => Set<Todo>();
    }
}
