namespace ToDo.Common.Network;

public interface IReciver : IDisposable
{
    void StartListning(Func<object, object> onMessageReceive);
}
