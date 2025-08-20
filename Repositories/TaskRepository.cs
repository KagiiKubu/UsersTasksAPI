using Microsoft.EntityFrameworkCore;
using UsersTasksAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<bool> UpdateTask(string title, UserTask updatedTask){
            
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Title == title);

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

            var expiredTasksList = new List<UserTask>();

            var allTasks = await _context.Tasks.ToListAsync();

            foreach (var task in allTasks){

                if (task.DueDate < DateOnly.FromDateTime(DateTime.Now))
                    expiredTasksList.Add(task);

            }

            return expiredTasksList;
        }

        // returns all active tasks
        public async Task<IEnumerable<UserTask>> GetActiveTasks(){

            var activeTasksList = new List<UserTask>();

            var allTasks = await _context.Tasks.ToListAsync();

            foreach (var task in allTasks){

                if (task.DueDate >= DateOnly.FromDateTime(DateTime.Now))
                    activeTasksList.Add(task);

            }

            return activeTasksList;
        }

        // returns all tasks by given date
        public async Task<IEnumerable<UserTask>> GetTasksByDate(DateOnly givenDate){

            var tasksByDateList = new List<UserTask>();

            var allTasks = await _context.Tasks.ToListAsync();

            foreach (var task in allTasks){

                if (task.DueDate == givenDate)
                    tasksByDateList.Add(task);

            }

            return tasksByDateList;
        }

        // returns all tasks by assignee
        public async Task<IEnumerable<UserTask>> GetTasksByAssignee(int assigneeId){

            var tasksByAssignee = new List<UserTask>();

            var allTasks = await _context.Tasks.ToListAsync();

            foreach (var task in allTasks){

                if (task.Assignee == assigneeId)
                    tasksByAssignee.Add(task);

            }

            return tasksByAssignee;
        }

    }

}