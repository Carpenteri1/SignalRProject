using WebClientApp.Models;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;

namespace WebClientApp.ApiServices
{
    public class PersonApiService : IPersonApiService
    {
        private readonly HttpClient httpClient;
        private HubConnection hubConnection;
        public PersonApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;

            hubConnection = new HubConnectionBuilder()
                 .WithUrl("https://localhost:7143/personhub")
                 .WithAutomaticReconnect()
                 .Build();
            
            hubConnection.StartAsync();
        }
        public async Task<Person[]> GetAll()
        {
            var response = await httpClient.GetAsync("people");
            response.EnsureSuccessStatusCode();
            var people = await response.Content.ReadFromJsonAsync<IEnumerable<Person>>();

            if (people == null)
                return Enumerable.Empty<Person>().ToArray();
            return people.ToArray();

        }

        public async Task<Person> GetOne(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"/person/{id}");
                return JsonSerializer.Deserialize<Person>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                
            }
            return null;
        }
        
        public async Task<Person?> Add(Person person)
        {
            var jsonContent = JsonContent.Create(person);
            var response = await httpClient.PostAsync("person", jsonContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Person>();
        }

        public async void GetMessage(string message)
        {
            try
            {
                hubConnection.On<string>("SendMessageToAll", (theMessage) =>
                {
                    theMessage = message;
                });
             
            }
            catch (Exception e)
            {

            }

        }
    }
}
