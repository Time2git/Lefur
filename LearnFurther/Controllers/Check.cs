using Microsoft.AspNetCore.Mvc;
using LearnFurther.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnFurther.Controllers
{
    public class Check
    {
        public CheckViewModel CheckTestTask(ExecuteTaskViewModel model)
        {
            int AnswCount = 0;
            int RightAnswCount = 0;
            for (int i = 0; i < model.Questions.Count; i++)
            {
                for (int j = 0; j < model.Questions[i].Answers.Count; j++)
                {
                    AnswCount++;
                    if (model.Questions[i].Answers[j].State.Equals(model.Questions[i].UserAnswers[j].State))
                    {
                        RightAnswCount++;
                    }
                }
            }
            CheckViewModel model1 = new()
            {
                AnswersCount = AnswCount,
                RightAnswersCount = RightAnswCount,
                //grade = (byte)(RightAnswCount * 100 / AnswCount)//высчитываем проценты выполненных заданий, затем нужно основываясь на них высчитать оценку
            };
            return (model1);
        }
    }
}
