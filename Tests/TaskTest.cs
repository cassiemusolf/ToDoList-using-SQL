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
      Task firstTask = new Task("Mow the lawn", 1, date1);
      Task secondTask = new Task("Mow the lawn", 1, date1);

      //Assert
      Assert.Equal(firstTask, secondTask);
    }
    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      DateTime date1 = new DateTime(2008, 4, 10);
      Task testTask = new Task("Mow the lawn", 1, date1);
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
      Task testTask = new Task("Mow the lawn", 1, date1);

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
      Task testTask = new Task("Mow the lawn", 1, date1);
      testTask.Save();

      //Act
      Task foundTask = Task.Find(testTask.GetId());

      //Assert
      Assert.Equal(testTask, foundTask);
    }
  }
}
