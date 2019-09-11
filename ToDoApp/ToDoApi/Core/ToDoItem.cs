using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class ToDoItem
    {
        public string Description { get; set; }
        public Guid Id { get; set; }

        public Guid ToDoListId { get; set; }

        public ToDoList ToDoList { get; set; }

        public int Position { get; set; }

        public bool Completed { get; set; } = false;

        public ToDoItem() { }

        public ToDoItem(ToDoItem item)
        {
            Id = item.Id;
            Description = item.Description;
            ToDoListId = item.ToDoListId;
            ToDoList = item.ToDoList;
            Position = item.Position;
            Completed = item.Completed;
        }

        public void Update(ToDoItem changedItem)
        {
            Description = changedItem.Description;
            Id = changedItem.Id;
            ToDoListId = changedItem.ToDoListId;
            ToDoList = changedItem.ToDoList;
            Completed = changedItem.Completed;
        }

        public void UpdatePosition(int newPosition)
        {
            List<ToDoItem> requiresUpdate;

            if (newPosition > Position)
            {
                requiresUpdate = ToDoList.Items.Where(i => i.Position <= newPosition && i.Position > Position).ToList();
                foreach (ToDoItem item in requiresUpdate)
                {
                    item.Position--;
                }
            }
            else if (newPosition < Position)
            {
                requiresUpdate = ToDoList.Items.Where(i => i.Position >= newPosition && i.Position < Position).ToList();
                foreach (ToDoItem item in requiresUpdate)
                {
                    item.Position++;
                }
            }
            Position = newPosition;
        }
    }
}
