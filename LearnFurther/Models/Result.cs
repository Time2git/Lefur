using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Models
{
    public class Result
    {
        public short Id { get; set; }
        public byte Grade { get; set; }
        public string UserId { get; set; }
        public  User User { get; set; }
        public short TaskId { get; set; }
        public  Task Task { get; set; }
    }
}