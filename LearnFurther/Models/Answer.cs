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
        public ushort Answerid { get; set; }
        public string Content { get; set; }
        public bool state { get; set; }
        public int? QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}