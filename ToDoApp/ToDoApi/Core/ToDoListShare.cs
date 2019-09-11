using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class ToDoListShare
    {
        public Guid Id { get; set; }
        public DateTime ExpiresOn { get; set; }
        public Guid ToDoListId { get; set; }
        public ToDoList ToDoList { get; set; }

        public ToDoListShare() { }

        public ToDoListShare(ToDoList list)
        {
            ToDoList = list;
            ToDoListId = list.Id;
            Id = Guid.NewGuid();
            ExpiresOn = DateTime.Now.AddHours(2);
        }
    }
}
