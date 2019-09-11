using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ToDoApi.DTOs
{
    public class ToDoListDTO
    {
        [StringLength(20)]
        public string Title { get; set; }
        public List<ToDoItemDTO> Items { get; set; } = new List<ToDoItemDTO>();
        public Guid Id { get; set; }
        public DateTime ReminderDate { get; set; }
        public int Position { get; set; }
        public bool Reminded { get; set; }

        public ToDoListDTO() { }

        public ToDoListDTO(ToDoList list)
        {
            Title = list.Title;
            Id = list.Id;
            ReminderDate = list.ReminderDate;
            Position = list.Position;
            Reminded = list.Reminded;
            Items = list.Items.OrderBy(x => x.Position).Select(i => new ToDoItemDTO(i)).ToList();
        }

        public ToDoList ToEntity()
        {
            ToDoList list = new ToDoList
            {
                Id = Id,
                ReminderDate = ReminderDate,
                Title = Title,
                Position = Position,
                Reminded = Reminded
            };

            return list;
        }
    }
}
