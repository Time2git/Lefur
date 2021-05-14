using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Models
{
    public class Answer
    {
        public short Id { get; set; }//в mysql меняем на ushort
        public string Content { get; set; }
        public bool State { get; set; }
        public short QuestionId { get; set; }
        public Question Question { get; set; }
    }
}