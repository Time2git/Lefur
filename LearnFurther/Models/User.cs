using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LearnFurther.Models
{
    public class User : IdentityUser//<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Task> Tasks { get; set; }
        public IList<UsersWithAccessToTheTask> Users { get; set; }
        //public int? RoleId { get; set; }
        //public virtual Role Role { get; set; }
        //public virtual ICollection<Result> Results { get; set; }
        //public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}