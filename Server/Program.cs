using ToDo.Common;
using ToDo.Common.Network;
using ToDo.Common.ServiceRequests;
using ToDo.DB;
using Unity;
using Unity.Injection;

IUnityContainer container = new UnityContainer();

container.RegisterInstance(new ReciverConfig { IP = "127.0.0.1", Port = 6000 });
container.RegisterType<IReciver, Reciver>();
container.RegisterType<IToDoRepository, ToDoRepository>(new InjectionConstructor(new object[] { "Data Source=Laptop-nf4s8qom;Initial Catalog=ToDoApp;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True" }));
container.RegisterType<IToDoService, ToDoService>();

var messageReciver = container.Resolve<IReciver>();
while (true)
{
    messageReciver.StartListning(MessageRecived);
}
object MessageRecived(object message)
{
    IToDoService toDoService = container.Resolve<IToDoService>();
    object response = null;
    if (message != null)
    {
        if (message is AddToDoRequest add)
        {
            toDoService.AddToDo(add.ToDo);
            return CreateDefaultResponse();
        }
        else if (message is GetAllToDosRequest)
        {
            var result = toDoService.GetToDoItems();
            return new GetAllToDoResponse { IsSuccess = true, ToDoData = result.ToList() };
        }
        else if (message is GetToDoRequest get)
        {
            var result = toDoService.GetToDoItem(get.Id);
            return new GetToDoResponse { IsSuccess = true, ToDoData = result };
        }
        else if (message is UpdateToDoRequest update)
        {
            toDoService.UpdateToDoText(update.ToDo.Id, update.ToDo.ToDo);
            return CreateDefaultResponse();
        }
        else if (message is MarkAsCompleteRequest markAsComplete)
        {
            toDoService.MarkAsComplete(markAsComplete.Id);
            return CreateDefaultResponse();
        }
        else if (message is DeleteToDoRequest delete)
        {
            toDoService.DeleteToDoItem(delete.Id);
            return CreateDefaultResponse();
        }

    }
    return response;

}




ToDoResponse CreateDefaultResponse()
{
    return new ToDoResponse()
    {
        IsSuccess = true       
    };
}


