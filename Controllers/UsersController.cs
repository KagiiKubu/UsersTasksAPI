using Microsoft.AspNetCore.Mvc;
using UsersTasksAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersTasksAPI.Repositories;

namespace UsersTasksAPI.Controllers
{

    // Tells ASP.NET that this is a API Controller class
    [ApiController]
    // Sets base url for all routes in this controller
    [Route("api/[controller]")]

    public class UsersController : ControllerBase{

        //inject IUserRepository into controller so it can access DB via repository pattern
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository){

            _userRepository = userRepository;
        }


        // returns all users from DB
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers(){

            // Use EF Core to get all users from the Users table
            var users = await _userRepository.GetAllUsers();
            return Ok(users);
        }

        // GET: api/users/{Id} (returns single user by their ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id){

            // Use EF Core to find user by ID in DB
            var user = await _userRepository.GetUserById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        //POST: api/users (Add new user)
        [HttpPost]
        public async Task<ActionResult<User>> AddNewUser(User newUser){

            // Add new user to DB
            var success = await _userRepository.AddNewUser(newUser);

            if (!success)
                return BadRequest("Could not add the user.");

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.UserID }, newUser);
        }

        // PUT: api/users/{Id} (update existing user by Id)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserById(int id, User updatedUser){
            
            updatedUser.UserID = id;
            var success = await _userRepository.UpdateUser(updatedUser);

            if (!success)
                return NotFound();

            return NoContent(); //returns 204 meaning success
        }

         // DELETE: api/users/{Id} (remove existing user by Id)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(int id){
            
            var success = await _userRepository.DeleteUserById(id);

            if (!success)
                return NotFound();

            return NoContent(); //returns 204 meaning success
        }


    }
}