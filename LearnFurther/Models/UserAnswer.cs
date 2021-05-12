using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Models
{
    public class UserAnswer
    {
        public ushort UserAnswerid { get; set; }
        public string Content { get; set; }
        public bool State { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public int? QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
