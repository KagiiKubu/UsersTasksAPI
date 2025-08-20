using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersTasksAPI.Models;


namespace UsersTasksAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();

        Task<User> GetUserById(int id);
        Task<bool> AddNewUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUserById(int id);

    }
}