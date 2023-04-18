using ToDo.Client.Console;
using ToDo.Common;
using ToDo.Common.DTOs;
using ToDo.Common.Enums;
using ToDo.Common.ServiceRequests;
using ToDo.Common.Network;
using Unity;

IUnityContainer container = new UnityContainer();

container.RegisterInstance(new SenderConfig { IP = "127.0.0.1", Port = 6000 });
container.RegisterType<ISender, Sender>();
container.RegisterType<IToDoService, ToDoServiceTcpProxy>("Tcp Proxy");
container.RegisterType<IToDoService, ToDoServiceHttpProxy>("Http Proxy");
var toDoService = ChooseProtocol(container);


var flag = true;
while (flag)
{
    DisplayToDoServices.Display();
    Console.WriteLine("Select an option listed above : ");
    if (!int.TryParse(Console.ReadLine(), out int inputValue))
        continue;
    Console.WriteLine((ToDoServiceChoice)inputValue);
    var response = new ToDoResponse();

    switch ((ToDoServiceChoice)inputValue)
    {
        case ToDoServiceChoice.Add:
            {
                Console.Write("Please enter the todo item: ");
                var todoItem = Console.ReadLine();
                SafeActionCall(() => toDoService.AddToDo(new ToDoItem() { Id = Guid.NewGuid(), ToDo = todoItem }));
                break;
            }

        case ToDoServiceChoice.ViewAll:
            {

                var toDoResponse = SafeFunctionCall(() => toDoService.GetToDoItems());
                DisplayToDoItemOnConsole(toDoResponse);
                break;
            }

        case ToDoServiceChoice.View:
            {
                Console.WriteLine("Please enter the Id of the todo to be viewed: ");
                var idToUpdate = Console.ReadLine();

                if (Guid.TryParse(idToUpdate, out Guid guid))
                {
                    var toDoResponse = toDoService.GetToDoItem(guid);

                    if(toDoResponse is not null)
                        Console.WriteLine($"{toDoResponse.Id} {toDoResponse.Status} {toDoResponse.ToDo}");
                }

                break;
            }

        case ToDoServiceChoice.Update:
            {
                Console.WriteLine("Please enter the Id of the todo to be Updated: ");
                var idToUpdate = Console.ReadLine();
                Console.WriteLine("Enter the updated text: ");
                var updatedText = Console.ReadLine();

                if (Guid.TryParse(idToUpdate, out Guid guid))
                {
                    toDoService.UpdateToDoText(guid, updatedText);
                }

                break;
            }

        case ToDoServiceChoice.MarkAsComplete:
            {
                Console.WriteLine("Please enter the Id of the todo to be mark as complete: ");
                var id = Console.ReadLine();
                if (Guid.TryParse(id, out Guid guid))
                {
                    toDoService.MarkAsComplete(guid);
                }
                break;
            }

        case ToDoServiceChoice.Delete:
            {
                Console.WriteLine("Please enter the Id of the todo to be delete: ");
                var id = Console.ReadLine();
                if (Guid.TryParse(id, out Guid guid))
                {
                    toDoService.DeleteToDoItem(guid);
                }
                break;
            }

        case ToDoServiceChoice.Exit:
            {
                flag = false;
                break;
            }
    }
}

void DisplayToDoItemOnConsole(IEnumerable<ToDoItem> toDosItems)//map it properly
{
    if (toDosItems is null)
        return;
    foreach (var toDoItem in toDosItems)
        Console.WriteLine($"{toDoItem.Id} {toDoItem.Status} {toDoItem.ToDo}");  
}

void SafeActionCall(Action func)
{
    try
    {
        func();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

T SafeFunctionCall<T>(Func<T> func)
{
    try
    {
        return func();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        return default(T);
    }
}

static IToDoService ChooseProtocol(IUnityContainer container)
{
    IToDoService toDoService;
    while (true)
    {
        Console.WriteLine("Please select the protocol for \n 1. Tcp \n 2. Http");
        if (int.TryParse(Console.ReadLine(), out int protocol))
        {
            if ((Protocols)protocol == Protocols.Tcp)
            { 
                toDoService = container.Resolve<IToDoService>("Tcp Proxy");
                break;
            }
            else if ((Protocols)protocol == Protocols.Http)
            { 
                toDoService = container.Resolve<IToDoService>("Http Proxy");
                break;
            }
            else
            continue;       
        }
    }

    return toDoService;
}