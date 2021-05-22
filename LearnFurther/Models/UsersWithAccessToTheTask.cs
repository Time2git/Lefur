﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Models
{
    public class UsersWithAccessToTheTask
    {
        public short Id { get; set; }
        public short TaskId { get; set; }
        public Task Task { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}