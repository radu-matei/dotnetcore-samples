using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace console_client
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
                 .WithConsoleLogger()
                 .Build();

            await _connection.StartAsync();
        }

        public static async Task DisposeAsync()
        {
            await _connection.DisposeAsync();
        }
    }
}
