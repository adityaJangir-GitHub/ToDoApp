using ToDo.Common;
using ToDo.Common.DTOs;

namespace ToDo.DB;

public class ToDoService : IToDoService
{
    IToDoRepository _dbService;
    public ToDoService(IToDoRepository dbService)
    {
        _dbService = dbService;
    }
    public void AddToDo(ToDoItem toDoItem)
    {
     _dbService.Add(toDoItem);
    }

    public void DeleteToDoItem(Guid id)
    {
        _dbService.Delete(id);
    }

    public ToDoItem GetToDoItem(Guid id)
    {
        var toDoItem = _dbService.GetById(id);
        return toDoItem;
    }

    public IEnumerable<ToDoItem> GetToDoItems()
    {
        return _dbService.Get();
    }

    public void MarkAsComplete(Guid id)
    {
        _dbService.MarkAsComplete(id);
    }

    public void UpdateToDoText(Guid id, string updatedText)
    {
        _dbService.Update(id, updatedText);        
    }
}
