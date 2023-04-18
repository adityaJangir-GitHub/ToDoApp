using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using ToDo.Common;
using ToDo.Common.DTOs;

namespace ToDo.Client.Console
{
    public class ToDoServiceHttpProxy : IToDoService
    {
        private HttpClient _httpClient;
        private const string BASE_API_ADDRESS = "http://localhost:5096/ToDoService";
        public ToDoServiceHttpProxy()
        {
            _httpClient = new HttpClient();
        }

        public void AddToDo(ToDoItem toDoItem)
        {
            var json = JsonConvert.SerializeObject(toDoItem);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{BASE_API_ADDRESS}");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            _httpClient.Send(request);
        }
        public IEnumerable<ToDoItem> GetToDoItems()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BASE_API_ADDRESS}");
            var response = _httpClient.Send(request);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<IEnumerable<ToDoItem>>(content);
                return result;
            }
            return null;
        }

        public ToDoItem GetToDoItem(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BASE_API_ADDRESS}/{id}");
            var response = _httpClient.Send(request);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ToDoItem>(content);
                return result;
            }
            return null;
        }

        public void MarkAsComplete(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, $"{BASE_API_ADDRESS}/{id}");
            _httpClient.Send(request);
        }

        public  void UpdateToDoText(Guid id, string updatedText)
        {
            var json = JsonConvert.SerializeObject(updatedText);
            var request = new HttpRequestMessage(HttpMethod.Put, $"{BASE_API_ADDRESS}/{id}");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            _httpClient.Send(request);

        }

        public void DeleteToDoItem(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{BASE_API_ADDRESS}/{id}");
            _httpClient.Send(request);
        }
    }
}
