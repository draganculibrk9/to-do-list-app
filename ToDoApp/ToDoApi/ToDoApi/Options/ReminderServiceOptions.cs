using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Options
{
    public class ReminderServiceOptions
    {
        public int ReminderInterval { get; set; }
        public string ApiKey { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
