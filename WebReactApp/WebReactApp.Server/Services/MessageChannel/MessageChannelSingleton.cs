namespace WebReactApp.Server.Services.MessageChannel
{
    public class MessageChannelSingleton
    {
        public delegate void MessageListener(Message message);
        private class Channel
        {
            public const int MAX_RECENT_MESSAGE = 50;
            public string Name { get; set; }
            public Dictionary<string, Audience> audiences { get; set; } = new Dictionary<string, Audience>();
            private Queue<Message> recentMessage { get; set; } = new Queue<Message>();

            public void Write(Message message)
            {
                foreach (var audience in audiences)
                {
                    audience.Value.Send(message);
                }
                if (recentMessage.Count >= MAX_RECENT_MESSAGE)
                {
                    recentMessage.Dequeue();
                }
                recentMessage.Enqueue(message);
            }
            public List<Message> GetRecentMessages()
            {
                List<Message> messages = [.. recentMessage];
                return messages;
            }
        }
        private class Audience
        {
            public string Name { get; set; }
            public MessageListener Listener { get; set; }

            public void Send(Message message)
            {
                this.Listener(message);
            }
        }
        public class Message
        {
            public DateTime CreateTime { get; set; }
            public string Owner { get; set; }
            public string Text { get; set; }
        }

        private Dictionary<string, Channel> _channels { get; set; } = new Dictionary<string, Channel>();
        private readonly ILogger<MessageChannelSingleton> _logger;

        public MessageChannelSingleton(ILogger<MessageChannelSingleton> logger)
        {
            _logger = logger;
        }

        public void ListenChannel(string channelname, string subscribername, MessageListener listener)
        {
            Channel channel;
            Audience audience;
            if (!_channels.TryGetValue(subscribername, out channel))
            {
                channel = new Channel()
                {
                    Name = channelname,
                };
                _channels.Add(channelname, channel);
                Console.WriteLine(string.Format("Added channel: {0}", channelname));
            }
            if (channel.audiences.TryGetValue(subscribername, out _))
            {
                channel.audiences.Remove(subscribername);
            }
            audience = new Audience()
            {
                Listener = listener,
                Name = subscribername,
            };
            channel.audiences.TryAdd(subscribername, audience);
        }
        public void DisconnectChannel(string channelname, string subscribername)
        {
            if (_channels.TryGetValue(channelname, out Channel channel))
            {
                channel.audiences.Remove(subscribername);
            }
        }
        public void SendMessageToChannel(string channelname, Message message)
        {
            if (_channels.TryGetValue(channelname, out Channel channel))
            {
                channel.Write(new Message()
                {
                    CreateTime = DateTime.UtcNow,
                    Owner = message.Owner,
                    Text = message.Text
                });
            }
        }
    }
}
