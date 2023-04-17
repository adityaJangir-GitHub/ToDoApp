using ToDo.Common.DTOs;

namespace ToDo.DB;

public interface IToDoRepository
{
    void Add(ToDoItem toDo);
    IEnumerable<ToDoItem> Get();
    ToDoItem GetById(Guid id);
    void Update(Guid id, string toDoItem);
    void MarkAsComplete(Guid id);
    void Delete(Guid id);
}
