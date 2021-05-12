using LearnFurther.Models;
using System.Collections.Generic;

namespace LearnFurther.ViewModels
{
    public class ShowTaskViewModel
    {
        public short Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<Question> Questions { get; set; }
        public ICollection<Result> Results { get; set; }
        public User TestPerson { get; set; }
    }
}