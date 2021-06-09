using System;
using System.Collections.Generic;
using LearnFurther.Models;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.ViewModels
{
    public class UserListViewModel : User
    {
        public string Role { get; set; }
    }
}