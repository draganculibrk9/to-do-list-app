using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class ToDoList
    {
        public string Title { get; set; }
        public List<ToDoItem> Items { get; set; } = new List<ToDoItem>();
        public Guid Id { get; set; }
        public DateTime ReminderDate { get; set; }
        public int Position { get; set; }
        public bool Reminded { get; set; }
        public string Owner { get; set; }
        public List<ToDoListShare> ToDoListShares { get; set; } = new List<ToDoListShare>();

        public void Update(ToDoList changedList)
        {
            Title = changedList.Title;
            Id = changedList.Id;
            ReminderDate = changedList.ReminderDate;
            Reminded = changedList.Reminded;
        }
    }
}
