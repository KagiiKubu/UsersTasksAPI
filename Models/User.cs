using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.VisualBasic;

namespace UsersTasksAPI.Models{
    public class User{
        public User() { } // Parameterless constructor for EF Core

        //Properties
        public int  UserID{get; set;}
        public string Username{get; set;}
        public string EmailAddress{get; set;}
        public string Password{get; set;}


    }
}