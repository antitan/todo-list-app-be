
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Dtos
{

    public class TodoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; } 
     }

    public class CreateTodoRequest
    {
        [Required, MaxLength(300)]
        public string Title { get; set; }
    }

    public sealed class UpdateTodoStatusRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool Completed { get; set; }
    }
}
