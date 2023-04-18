using ToDo.Common.DTOs;

namespace ToDo.Common.ServiceRequests
{
    public class GetAllToDoResponse : ToDoResponse
    {
        public List<ToDoItem> ToDoData { get; set;}
    }
    public class GetToDoResponse : ToDoResponse
    {
        public ToDoItem ToDoData { get; set;}
    }
}
