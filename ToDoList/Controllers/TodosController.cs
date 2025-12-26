using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Dtos;
using ToDoList.EntityFramework;
using ToDoList.EntityFramework.Entities;

[ApiController]
[Route("todos")]
public sealed class TodosController : ControllerBase
{
    private readonly AppDbContext _db;

    public TodosController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("lists")]
    public async Task<ActionResult> GetTodos(
      [FromQuery] int page = 1,
      [FromQuery] int pageSize = 20,
      CancellationToken cancellationToken = default)
    { 
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 5 : pageSize;

        var query = _db.Todos.AsNoTracking();

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TodoDto
            {
                Id = t.Id,
                Title = t.Title,
                Completed = t.Completed
            })
            .ToListAsync(cancellationToken);

        return Ok(new
        {
            items,
            total,
            page,
            pageSize
        });
    }


    [HttpPost("add")]
    public async Task<ActionResult<TodoDto>> AddTodo([FromBody] CreateTodoRequest request, CancellationToken cancellationToken)
    { 
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Title is required.");

        var entity = new TodoItem
        {
            Title = request.Title.Trim(),
            Completed = false
        };

        _db.Todos.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        var dto = new TodoDto { Id = entity.Id, Title = entity.Title, Completed = entity.Completed };
        return Ok(dto);
    }
     
    [HttpPost("update-status")]
    public async Task<ActionResult<TodoDto>> UpdateStatus([FromBody] UpdateTodoStatusRequest request, CancellationToken cancellationToken)
    {
        var entity = await _db.Todos
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (entity is null)
            return NotFound();

        entity.Completed = request.Completed;
        await _db.SaveChangesAsync(cancellationToken);
        var dto = new TodoDto { Id = entity.Id, Title = entity.Title, Completed = entity.Completed };
        return Ok(dto);
    }
     
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] int id, CancellationToken cancellationToken)
    {
        var entity = await _db.Todos
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (entity is null)
            return NotFound();

        _db.Todos.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
