using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Models
{
    public enum TaskTypes
    {
        Test,
        TaskWithFullSolution
    }
    public class Task
    {
        public short TaskId { get; set; }//в mysql меняем на ushort
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual IList<Question> Questions { get; set; }
        public virtual ICollection<Result> Results { get; set; }
        public TaskTypes Types { get; set; }
        public User Author { get; set; }
    }
}