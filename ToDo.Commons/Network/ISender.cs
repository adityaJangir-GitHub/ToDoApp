namespace ToDo.Common.Network;

public interface ISender : IDisposable
{
    TResponse Send<T, TResponse>(T message) where TResponse : new();
}



