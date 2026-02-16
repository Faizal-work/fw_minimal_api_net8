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
    // Updated to use DTO
    return TypedResults.Ok(await db.Todos
                                   .Select(x => new TodoItemDTO(x))
                                   .ToArrayAsync());
}

// Get completed
static async Task<IResult> GetCompleteTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos
                                   .Where(t => t.IsComplete==true)
                                   .Select(x => new TodoItemDTO(x))
                                   .ToArrayAsync());
}

// Get based on ID
static async Task<IResult> GetTodo(int id, TodoDb db)
{
    return await db.Todos.FindAsync(id)
        is Todo todo 
        ? TypedResults.Ok(new TodoItemDTO(todo)) 
        : TypedResults.NotFound();
}

// Adding into todo
static async Task<IResult> CreateTodo( 
    TodoDb db, 
    TodoItemDTO todoItemDTO)
{
    if (string.IsNullOrWhiteSpace(todoItemDTO.Name))
    {
        return Results.NoContent();
    }

    var todo = new Todo
    {
        Name = todoItemDTO.Name,
        IsComplete = todoItemDTO.IsComplete
    };

    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    todoItemDTO = new TodoItemDTO(todo);
    return TypedResults.Created($"/todoitems/{todoItemDTO.Id}"
                                , todoItemDTO);
}

// Update
// Replaced todoInput with todoItemDTO to ensure secret is never called
static async Task<IResult> UpdateTodo(
    int id, 
    TodoItemDTO todoItemDTO,
    TodoDb db)
{
    if (string.IsNullOrWhiteSpace(todoItemDTO.Name))
    {
        return Results.NoContent();
    }

    var todoNew = await db.Todos.FindAsync(id);
    if (todoNew == null) {
        return TypedResults.NotFound();
    }

    todoNew?.Name = todoItemDTO.Name;
    todoNew?.IsComplete = todoItemDTO.IsComplete;

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
