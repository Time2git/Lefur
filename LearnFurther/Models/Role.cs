using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Models
{
    public class Role : IdentityRole<int>
    {
        public Role() : base()
        {

        }
        //public ushort RoleId { get; set; }
        //public string Name { get; set; }
        //public virtual ICollection<User> Users { get; set; }
        public Role(string roleName) : base(roleName)
        {
        }
    }
}