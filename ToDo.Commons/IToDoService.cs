using System.Collections.Generic;
using ToDo.Common.DTOs;
using ToDo.Common.ServiceRequests;

namespace ToDo.Common;

public interface IToDoService
{
    void AddToDo(ToDoItem toDoItem);
    IEnumerable<ToDoItem> GetToDoItems();
    ToDoItem GetToDoItem(Guid id);
    void MarkAsComplete(Guid id);
    void UpdateToDoText(Guid id, string updatedText);
    void DeleteToDoItem(Guid id);
}
