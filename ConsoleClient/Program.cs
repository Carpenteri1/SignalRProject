// See https://aka.ms/new-console-template for more information
using ConsoleClient.Data;
using ConsoleClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Immutable;
using System.Net.Http.Json;
using System.Reflection;

const string consoleClient = nameof(consoleClient);
const string hubUser = nameof(hubUser);
const string webAppClient = nameof(webAppClient);

string lastSentMessage = string.Empty;
ImmutableList<string> messages = ImmutableList<string>.Empty;
using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("https://localhost:7143");
var response = await httpClient.GetAsync("/people");
var people = await response.Content.ReadFromJsonAsync<Person[]>();

var hubConnection = BuildConnection();
SetupSub(hubConnection);
await Connect(hubConnection);
await Run();

async Task Run()
{
    if (hubConnection.State == HubConnectionState.Connecting ||
        hubConnection.State == HubConnectionState.Disconnected)
        await ReConnect(hubConnection);

    while (true)
    {
        if (lastSentMessage != string.Empty)
        {
            Console.ReadLine();
            Console.Clear();
            lastSentMessage = string.Empty;
        }
        Console.ForegroundColor = ConsoleColor.White;
        await SendMessage(hubConnection);
    }
}

async Task<bool> PublishMessagesToConsole()
{
    await Task.Run(() =>
    {
        foreach (var message in messages)
        {
            if (message.StartsWith(ResourceStrings.MyMessage))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
            }
            if (message.StartsWith(ResourceStrings.FromHub))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{message}");
            }
            if (message.Contains(ResourceStrings.LastMessageSent))
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{message}\n");
            }
            if (message.Contains(ResourceStrings.RecievedFromClient))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{message}\n");
            }
        }

    });
    return true;

}

void SetupSub(HubConnection hubConnection)
{
    hubConnection.On(ResourceStrings.Receive, async Task (string user, string message, string fromHub) =>
    {
        Console.Clear();
        await ReceiveMessage(user, message, fromHub);
    });

}

async Task ReceiveMessage(string user, string message, string fromHub)
{
    Console.Clear();
    if(!messages.Contains($"{ResourceStrings.LastMessageSent}{message}")) messages = messages.Add($"{ResourceStrings.LastMessageSent}{message}");
    if (!messages.Contains($"{ResourceStrings.MyMessage}{message}")) messages = messages.Add($"{ResourceStrings.MyMessage}{message}");
    if (!messages.Contains($"{ResourceStrings.FromHub}{fromHub}\n")) messages = messages.Add($"{ResourceStrings.FromHub}{fromHub}\n");
    if (!messages.Contains($"{ResourceStrings.RecievedFromClient}{user}")) messages = messages.Add($"{ResourceStrings.RecievedFromClient}{user}");
    await PublishMessagesToConsole();
    messages = ImmutableList<string>.Empty;
}
async Task SendMessage(HubConnection hubConnection)
{
    Console.Write(ResourceStrings.EnterMessage);
    lastSentMessage = Console.ReadLine();
    Console.Clear();
    await hubConnection.InvokeAsync(ResourceStrings.SendMessage, consoleClient, lastSentMessage);
}

async Task Connect(HubConnection hubConnection)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write(ResourceStrings.Connecting);
    await hubConnection.StartAsync();
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.White;
}
async Task ReConnect(HubConnection hubConnection)
{
    await hubConnection.StopAsync();
    System.Diagnostics
        .Process
        .Start(Assembly
        .GetExecutingAssembly()
        .Location);
}
HubConnection BuildConnection() =>
    new HubConnectionBuilder()
            .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(5) })
            .WithUrl("https://localhost:7143/personhub")
            .Build();