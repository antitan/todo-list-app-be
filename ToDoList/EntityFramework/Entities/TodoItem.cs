
using System.ComponentModel.DataAnnotations;


namespace ToDoList.EntityFramework.Entities
{
    public sealed class TodoItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        public bool Completed { get; set; }
    }
}
