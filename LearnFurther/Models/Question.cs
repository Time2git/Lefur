using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Models
{
    public class Question
    {
        public short Id { get; set; }//в mysql меняем на ushort
        public string Context { get; set; }
        public short TaskId { get; set; }
        public  Task Task { get; set; }
        public  IList<UserAnswer> UserAnswers { get; set; }
        public  IList<Answer> Answers { get; set; }
    }
}