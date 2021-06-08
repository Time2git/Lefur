using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Models
{
    public class Notification
    {
        public short Id { get; set; }//в mysql меняем на ushort
        public int UserId { get; set; }
        public User User { get; set; }
        public string Context { get; set; }
        public bool HasBeenRead { get; set; }
    }
}
