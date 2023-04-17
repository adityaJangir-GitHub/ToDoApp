using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;

namespace ToDo.Common.Network;

public class Sender : ISender
{
    public IPAddress IP { get; private set; }
    public int Port { get; private set; }
    private IPEndPoint _endPoint;
    private TcpClient _tcpClient;
    private NetworkStream _stream;
    private StreamWriter _writer;
    private StreamReader _reader;
    private bool _disposed = false;

    public Sender(SenderConfig config)
    {
        IP = IPAddress.Parse(config.IP);
        Port = config.Port; 
        _tcpClient = new();
        _tcpClient.Connect(IP, Port);
        _stream = _tcpClient.GetStream();
        _writer = new(_stream);
        _reader = new(_stream);

    }
    public void Dispose()
    {
        if (!_disposed)
        {
            _tcpClient?.Dispose();
            _stream?.Dispose();
            _writer?.Dispose();
            _reader?.Dispose();
        }
        _disposed = true;

    }
    public TResponse Send<T, TResponse>(T message) where TResponse : new()
    {
        TResponse response;
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
        var jsonMessage = JsonConvert.SerializeObject(message, settings);
        _writer.WriteLine(jsonMessage);
        _writer.Flush();

        var jsonRespose = _reader.ReadLine();
        response = JsonConvert.DeserializeObject<TResponse>(jsonRespose, settings);
        return response ?? new TResponse();
    }

    ~Sender() 
    {
        Dispose();
    }
}

public class SenderConfig
{
    public string IP { get; set; }
    public int Port { get; set; }
}




