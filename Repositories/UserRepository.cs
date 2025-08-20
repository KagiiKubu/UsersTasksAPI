

namespace UsersTasksAPI.Repositories{

    public class UserRepository : IUserRepository{

        private readonly DataContext _context;
        
        public UserRepository(DataContext context){

            _context = context;
        }

        // Get All Users from DB
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // Get single user by ID
        public async Task<User> GetUserById(int id){

            // Use EF Core to find user by ID in DB
            return await _context.Users.FindAsync(id);
        }

        // Add new user
        public async Task<bool> AddNewUser(User newUser){

            await _context.Users.AddAsync(newUser);
            return await _context.SaveChangesAsync() > 0;         //converts it to bool
        }

        // update existing user
        public async Task<bool> UpdateUser(User updatedUser){
            
            var user = await _context.Users.FindAsync(updatedUser.UserID);

            if (user == null)
                return false;

            user.Username = updatedUser.Username;
            user.EmailAddress = updatedUser.EmailAddress;
            user.Password = updatedUser.Password;

            // save changes to DB
            return await _context.SaveChangesAsync() > 0;
        }

        // remove existing user by Id
        public async Task<bool> RemoveUserById(int id){
            
            //look for user in the DB using ID
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return false;

            // Remove new user to DB
            _context.Users.Remove(user);

            // save changes to DB
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
