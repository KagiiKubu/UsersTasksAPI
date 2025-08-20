using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UsersTasksAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UsersTasksAPI.Repositories;


namespace UsersTasksAPI.Controllers
{

    // Tells ASP.NET that this is a API Controller class
    [ApiController]
    // Sets base url for all routes in this controller
    [Route("api/[controller]")]

    public class TasksController : ControllerBase{

        //inject ITaskRepository into controller so it can access DB via repository pattern
        private readonly ITaskRepository _taskRepository;

        public TasksController(ITaskRepository taskRepository){

            _taskRepository = taskRepository;
        }



        // GET: api/Tasks (returns all Tasks in DB)
        [HttpGet]
        public async Task<ActionResult<List<UserTask>>> GetAllTasks(){

            // Use EF Core to get all tasks from the Users table
            var tasks = await _taskRepository.GetAllTasks();
            return Ok(tasks);
        }

        // GET: api/Tasks/{Title} (returns single Task by their Title)
        [HttpGet("{title}")]
        public async Task<ActionResult<UserTask>> GetTaskByTitle(string title){

            // Use EF Core to find task by title in DB
            var task = await _taskRepository.GetTaskByTitle(title);

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        //POST: api/Tasks (Add new Task)
        [HttpPost]
        public async Task<ActionResult<UserTask>> AddNewTask(UserTask newTask){

            //Add new Task to DB
            var success = await _taskRepository.AddNewTask(newTask);

            if (!success)
                return BadRequest("Could not add the task.");

            return CreatedAtAction(nameof(GetTaskByTitle), new { title = newTask.Title }, newTask);
        }

        // PUT: api/Tasks/{id} (update existing Task by id)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, UserTask updatedTask){
            
            // Use EF Core to find task by title in DB
            var success = await _taskRepository.UpdateTask(id, updatedTask);

            if (!success)
                return NotFound();

            return NoContent(); //returns 204 meaning success
        }

         // DELETE: api/Tasks/{title} (remove existing Task by Title)
        [HttpDelete("{title}")]
        public async Task<IActionResult> DeleteTaskByTitle(string title){
            
            // Use EF Core to find task by title in DB
            var success = await _taskRepository.DeleteTaskByTitle(title);

            if (!success)
                return NotFound();

            return NoContent(); //returns 204 meaning success
        }

        // GET: api/Tasks/{expired} (returns all expired tasks)
        [HttpGet("expired")]
        public async Task<ActionResult<List<UserTask>>> GetExpiredTasks(){

            // Use EF Core to get all tasks from the Users table
            var expiredTasks = await _taskRepository.GetExpiredTasks();

            return Ok(expiredTasks);
        }


        // GET: api/Tasks/{active} (returns all active tasks)
        [HttpGet("active")]
        public async Task<ActionResult<List<UserTask>>> GetActiveTasks(){

            var activeTasks = await _taskRepository.GetActiveTasks();

            return Ok(activeTasks);
        }

        // GET: api/Tasks/{givenDate} (returns all tasks by given date)
        [HttpGet("date/{givenDate}")]
        public async Task<ActionResult<List<UserTask>>> GetTasksByDate(DateOnly givenDate){

            var tasksByDate = await _taskRepository.GetTasksByDate(givenDate);

            return Ok(tasksByDate);
        }

        
        // GET: api/Tasks/{assignee} (returns all tasks by assignee)
        [HttpGet("{assigneeId}")]
        public async Task<IEnumerable<UserTask>> GetTasksByAssignee(int assigneeId){

            return await _taskRepository.GetTasksByAssignee(assigneeId);
        }


    }
}