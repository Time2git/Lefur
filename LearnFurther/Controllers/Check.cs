using Microsoft.AspNetCore.Mvc;
using LearnFurther.Models;
using LearnFurther.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LearnFurther.Controllers
{
    public class Check
    {
        public CheckViewModel CheckTestTask(ExecuteTaskViewModel model, Models.Task task)
        {
            int AnswCount = 0;
            int RightAnswCount = 0;
            for (int i = 0; i < model.Questions.Count; i++)
            {
                for (int j = 0; j < model.Questions[i].Answers.Count; j++)
                {
                    AnswCount++;
                    if (task.Questions[i].Answers[j].State.Equals(model.Questions[i].UserAnswers[j].State))
                    {
                        RightAnswCount++;
                    }
                }
            }
            int persents = (RightAnswCount * 100 / AnswCount);
            CheckViewModel model1 = new()
            {
                AnswersCount = AnswCount,
                RightAnswersCount = RightAnswCount,
                //grade = (byte)//высчитываем проценты выполненных заданий, затем нужно основываясь на них высчитать оценку
            };
            if (persents < 50)
            {
                model1.Grade = 2;
            }
            if ((persents >= 50) && (persents < 70))
            {
                model1.Grade = 3;
            }
            if ((persents >= 70) && (persents < 85))
            {
                model1.Grade = 4;
            }
            if ((persents >= 85) && (persents <= 100))
            {
                model1.Grade = 5;
            }
            return (model1);
        }
    }
}