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



        /// <summary>
        /// Returns all tasks.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<UserTask>>> GetAllTasks(){

            // Use EF Core to get all tasks from the Users table
            var tasks = await _taskRepository.GetAllTasks();
            return Ok(tasks);
        }

        /// <summary>
        /// Returns tasks by title.
        /// </summary>
        [HttpGet("{title}")]
        public async Task<ActionResult<UserTask>> GetTaskByTitle(string title){

            // Use EF Core to find task by title in DB
            var task = await _taskRepository.GetTaskByTitle(title);

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        /// <summary>
        /// Adds new task.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserTask>> AddNewTask(UserTask newTask){

            //Add new Task to DB
            var success = await _taskRepository.AddNewTask(newTask);

            if (!success)
                return BadRequest("Could not add the task.");

            return CreatedAtAction(nameof(GetTaskByTitle), new { title = newTask.Title }, newTask);
        }

        /// <summary>
        /// Updates existing task.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, UserTask updatedTask){
            
            // Use EF Core to find task by title in DB
            var success = await _taskRepository.UpdateTask(id, updatedTask);

            if (!success)
                return NotFound();

            return NoContent(); //returns 204 meaning success
        }

         /// <summary>
        /// Deletes task
        /// </summary>
        [HttpDelete("{title}")]
        public async Task<IActionResult> DeleteTaskByTitle(string title){
            
            // Use EF Core to find task by title in DB
            var success = await _taskRepository.DeleteTaskByTitle(title);

            if (!success)
                return NotFound();

            return NoContent(); //returns 204 meaning success
        }

        /// <summary>
        /// Returns all expired tasks.
        /// </summary>
        [HttpGet("expired")]
        public async Task<ActionResult<List<UserTask>>> GetExpiredTasks(){

            // Use EF Core to get all tasks from the Users table
            var expiredTasks = await _taskRepository.GetExpiredTasks();

            return Ok(expiredTasks);
        }


        /// <summary>
        /// Returns all active tasks
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<List<UserTask>>> GetActiveTasks(){

            var activeTasks = await _taskRepository.GetActiveTasks();

            return Ok(activeTasks);
        }

        /// <summary>
        /// Returns tasks by given date
        /// </summary>
        [HttpGet("date/{givenDate}")]
        public async Task<ActionResult<List<UserTask>>> GetTasksByDate(DateOnly givenDate){

            var tasksByDate = await _taskRepository.GetTasksByDate(givenDate);

            return Ok(tasksByDate);
        }

        
        /// <summary>
        /// Returns tasks by given assignee
        /// </summary>
        [HttpGet("{assigneeId}")]
        public async Task<IEnumerable<UserTask>> GetTasksByAssignee(int assigneeId){

            return await _taskRepository.GetTasksByAssignee(assigneeId);
        }


    }
}