using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Models
{
    public class UserAnswer
    {
        public short Id { get; set; }
        public string Content { get; set; }
        public bool State { get; set; }
        public int UserId { get; set; }
        public  User User { get; set; }
        public short QuestionId { get; set; }
        public  Question Question { get; set; }
    }
}
