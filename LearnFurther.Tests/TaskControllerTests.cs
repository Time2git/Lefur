using System;
using System.Threading.Tasks;
using LearnFurther;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearnFurther.Controllers;
using Moq;
using LearnFurther.ViewModels;
using LearnFurther.Models;
using Microsoft.AspNetCore.Identity;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnFurther.Tests
{
    public class TaskControllerTests
    {
        [Fact]
        public async System.Threading.Tasks.Task TaskExecuteReturnsViewWithExecuteTaskViewModel()
        {
            //Arrange
            short taskId = 5;
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseInMemoryDatabase("lefurdb");
            var _dbContext = new ApplicationContext(optionsBuilder.Options);
            _dbContext.Questions.Add(new Question { Id = 1, TaskId = 5, Context = "Rnek" });
            _dbContext.Tasks.Add(new Models.Task { Id = 5, Title = "Задание в виде теста", 
                Description = "Описание задания", Types = 0 });//добавляем новое задание
            var controller = new TaskController(null, _dbContext);

            ////Act
            var result = controller.TaskExecuteAsync(taskId);

            ////Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ExecuteTaskViewModel>(viewResult.Model);
            Assert.Equal("Задание в виде теста", model.Title);
            Assert.Equal(taskId, model.Id);
        }
        [Fact]
        public async System.Threading.Tasks.Task AddTaskReturnOkWithAddTaskViewModel()
        {
            //Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseInMemoryDatabase("lefurdb");
            var _dbContext = new ApplicationContext(optionsBuilder.Options);
            var controller = new TaskController(null, _dbContext);
            AddTaskViewModel model = new()
            {
                Title = "Новое задание",
                Description = "Описание задания",
                Types = (ViewModels.TaskTypes)1
            };
            //Act
            var result = controller.AddTask(model);
            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var task = Assert.IsType< Models.Task >(viewResult.Value);
            Assert.Equal(1,task.Id);
        }
    }
}