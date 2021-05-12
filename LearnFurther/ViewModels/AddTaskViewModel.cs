using System;
using System.Collections.Generic;
using LearnFurther.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.ViewModels
{
    public enum TaskTypes
    {
        [Display(Name = "Тест")]
        Test,
        [Display(Name = "Задание с полным ответом")]
        TaskWithFullSolution
    }
    public class AddTaskViewModel
    {
        [Display(Name = "Тема")]
        public string Title { get; set; }
        [Display(Name = "Описание задания")]
        public string Description { get; set; }
        //[Display(Name = "Тип задания")]
        public TaskTypes Types { get; set; }
        public IList<Question> Questions { get; set; }
    }
}