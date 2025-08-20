using Microsoft.EntityFrameworkCore;
using UsersTasksAPI.Models;


namespace UsersTasksAPI.Models
{
    // This class tells EF Core what tables to create in our Database
    public class DataContext : DbContext{

        // connection string is passed to DbContext class
        public DataContext (DbContextOptions<DataContext> options) : base(options){
        }
        // set Dbset properties to define tables based on models in Database
        public DbSet<User> Users {get; set;}
        public DbSet<UserTask> Tasks {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            
            modelBuilder.Entity<UserTask>().HasKey(t => t.TaskId); // explicit primary key
        }

    }
    
}