using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace console_client_binary
{
    class Program
    {
        private static HubConnection _connection;
        static void Main(string[] args)
        {

            StartConnectionAsync();
            _connection.On<string, string>("broadcastMessage", (name, message) =>
            {
                Console.WriteLine($"{name} said: {message}");
            });

            Console.ReadLine();
            DisposeAsync();
        }


        public static async Task StartConnectionAsync()
        {
            _connection = new HubConnectionBuilder()
                 .WithUrl("http://localhost:5000/chat")
                 .ConfigureLogging(logging => {
                     logging.AddConsole();
                 })
                 .AddMessagePackProtocol()
                 .Build();

            await _connection.StartAsync();
        }

        public static async Task DisposeAsync()
        {
            await _connection.DisposeAsync();
        }
    }
}
