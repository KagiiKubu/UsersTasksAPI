using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace UsersTasksAPI.Models{
    public class Task{

        public Task() { } // Parameterless constructor for EF Core

        //Properties
        public int TaskId{get; set;}
        public string Title{get; set;}
        public string Description{get; set;}
        public int Assignee{get; set;}
        public DateOnly DueDate{get; set;}

    }
}