using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Models
{
    public class Result
    {
        public short Resultid { get; set; }
        public byte grade { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public int? TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}