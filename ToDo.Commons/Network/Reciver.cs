using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;

namespace ToDo.Common.Network;

public class Reciver : IReciver
{
    public IPAddress IP { get; private set; }
    public int Port { get; private set; }
    private TcpListener _listener;
    private TcpClient _client;
    private NetworkStream _stream;
    private StreamReader _reader;
    private StreamWriter _writer;
    private bool _disposed = false;
    public Reciver(ReciverConfig config)
    {
        IP = IPAddress.Parse(config.IP);
        Port = config.Port;
        _listener = new TcpListener(IP, Port);
        _listener.Start();
        _client = _listener.AcceptTcpClient();
        _stream = _client.GetStream();
        _reader = new StreamReader(_stream);
        _writer = new StreamWriter(_stream);
    }
    public void Dispose()
    {
        if (!_disposed)
        {
            _writer.Dispose();
            _reader.Dispose();
            _stream.Dispose();
            _client.Dispose();
            _listener.Stop();
            //how to properly dispose a tcpListner
        }
        _disposed = true;
    }
    public void StartListning(Func<object, object> onMessageReceive)
    {
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
        var jsonMessage = _reader.ReadLine();
        var message = JsonConvert.DeserializeObject(jsonMessage, settings);

        if (message is not null)
        {
            var response = onMessageReceive(message);
            var jsonResponse = JsonConvert.SerializeObject(response, settings);
            _writer.WriteLine(jsonResponse);
            _writer.Flush();
        }
    }

    ~Reciver() 
    {
        Dispose();
    }
}
public class ReciverConfig
{
    public string IP { get; set; }
    public int Port { get; set; }
}