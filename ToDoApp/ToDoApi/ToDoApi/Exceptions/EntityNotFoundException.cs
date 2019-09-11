using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }
    }
}
