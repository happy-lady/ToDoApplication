using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApplication.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }

        [ForeignKey("UserDto")]
        public int UserId { get; set; } // foreign key
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
