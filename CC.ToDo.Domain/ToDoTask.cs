using System;
using System.Collections.Generic;
using System.Text;

namespace CC.ToDo.Domain
{
    public class ToDoTask
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsComplete { get; set; }
        public bool IsRemoved { get; set; }
    }

    public class User
    {
        public Guid Id { get; set; }
        public IList<ToDoTask> Tasks { get; set; }
    }
}
