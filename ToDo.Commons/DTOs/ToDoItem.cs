using ToDo.Common.Enums;

namespace ToDo.Common.DTOs
{
    public class ToDoItem
    {
        public string ToDo { get; set; }
        public ToDoItemStatus Status { get; set; }
        public Guid Id { get; set; }

        public ToDoItem()
        {

        }

        public ToDoItem(Guid guid, string itemName, ToDoItemStatus status)
        {
            Id = guid;
            ToDo = itemName;
            Status = status;
        }
    }
}
