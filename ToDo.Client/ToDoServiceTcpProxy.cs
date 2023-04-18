using ToDo.Common;
using ToDo.Common.DTOs;
using ToDo.Common.Network;
using ToDo.Common.ServiceRequests;

namespace ToDo.Client.Console;
public class ToDoServiceTcpProxy : IToDoService
{
    private ISender _sender;  
    public ToDoServiceTcpProxy(ISender sender)
    {
        _sender = sender;
    }

    public void AddToDo(ToDoItem toDoItem)
    {
        var request = new AddToDoRequest() { ToDo = toDoItem };        
        var response = _sender.Send<AddToDoRequest,ToDoResponse>(request);
        if (response == null || response.IsSuccess == false)
        {
            //log 
            throw new Exception(response?.ErrorMessage ?? string.Empty);
        }   
    }

    public void UpdateToDoText(Guid id, string updatedText)
    {
        var toDo = new ToDoItem() { Id = id, ToDo = updatedText };
        var request = new UpdateToDoRequest() { ToDo = toDo };
        var response = _sender.Send<UpdateToDoRequest, ToDoResponse>(request);
        
    }

    public IEnumerable<ToDoItem> GetToDoItems()
    {
        var request = new GetAllToDosRequest();
        var response = _sender.Send<GetAllToDosRequest, GetAllToDoResponse>(request);
     
        return response.ToDoData;
    }

    public void MarkAsComplete(Guid id)
    {
        var request = new MarkAsCompleteRequest() {  Id = id };
        var response = _sender.Send<MarkAsCompleteRequest, ToDoResponse>(request);       
    }

    public ToDoItem GetToDoItem(Guid id)
    {
        var request = new GetToDoRequest() { Id = id };
        var response = _sender.Send<GetToDoRequest, GetToDoResponse>(request);
        return response.ToDoData;
    }

    public void DeleteToDoItem(Guid id)
    {
        var request = new DeleteToDoRequest() { Id = id };
        var response = _sender.Send<DeleteToDoRequest, ToDoResponse>(request);
    }
}

