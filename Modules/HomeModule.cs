using Nancy;
using System;
using System.Collections.Generic;

namespace ToDoList
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };

      Get["/tasks"] = _ => {
        List<Task> AllTasks = Task.GetAll();
        return View["tasks.cshtml", AllTasks];
      };

      Get["/categories"] = _ => {
        List<Category> allCategories = Category.GetAll();
        return View["categories.cshtml", allCategories];
      };

      Get["/categories/new"] = _ => {
        return View["categories_form.cshtml"];
      };

      Post["/categories/new"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        return View["success.cshtml"];
      };

      Get["/tasks/new"] = _ => {
        return View["tasks_form.cshtml"];
      };

      Post["/tasks/new"] = _ => {
        Task newTask = new Task(Request.Form["task-description"], Request.Form["due-date"]);
        newTask.Save();
        return View["success.cshtml"];
      };

      Post["/tasks/delete"] = _ => {
        Task.DeleteAll();
        return View["cleared.cshtml"];
      };

      Get["/categories/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Category SelectedCategory = Category.Find(parameters.id);
        List<Task> CategoryTasks = SelectedCategory.GetTasks();
        List<Task> AllTasks = Task.GetAll();
        model.Add("category", SelectedCategory);
        model.Add("categoryTasks", CategoryTasks);
        model.Add("allTasks", AllTasks);
        return View["category.cshtml", model];
      };

      Get["tasks/{id}"] = parameters => {
        Dictionary<string,object> model = new Dictionary<string, object>();
        Task SelectedTask = Task.Find(parameters.id);
        List<Category> TaskCategories = SelectedTask.GetCategories();
        List<Category> allCategories = Category.GetAll();
        model.Add("task", SelectedTask);
        model.Add("TaskCategories", TaskCategories);
        model.Add("allCategories", allCategories);
        return View["task.cshtml", model];
      };

      Post["task/add_category"] = _ => {
        Category category = Category.Find(Request.Form["category-id"]);
        Task task = Task.Find(Request.Form["task-id"]);
        List<Category> taskCategories = task.GetCategories();
        bool tracker = false;
        foreach (Category thisCategory in taskCategories)
        {
          if (category.GetName()== thisCategory.GetName())
          {
            tracker = true;
          }
        }
        if (!tracker)
        {
          task.AddCategory(category);
        }
        return View["success.cshtml"];
      };

      Post["category/add_task"] = _ => {
        Category category = Category.Find(Request.Form["category-id"]);
        Task task = Task.Find(Request.Form["task-id"]);
        category.AddTask(task);
        return View["success.cshtml"];
      };
    }
  }
}
