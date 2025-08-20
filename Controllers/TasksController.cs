using Microsoft.AspNetCore.Mvc;
using UsersTasksAPI.Models;
using Microsoft.EntityFrameworkCore;
using UserTask = UsersTasksAPI.Models.Task;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UsersTasksAPI.Controllers
{

    // Tells ASP.NET that this is a API Controller class
    [ApiController]
    // Sets base url for all routes in this controller
    [Route("api/[controller]")]

    public class TasksController : ControllerBase{

        //inject DataContext into controller so it can interact with DB
        private readonly DataContext _context;

        public TasksController(DataContext context){
            _context = context;
        }


        // GET: api/Tasks (returns all Tasks in DB)
        [HttpGet]
        public async Task<ActionResult<List<UserTask>>> GetAllTasks(){

            // Use EF Core to get all tasks from the Users table
            var tasks = await _context.Tasks.ToListAsync();
            return Ok(tasks);
        }

        // GET: api/Tasks/{Title} (returns single Task by their Title)
        [HttpGet("{title}")]
        public async Task<ActionResult<UserTask>> GetTaskByTitle(string title){

            // Use EF Core to find task by title in DB
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Title == title);

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        //POST: api/Tasks (Add new Task)
        [HttpPost]
        public async Task<ActionResult<UserTask>> AddNewTask(UserTask newTask){

            //Add new Task to DB
            await _context.Tasks.AddAsync(newTask);

            // Save changes to DB
            await _context.SaveChangesAsync();

            // Return 201 Created, with a link to get the newly created Task
            return CreatedAtAction(nameof(GetTaskByTitle), new { title = newTask.Title }, newTask);
        }

        // PUT: api/Tasks/{title} (update existing Task by Title)
        [HttpPut("{title}")]
        public async Task<IActionResult> UpdateTaskByTitle(string title, UserTask updatedTask){
            
            // Use EF Core to find task by title in DB
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Title == title);

            if (task == null)
                return NotFound();

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Assignee = updatedTask.Assignee;
            task.DueDate = updatedTask.DueDate;

            // Save changes to DB
            await _context.SaveChangesAsync();

            return NoContent(); //returns 204 meaning success
        }

         // DELETE: api/Tasks/{title} (remove existing Task by Title)
        [HttpDelete("{title}")]
        public async Task<IActionResult> RemoveTaskByTitle(string title){
            
            // Use EF Core to find task by title in DB
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Title == title);

            if (task == null)
                return NotFound();

            //Add new Task to DB
            _context.Tasks.Remove(task);

            // Save changes to DB
            await _context.SaveChangesAsync();

            return NoContent(); //returns 204 meaning success
        }

        // GET: api/Tasks/{expired} (returns all expired tasks)
        [HttpGet("expired")]
        public async Task<ActionResult<List<UserTask>>> GetExpiredTasks(){

            //create new list for expired tasks
            var expiredTasksList = new List<UserTask>();

            // Use EF Core to get all tasks from the Users table
            var allTasks = await _context.Tasks.ToListAsync();

            //loop though task list to find expired tasks
            foreach (var task in allTasks){

                if (task.DueDate < DateOnly.FromDateTime(DateTime.Now))
                    expiredTasksList.Add(task);

            }

            return Ok(expiredTasksList);
        }


        // GET: api/Tasks/{active} (returns all active tasks)
        [HttpGet("active")]
        public async Task<ActionResult<List<UserTask>>> GetActiveTasks(){

            //create new list for active tasks
            var activeTasksList = new List<UserTask>();

            // Use EF Core to get all tasks from the Users table
            var allTasks = await _context.Tasks.ToListAsync();

            //loop though task list to find active tasks
            foreach (var task in allTasks){

                if (task.DueDate >= DateOnly.FromDateTime(DateTime.Now))
                    activeTasksList.Add(task);

            }

            return Ok(activeTasksList);
        }

        // GET: api/Tasks/{givenDate} (returns all tasks by given date)
        [HttpGet("date/{givenDate}")]
        public async Task<ActionResult<List<UserTask>>> GetTasksByDate(DateOnly givenDate){

            //create new list for given date tasks
            var tasksByDateList = new List<UserTask>();

            // Use EF Core to get all tasks from the Users table
            var allTasks = await _context.Tasks.ToListAsync();

            //loop though task list to find given date tasks
            foreach (var task in allTasks){

                if (task.DueDate == givenDate)
                    tasksByDateList.Add(task);

            }

            return Ok(tasksByDateList);
        }

        
        // GET: api/Tasks/{assignee} (returns all tasks by assignee)
        [HttpGet("user/{assigneeId}")]
        public async Task<ActionResult<List<UserTask>>> GetTasksByAssignee(int assigneeId){

            //create new list for given assignee's tasks
            var tasksByAssignee = new List<UserTask>();

            // Use EF Core to get all tasks from the Users table
            var allTasks = await _context.Tasks.ToListAsync();

            //loop though task list to find given assignee's tasks
            foreach (var task in allTasks){

                if (task.Assignee == assigneeId)
                    tasksByAssignee.Add(task);

            }

            return Ok(tasksByAssignee);
        }


    }
}