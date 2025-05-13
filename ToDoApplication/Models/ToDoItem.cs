using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApplication.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; } // foreign key
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
