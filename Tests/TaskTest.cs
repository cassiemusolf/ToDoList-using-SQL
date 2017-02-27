using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class ToDoTest : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=ToDoList_Test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Task.DeleteAll();
      Category.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Task.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      DateTime date1 = new DateTime(2008, 4, 10);
      Task firstTask = new Task("Mow the lawn", date1);
      Task secondTask = new Task("Mow the lawn", date1);

      //Assert
      Assert.Equal(firstTask, secondTask);
    }
    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      DateTime date1 = new DateTime(2008, 4, 10);
      Task testTask = new Task("Mow the lawn", date1);
      //Act
      testTask.Save();
      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};
      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      DateTime date1 = new DateTime(2008, 4, 10);
      Task testTask = new Task("Mow the lawn", date1);

      //Act
      testTask.Save();
      Task savedTask = Task.GetAll()[0];

      int result = savedTask.GetId();
      int testId = testTask.GetId();

      //Assert
      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      //Arrange
      DateTime date1 = new DateTime(2008, 4, 10);
      Task testTask = new Task("Mow the lawn", date1);
      testTask.Save();

      //Act
      Task foundTask = Task.Find(testTask.GetId());

      //Assert
      Assert.Equal(testTask, foundTask);
    }
    [Fact]
    public void Test_AddCategory_AddsCategoryToTask()
    {
      DateTime date1 = new DateTime(2008, 4, 10);
      Task testTask = new Task("Mow the lawn", date1);
      testTask.Save();

      Category testCategory = new Category("Home Stuff");
      testCategory.Save();

      testTask.AddCategory(testCategory);

      List<Category> result = testTask.GetCategories();
      List<Category> testList = new List<Category>{testCategory};

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_GetCategories_ReturnsAllTaskCategories()
    {
      DateTime date1 = new DateTime(2008, 4, 10);
      Task testTask = new Task("Mow the lawn", date1);
      testTask.Save();

      Category testCategory1 = new Category("Home stuff");
      testCategory1.Save();

      Category testCategory2 = new Category("Work stuff");
      testCategory2.Save();

      testTask.AddCategory(testCategory1);
      List<Category> result = testTask.GetCategories();
      List<Category> testList = new List<Category> {testCategory1};

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Delete_DeletesTaskAssociationsFromDatabase()
    {
      Category testCategory = new Category("Home stuff");
      testCategory.Save();

      string testDescription = "Mow the lawn";
      DateTime date1 = new DateTime(2008, 4, 10);
      Task testTask = new Task(testDescription, date1);
      testTask.Save();

      testTask.AddCategory(testCategory);
      testTask.Delete();

      List<Task> resultCategoryTasks = testCategory.GetTasks();
      List<Task> testCategoryTasks = new List<Task> {};

      Assert.Equal(testCategoryTasks, resultCategoryTasks);
    }
    [Fact]
    public void Test_MarkDone_MarkTaskAsDone()
    {
      DateTime date1 = new DateTime(2008, 4, 10);
      Task testTask = new Task("Mow the lawn", date1);
      testTask.Save();
      testTask.MarkDone();
      Task actual = Task.GetAll()[0];

      Assert.Equal(true, actual.GetStatus());
    }
  }
}
