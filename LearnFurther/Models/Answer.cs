using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Models
{
    public class Answer
    {
        [Key]
        public short Answerid { get; set; }//в mysql меняем на ushort
        public string Content { get; set; }
        public bool State { get; set; }
        //public int? QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}