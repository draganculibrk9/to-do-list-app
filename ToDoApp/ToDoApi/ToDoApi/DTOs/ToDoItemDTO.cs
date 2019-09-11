using Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApi.DTOs
{
    public class ToDoItemDTO
    {
        [Required]
        public string Description { get; set; }
        public Guid Id { get; set; }
        public Guid ToDoListId { get; set; }
        public int Position { get; set; }
        public bool Completed { get; set; } = false;

        public ToDoItemDTO() { }

        public ToDoItemDTO(ToDoItem item)
        {
            Description = item.Description;
            Id = item.Id;
            ToDoListId = item.ToDoListId;
            Position = item.Position;
            Completed = item.Completed;
        }

        public ToDoItem ToEntity(ToDoList list)
        {
            ToDoItem item = new ToDoItem()
            {
                Description = Description,
                Id = Id,
                ToDoListId = ToDoListId,
                ToDoList = list,
                Position = list.Position,
                Completed = Completed
            };

            return item;
        }
    }
}
