using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UsersTasksAPI.Models;
using System.Collections.Generic;
using System.Linq;


namespace UsersTasksAPI.Repositories{

    public class TaskRepository : ITaskRepository{

        private readonly DataContext _context;
        
        public TaskRepository(DataContext context){

            _context = context;
        }

        // returns all Tasks in DB
        public async Task<IEnumerable<UserTask>> GetAllTasks(){

            return await _context.Tasks.ToListAsync();
        }


         // returns single Task by their Title
        public async Task<UserTask> GetTaskByTitle(string title){

            return await _context.Tasks.FirstOrDefaultAsync(t => t.Title == title);
        }

        // Add new Task
        public async Task<bool> AddNewTask(UserTask newTask){

            await _context.Tasks.AddAsync(newTask);
            return await _context.SaveChangesAsync() > 0;
        }

        // update existing Task
        public async Task<bool> UpdateTask(int id, UserTask updatedTask){
            
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return false;

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Assignee = updatedTask.Assignee;
            task.DueDate = updatedTask.DueDate;

            return await _context.SaveChangesAsync() > 0;
        }

        // remove existing Task by Title
        public async Task<bool> DeleteTaskByTitle(string title){
            
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Title == title);

            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            return await _context.SaveChangesAsync() > 0;
        }

        // returns all expired tasks
        public async Task<IEnumerable<UserTask>> GetExpiredTasks(){

            // Get current date
            var today = DateOnly.FromDateTime(DateTime.Now);

            // only get tasks with duedate < today
            return await _context.Tasks
                                .Where(t => t.DueDate < today)
                                .ToListAsync();
        }

        // returns all active tasks
        public async Task<IEnumerable<UserTask>> GetActiveTasks(){

            // Get current date
            var today = DateOnly.FromDateTime(DateTime.Now);

            // only get tasks with duedate >= today
            return await _context.Tasks
                                .Where(t => t.DueDate >= today)
                                .ToListAsync();
        }

        // returns all tasks by given date
        public async Task<IEnumerable<UserTask>> GetTasksByDate(DateOnly givenDate){


            // only get tasks with duedate = givenDate
            return await _context.Tasks
                                .Where(t => t.DueDate == givenDate)
                                .ToListAsync();
        }

        // returns all tasks by assignee
        public async Task<IEnumerable<UserTask>> GetTasksByAssignee(int assigneeId){

            // only get tasks with specified assignee
            return await _context.Tasks
                                .Where(t => t.Assignee == assigneeId)
                                .ToListAsync();
        }

    }

}