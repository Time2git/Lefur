using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Models
{
    public class Question
    {
        public short QuestionId { get; set; }//в mysql меняем на ushort
        public string Context { get; set; }
        public int? TaskId { get; set; }
        public virtual Task Task { get; set; }
        public virtual IList<UserAnswer> UserAnswers { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}