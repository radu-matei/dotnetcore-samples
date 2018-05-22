using System;
using System.Reactive.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace streaming
{
    public class StreamingHub : Hub
    {

        public void SendStreamInit()
        {
            Clients.All.SendAsync("streamStarted");
        }

        public ChannelReader<string> StartStreaming()
        {
            var channel = Channel.CreateUnbounded<string>();
            _ = WriteToChannel(channel);
            return channel.Reader;

            async Task WriteToChannel(ChannelWriter<string> writer)
            {
                for (int i = 0; i < 10; i++)
                {
                    await writer.WriteAsync($"sending... {i}");
                    await Task.Delay(1000);
                }
                writer.Complete();
            }
        }
    }
}