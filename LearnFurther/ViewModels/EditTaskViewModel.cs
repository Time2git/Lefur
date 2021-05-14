using System;
using LearnFurther.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.ViewModels
{
    public class EditTaskViewModel
    {
        public short Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<Question> Questions { get; set; }
    }
}