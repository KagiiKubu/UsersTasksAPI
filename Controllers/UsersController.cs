using Microsoft.AspNetCore.Mvc;
using UsersTasksAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UsersTasksAPI.Controllers
{

    // Tells ASP.NET that this is a API Controller class
    [ApiController]
    // Sets base url for all routes in this controller
    [Route("api/[controller]")]

    public class UsersController : ControllerBase{

        //inject DataContext into controller so it can interact with DB
        private readonly DataContext _context;

        public UsersController(DataContext context){
            _context = context;
        }


        // returns all users from DB
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers(){

            // Use EF Core to get all users from the Users table
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/users/{Id} (returns single user by their ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id){

            // Use EF Core to find user by ID in DB
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        //POST: api/users (Add new user)
        [HttpPost]
        public async Task<ActionResult<User>> AddNewUser(User newUser){

            // Add new user to DB
            await _context.Users.AddAsync(newUser);

            // save changes to DB
            await _context.SaveChangesAsync();

            // Return 201 Created, with a link to get the newly created user
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.UserID }, newUser);
        }

        // PUT: api/users/{Id} (update existing user by Id)
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUserById(int id, User updatedUser){
            
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.Username = updatedUser.Username;
            user.EmailAddress = updatedUser.EmailAddress;
            user.Password = updatedUser.Password;

            // save changes to DB
            await _context.SaveChangesAsync();

            return NoContent(); //returns 204 meaning success
        }

         // DELETE: api/users/{Id} (remove existing user by Id)
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> RemoveUserById(int id){
            
            //look for user in the DB using ID
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            // Remove new user to DB
            _context.Users.Remove(user);

            // save changes to DB
            await _context.SaveChangesAsync();

            return NoContent(); //returns 204 meaning success
        }


    }
}