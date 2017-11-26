using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace console_streaming
{
    class Program
    {
        private static HubConnection _connection;
        static void Main(string[] args)
        {

            StartConnectionAsync();
            _connection.On("streamStarted", () =>
            {
                StartStreaming();
            });

            Console.ReadLine();
            DisposeAsync();
        }


        public async static Task StartStreaming()
        {
            var channel = await _connection.StreamAsync<string>("StartStreaming", CancellationToken.None);
            while (await channel.WaitToReadAsync())
            {
                while (channel.TryRead(out string message))
                {
                    Console.WriteLine($"Message received: {message}");
                }
            }   
        }

        public static async Task StartConnectionAsync()
        {
            _connection = new HubConnectionBuilder()
                 .WithUrl("http://localhost:5000/streaming")
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
