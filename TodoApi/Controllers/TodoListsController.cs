using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/todolists")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoListsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/todolists
        [HttpGet]
        public async Task<ActionResult<IList<TodoList>>> GetTodoLists()
        {
            List<TodoList> todoLists = await _context.TodoList
                .Include(t => t.Items)
                .ToListAsync();            
                        
            return Ok(todoLists);

            // return Ok(await _context.TodoList.ToListAsync()); 
        }

        // GET: api/todolists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> GetTodoList(long id)
        {
            // var todoList = await _context.TodoList.FindAsync(id);

            TodoList todoList = new TodoList() { Name = string.Empty };
            
            todoList = await _context.TodoList
            .Include(t => t.Items)
            .FirstOrDefaultAsync(t => t.Id == id);

                if (todoList == null)                
                    return NotFound();
                

                return Ok(todoList);
        }

        // PUT: api/todolists/5
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutTodoList(long id, UpdateTodoList payload)
        {
            // var todoList = await _context.TodoList.FindAsync(id);      
            TodoList todoList = new TodoList() { Name = string.Empty };
            todoList = await _context.TodoList.FindAsync(id);

            if (todoList == null)
            {
                return NotFound();
            }

            todoList.Name = payload.Name;
            await _context.SaveChangesAsync();

            return Ok(todoList);
        }

        [HttpPost]
        public async Task<ActionResult<TodoList>> PostTodoList(CreateTodoList payload)
        {
            var todoList = new TodoList { Name = payload.Name };

            _context.TodoList.Add(todoList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoList", new { id = todoList.Id }, todoList);
        }

        // DELETE: api/todolists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoList(long id)
        {
            //var todoList = await _context.TodoList.FindAsync(id);            
            TodoList todoList = new TodoList() { Name = string.Empty };
            todoList = await _context.TodoList.FindAsync(id);

            if (todoList == null)
            {
                return NotFound();
            }

            _context.TodoList.Remove(todoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("TodoListExists")]
        public async Task<ActionResult<bool>> TodoListExists([FromQuery, Required] long id)
        {
            bool todolistExists = _context.TodoList.Any(e => e.Id == id);
            return Ok(todolistExists);            
        }

        [HttpPost("Item")]
        public async Task<ActionResult<TodoList>> PostTodoListItem([FromQuery, Required] long id, CreateTodoListItem payload)
        {
            // var todoList = new TodoList { Name = payload.Name };
            TodoList todoList = new TodoList() { Name = payload.Name };
            todoList = await _context.TodoList
                .Include(t => t.Items)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (todoList == null)
            {
                return NotFound();
            }
            else
            {
                long nextItemId = 1;
                if (todoList.Items.Any())
                    nextItemId = todoList.Items.Max(i => i.ItemId) + 1;

                TodoListItem todoListItem = new TodoListItem
                {
                    Id = id,
                    ItemId = nextItemId,
                    Name = payload.Name,
                    Status = payload.Status
                };

                todoList.Items.Add(todoListItem);

                //_context.TodoList.Add(todoList);
                await _context.SaveChangesAsync();

                return Ok(todoListItem);
            }
        }

        [HttpPut("Item")]
        public async Task<IActionResult> PutTodoListItem([FromQuery, Required] long id, [FromQuery, Required] long itemId, CreateTodoListItem payload)
        {
            // var todoList = new TodoList { Name = payload.Name };
            TodoList todoList = new TodoList() { Name = payload.Name };
            todoList = await _context.TodoList
                .Include(t => t.Items)
                .FirstOrDefaultAsync(t => t.Id == id);

            TodoListItem todoListItem = await _context.TodoListItems
                .FirstOrDefaultAsync(i => i.Id == id && i.ItemId == itemId);

            if (todoListItem == null)
                return NotFound();

            todoListItem.Name = payload.Name;
            todoListItem.Status = payload.Status;

            await _context.SaveChangesAsync();

            return Ok(todoListItem);
        }

        [HttpDelete("Item")]
        public async Task<IActionResult> DeleteTodoListItem([FromQuery, Required] long id, [FromQuery, Required] long itemId)
        {
            // var todoList = new TodoList { Name = payload.Name };
            TodoList todoList = new TodoList() { Name = string.Empty };
            todoList = await _context.TodoList
                .Include(t => t.Items)
                .FirstOrDefaultAsync(t => t.Id == id);

            var todoListItem = await _context.TodoListItems
                .FirstOrDefaultAsync(i => i.Id == id && i.ItemId == itemId);

            if (todoListItem == null)
                return NotFound();

            _context.TodoListItems.Remove(todoListItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
