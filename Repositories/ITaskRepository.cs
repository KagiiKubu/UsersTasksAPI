using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersTasksAPI.Models;


namespace UsersTasksAPI.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<UserTask>> GetAllTasks();

        Task<UserTask> GetTaskByTitle(string title);
        Task<bool> AddNewTask(UserTask newTask);
        Task<bool> UpdateTask(int id, UserTask task);
        Task<bool> DeleteTaskByTitle(string title);

        Task<IEnumerable<UserTask>> GetExpiredTasks();
        Task<IEnumerable<UserTask>> GetActiveTasks();
        Task<IEnumerable<UserTask>> GetTasksByDate(DateOnly givenDate);
        Task<IEnumerable<UserTask>> GetTasksByAssignee(int assigneeId);
    }
}