using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace SignalR.Samples.ConsoleClient
{
    class Program
    {
        private static HubConnection _connection;
        public static async Task Main(string[] args)
        {

            await StartConnection();
            _connection.On<string, string>("broadcastMessage", (name, message) =>
            {
                Console.WriteLine($"{name} said: {message}");
            });

            _connection.On("streamStarted", async () =>
            {
                await StartStreaming();
            });

            Console.ReadLine();
            await Dispose();
        }

        public async static Task StartStreaming()
        {
            var channel = await _connection.StreamAsChannelAsync<string>("StartStreaming", CancellationToken.None);
            while (await channel.WaitToReadAsync())
            {
                while (channel.TryRead(out string message))
                {
                    Console.WriteLine($"Message received: {message}");
                }
            }
        }

        public static async Task StartConnection()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/demo")
                //to enable message pack for the .NET client, uncomment the following line
                //.AddMessagePackProtocol()
                .Build();

            await _connection.StartAsync();
        }

        public static async Task Dispose()
        {
            await _connection.DisposeAsync();
        }
    }
}
