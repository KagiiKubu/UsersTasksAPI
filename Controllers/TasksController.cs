using Microsoft.AspNetCore.Mvc;
using UsersTasksAPI.Models;
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

        //Temp list to store Tasks (simulates DB)
        private static List <Models.Task> tasksList = new List<Models.Task>();

        // GET: apli/Tasks (returns all Tasks in list)
        [HttpGet]
        public ActionResult<List<Models.Task>> GetAllTasks(){
            return tasksList;
        }

        // GET: apli/Tasks/{Title} (returns single Task by their Title)
        [HttpGet("{title}")]
        public ActionResult<Models.Task> GetTaskByTitle(string title){

            //look for Task in the list using its title
            var task = tasksList.FirstOrDefault(u => u.Title == title);

            if (task == null)
                return NotFound();

            return task;
        }

        //POST: api/Tasks (Add new Task)
        [HttpPost]
        public ActionResult<List<Models.Task>> AddNewTask(Models.Task newTask){

            //Add new Task to list
            tasksList.Add(newTask);

            // Return 201 Created, with a link to get the newly created Task
            return CreatedAtAction(nameof(GetTaskByTitle), new { id = newTask.Title }, newTask);
        }

        // PUT: apli/Tasks/{title} (update existing Task by Title)
        [HttpPut("{title}")]
        public ActionResult<List<Models.Task>> UpdateTaskById(string title, Models.Task updatedTask){
            
            //look for Task in the list using ID
            var task = tasksList.FirstOrDefault(u => u.Title == title);

            if (task == null)
                return NotFound();

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Assignee = updatedTask.Assignee;
            task.DueDate = updatedTask.DueDate;

            return NoContent(); //returns 204 meaning success
        }

         // DELETE: apli/Tasks/{title} (remove existing Task by Title)
        [HttpDelete("{title}")]
        public ActionResult<List<Models.Task>> RemoveTaskByTitle(string title){
            
            //look for Task in the list using ID
            var task = tasksList.FirstOrDefault(u => u.Title == title);

            if (task == null)
                return NotFound();

            tasksList.Remove(task);

            return NoContent(); //returns 204 meaning success
        }

                // GET: apli/Tasks/{expired} (returns all expired tasks)
        [HttpGet("expired")]
        public ActionResult<List<Models.Task>> GetExpiredTasks(){

            //create new list for expired tasks
            var expiredTasksList = new List<Models.Task>();

            //loop though task list to find expired tasks
            foreach (var task in tasksList){

                if (task.DueDate < DateOnly.FromDateTime(DateTime.Now))
                    expiredTasksList.Add(task);

            }

            return expiredTasksList;
        }


                // GET: apli/Tasks/{active} (returns all active tasks)
        [HttpGet("active")]
        public ActionResult<List<Models.Task>> GetActiveTasks(){

            //create new list for active tasks
            var activeTasksList = new List<Models.Task>();

            //loop though task list to find active tasks
            foreach (var task in tasksList){

                if (task.DueDate >= DateOnly.FromDateTime(DateTime.Now))
                    activeTasksList.Add(task);

            }

            return activeTasksList;
        }

        // GET: apli/Tasks/{givenDate} (returns all tasks by given date)
        [HttpGet("date/{givenDate}")]
        public ActionResult<List<Models.Task>> GetTasksByDate(DateOnly givenDate){

            //create new list for given date tasks
            var tasksByDateList = new List<Models.Task>();

            //loop though task list to find given date tasks
            foreach (var task in tasksList){

                if (task.DueDate == givenDate)
                    tasksByDateList.Add(task);

            }

            return tasksByDateList;
        }

        
        // GET: apli/Tasks/{assignee} (returns all tasks by assignee)
        [HttpGet("user/{assigneeId}")]
        public ActionResult<List<Models.Task>> GetTasksByAssignee(int assigneeId){

            //create new list for given assignee's tasks
            var tasksByAssignee = new List<Models.Task>();

            //loop though task list to find given assignee's tasks
            foreach (var task in tasksList){

                if (task.Assignee == assigneeId)
                    tasksByAssignee.Add(task);

            }

            return tasksByAssignee;
        }


    }
}