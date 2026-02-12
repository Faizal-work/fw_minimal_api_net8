using Microsoft.EntityFrameworkCore;
using min_api;

// Setting the build
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Grouping the app and using TypedResult/
var todoItems = app.MapGroup("/todoitems");

// Methods for the todo list
todoItems.MapGet("/", GetAllTodos);
todoItems.MapGet("/complete", GetCompleteTodos);
todoItems.MapGet("/{id}", GetTodo);
todoItems.MapPost("/", CreateTodo);
todoItems.MapPut("/{id}", UpdateTodo);
todoItems.MapDelete("/{id}", DeleteTodo);

app.Run();

// Get all list
static async Task<IResult> GetAllTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.ToArrayAsync());
}

// Get completed
static async Task<IResult> GetCompleteTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete==true).ToArrayAsync());
}

// Get based on ID
static async Task<IResult> GetTodo(int id, TodoDb db)
{
    return await db.Todos.FindAsync(id)
        is Todo todo ? TypedResults.Ok() : TypedResults.NotFound();
}

// Adding into todo
static async Task<IResult> CreateTodo( Todo todo, TodoDb db)
{
    if (string.IsNullOrWhiteSpace(todo.Name))
    {
        return Results.NoContent();
    }

    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/todoitems/{todo.Id}", todo);
}

// Update
static async Task<IResult> UpdateTodo(int id, Todo todoInput, TodoDb db)
{
    if (string.IsNullOrWhiteSpace(todoInput.Name))
    {
        return Results.NoContent();
    }

    var todoNew = await db.Todos.FindAsync(id);
    if (todoNew != null) {
        return TypedResults.NotFound();
    }

    todoNew?.Name = todoInput.Name;
    todoNew?.IsComplete = todoInput.IsComplete;

    await db.SaveChangesAsync();
    return TypedResults.Ok();
}

// Delete task
static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}
