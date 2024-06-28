using Microsoft.AspNetCore.Http.HttpResults;
using RazorComponentResultRnD;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorComponents();
var app = builder.Build();


var db = new List<TodoItem>()
{
    new TodoItem() { Id = 1, Name = "abc", IsCompleted = true },
    new TodoItem() { Id = 2, Name = "123", IsCompleted = false },
};
app.MapGet("/", () => new RazorComponentResult<DocumentComponent>());
app.MapGet("/todos", () => new RazorComponentResult<TodoItemListComponent>(new { Model = db }));
app.MapPost("/todos/{id}/toggle", (string id) =>
{
    var todo = db.First(t => t.Id.ToString() == id);
    todo.IsCompleted = !todo.IsCompleted;
    return new RazorComponentResult<ToDoItemComponent>(new { Model = todo });
});
app.MapDelete("/todos/{id}", (string id) =>
{
    var todo = db.First(t => t.Id.ToString() == id);
    db.Remove(todo);
});
app.MapPost("/todos", (AddToDoCommand command) =>
{
    var todo = new TodoItem { Id = db.Max(x =>x.Id) + 1, Name = command.Name, IsCompleted = false };
    db.Add(todo);
    return new RazorComponentResult<ToDoItemComponent>(new { Model = todo });
});
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record AddToDoCommand
{
    public string Name { get; set; }
}