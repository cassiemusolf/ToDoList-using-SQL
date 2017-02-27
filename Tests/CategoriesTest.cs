using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=ToDoList_Test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      int result = Category.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      Category firstCategory = new Category("Household chores");
      Category secondCategory = new Category("Household chores");

      Assert.Equal(firstCategory, secondCategory);
    }

    [Fact]
    public void Test_Save_SavesCategoryToDatabase()
    {
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToCategoryObject()
    {
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Category savedCategory = Category.GetAll()[0];

      int result = savedCategory.GetId();
      int testId = testCategory.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCategoryInDatabase()
    {
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Category foundCategory = Category.Find(testCategory.GetId());

      Assert.Equal(testCategory, foundCategory);

    }

    [Fact]
    public void Test_Delete_DeletesCategoryFromDatabase()
    {
      string name1 = "Home stuff";
      Category testCategory1 = new Category(name1);
      testCategory1.Save();

      string name2 = "Work stuff";
      Category testCategory2 = new Category(name2);
      testCategory2.Save();

      testCategory1.Delete();
      List<Category> resultCategories = Category.GetAll();
      List<Category> testCategoryList = new List<Category> {testCategory2};

      Assert.Equal(testCategoryList, resultCategories);
    }

    [Fact]
    public void Test_AddTask_AddsTaskToCategory()
    {
      DateTime date1 = new DateTime(2008, 4, 10);
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Task testTask = new Task("Mow the lawn", date1);
      testTask.Save();

      Task testTask2 = new Task("Water the garden", date1);
      testTask2.Save();

      testCategory.AddTask(testTask);
      testCategory.AddTask(testTask2);

      List<Task> result = testCategory.GetTasks();
      List<Task> testList = new List<Task>{testTask, testTask2};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_GetTasks_ReturnsAllCatergoryTasks()
    {
      DateTime date1 = new DateTime(2008, 4, 10);
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Task testTask1 = new Task("Mow the lawn", date1);
      testTask1.Save();

      Task testTask2 = new Task("Buy plane ticket", date1);
      testTask2.Save();

      testCategory.AddTask(testTask1);
      List<Task> savedTasks = testCategory.GetTasks();
      List<Task> testList = new List<Task> {testTask1};

      Assert.Equal(testList, savedTasks);
    }

    [Fact]
    public void Test_Delete_DeletesCategoryAssociationsFromDatabase()
    {
      DateTime date1 = new DateTime(2008, 4, 10);
      Task testTask = new Task("Mow the lawn", date1);
      testTask.Save();

      string testName = "Home Stuff";
      Category testCategory = new Category(testName);
      testCategory.Save();

      testCategory.AddTask(testTask);
      testCategory.Delete();

      List<Category> resultTaskCategories = testTask.GetCategories();
      List<Category> testTaskCategories = new List<Category> {};

      Assert.Equal(testTaskCategories, resultTaskCategories);
    }

    public void Dispose()
    {
      Task.DeleteAll();
      Category.DeleteAll();
    }
  }
}
