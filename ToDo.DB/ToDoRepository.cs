using Microsoft.Data.SqlClient;
using System.Data;
using ToDo.Common.DTOs;
using ToDo.Common.Enums;
namespace ToDo.DB;
public class ToDoRepository : IToDoRepository
{
    private string _connectionString;
    public ToDoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Add(ToDoItem toDoItem)
    {
        string query = $"Insert into ToDoItem values (@Id,@ToDo,@Status)";
        ExecuteNonQuery(query, new SqlParameter[] {
                                new("@Id", toDoItem.Id),
                                new("@ToDo", toDoItem.ToDo),
                                new ("@Status", toDoItem.Status)
                        });
    }

    public IEnumerable<ToDoItem> Get()
    {
        List<ToDoItem> toDoItems = new();
        
        string query = "select * from ToDoItem";

        var dataTable = ExecuteQuery(query);
        foreach(DataRow row in dataTable.Rows)
        {
            ToDoItem toDoItem = new();

            toDoItem.Id = Guid.Parse(row["id"].ToString());
            toDoItem.ToDo = row["ToDo"].ToString();
            toDoItem.Status = (ToDoItemStatus)Convert.ToInt32(row["Status"]);

            toDoItems.Add(toDoItem);
        }
        return toDoItems;
    }

    public ToDoItem GetById(Guid id)
    {
        ToDoItem toDoItem = new();
        string query = $"Select * from ToDoItem where Id = @Id";

        var dataTable = ExecuteQuery(query, new SqlParameter[] { new("@Id", id) });

        toDoItem.Id = Guid.Parse(dataTable.Rows[0]["id"].ToString());
        toDoItem.ToDo = dataTable.Rows[0]["ToDo"].ToString();
        toDoItem.Status = (ToDoItemStatus)Convert.ToInt32(dataTable.Rows[0]["Status"]);

        return toDoItem;
    }

    public void Update(Guid id, string toDo)
    {
        string qurey = $"Update ToDoItem set ToDo=@ToDoValue where Id=@Id";
        ExecuteNonQuery(qurey, new SqlParameter[] {
                                new("@ToDoValue", toDo),
                                new("@Id", id)
                                });
    }

    public void MarkAsComplete(Guid id)
    {
        string query = $"Update ToDoItem set Status = 1 where Id=@Id";
        ExecuteNonQuery(query, new SqlParameter[] {
                                new("@Id", id)
                                });
    }

    public void Delete(Guid id)
    {
        string query = $"Delete from ToDoItem where Id=@Id";
        ExecuteNonQuery(query, new SqlParameter[] {
                                new("@Id", id)
                                });
    }

    public int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
    {
        using SqlConnection connection = new(_connectionString);
        connection.Open();
        SqlCommand command = new(query, connection);

        if (parameters != null)
        {
            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
        }

        return command.ExecuteNonQuery();
    }

    public DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
    {
        using SqlConnection connection = new(_connectionString);
        connection.Open();
        SqlCommand command = new(query, connection);

        if (parameters != null)
        {
            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
        }

        SqlDataAdapter adapter = new(command);
        DataTable dataTable = new();
        adapter.Fill(dataTable);
        return dataTable;
    }
}