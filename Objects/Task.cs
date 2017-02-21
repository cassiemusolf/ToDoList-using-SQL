using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ToDoList
{
  public class Task
  {
    private int _id;
    private string _description;
    private int _categoryId;
    private DateTime _duedate;

    public Task(string Description, int CategoryId, DateTime DueDate, int Id = 0)
    {
      _id = Id;
      _categoryId = CategoryId;
      _description = Description;
      _duedate = DueDate;
    }

    public int GetId()
    {
      return _id;
    }

    public int GetCategoryId()
    {
      return _categoryId;
    }
    public void SetCategoryId(int newCategoryId)
    {
      _categoryId = newCategoryId;
    }
    public string GetDescription()
    {
      return _description;
    }

    public DateTime GetDueDate()
    {
      return _duedate;
    }

    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public override bool Equals(System.Object otherTask)
    {
      if (!(otherTask is Task))
      {
        return false;
      }
      else
      {
        Task newTask = (Task) otherTask;
        bool idEquality = (this.GetId() == newTask.GetId());
        bool descriptionEquality = (this.GetDescription() == newTask.GetDescription());
        bool categoryEquality = (this.GetCategoryId() == newTask.GetCategoryId());
        return (idEquality && descriptionEquality);
      }
    }
    public static List<Task> GetAll()
    {
      List<Task> allTasks = new List<Task>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskDescription = rdr.GetString(1);
        DateTime taskDueDate = rdr.GetDateTime(2);
        int taskCategory = rdr.GetInt32(3);
        Task newTask = new Task(taskDescription, taskCategory, taskDueDate, taskId);
        allTasks.Add(newTask);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allTasks;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO tasks (name, category_id, due_date) OUTPUT INSERTED.id VALUES (@TaskDescription, @TaskCategoryId, @TaskDueDate);", conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@TaskDescription";
      descriptionParameter.Value = this.GetDescription();

      SqlParameter categoryParameter = new SqlParameter();
      categoryParameter.ParameterName = "@TaskCategoryId";
      categoryParameter.Value = this.GetCategoryId();

      SqlParameter dueDateParameter = new SqlParameter();
      dueDateParameter.ParameterName = "@TaskDueDate";
      dueDateParameter.Value = this.GetDueDate();

      cmd.Parameters.Add(categoryParameter);
      cmd.Parameters.Add(descriptionParameter);
      cmd.Parameters.Add(dueDateParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM tasks;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
    public static Task Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE id = @TaskId;", conn);
      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = id.ToString();
      cmd.Parameters.Add(taskIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundTaskId = 0;
      int foundTaskCategory = 0;
      string foundTaskDescription = null;
      DateTime foundDueDate = new DateTime();

      while(rdr.Read())
      {
        foundTaskId = rdr.GetInt32(0);
        foundTaskDescription = rdr.GetString(1);
        foundDueDate = rdr.GetDateTime(2);
        foundTaskCategory = rdr.GetInt32(3);
      }
      Task foundTask = new Task(foundTaskDescription, foundTaskCategory, foundDueDate, foundTaskId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundTask;
    }
  }
}
