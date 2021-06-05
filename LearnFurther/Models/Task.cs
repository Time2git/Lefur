using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public short Id { get; set; }//в mysql меняем на ushort
        public string Title { get; set; }
        public string Description { get; set; }
        public  IList<Question> Questions { get; set; }
        public  ICollection<Result> Results { get; set; }
        public TaskTypes Types { get; set; }
        //[ForeignKey("UserId")]
        public int/*?*/ UserId { get; set; }
        public User Author { get; set; }
        public IList<UsersWithAccessToTheTask> Users { get; set; }
    }
}