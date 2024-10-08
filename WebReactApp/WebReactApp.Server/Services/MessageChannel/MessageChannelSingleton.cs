﻿namespace WebReactApp.Server.Services.MessageChannel
{
    public class MessageChannelSingleton
    {
        //서버에서 직접 클라이언트로 메세지 전송해야 할 때 콜백용
        public delegate void MessageListener(Message message);

        /*
         * 
         * 기본형 채널
         * 중복 접속 불가 (Audience의 Name으로 판단)
         * 전송한 메세지는 모든 참여 Audience로 전송
         * 
         * 만약 일방적으로 통신하는 채널이거나 해야한다면 컨트롤러 쪽에서 리스닝 제거 등 조치하고 사용할것
         */
        private interface Channel
        {
            public string Name { get; set; }
            public Dictionary<string, Audience> audiences { get; set; }
            public void Write(Message message);
            public List<Message> GetRecentMessages();
            public void AddAudience(Audience audience);
        }
        private class PublicChannel : Channel
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
            public void AddAudience(Audience audience)
            {
                if (audiences.TryGetValue(audience.Name, out Audience prevaudience))
                {
                    //채널에 같은 audience로 들어왔으면 기존 연결은 끊음
                    prevaudience.Send(new Message
                    {
                        ControlType = MessageControlType.Disconnection,
                        CreateTime = DateTime.UtcNow,
                        Owner = "",
                        Text = ""
                    });

                    audiences.Remove(audience.Name);
                }
                audiences.TryAdd(audience.Name, audience);
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
        public enum MessageControlType
        {
            None,
            PlainMessage,
            Disconnection
        }
        public class Message
        {
            public Guid MQID { get; set; } //TODO: 임시용이므로 차후 제거
            public MessageControlType ControlType { get; set; }
            public DateTime CreateTime { get; set; }
            public string Owner { get; set; }
            public string Text { get; set; }
        }

        private Dictionary<string, PublicChannel> _channels { get; set; } = new Dictionary<string, PublicChannel>();
        private readonly ILogger<MessageChannelSingleton> _logger;

        public MessageChannelSingleton(ILogger<MessageChannelSingleton> logger)
        {
            _logger = logger;
        }

        public void ListenChannel(string channelname, string subscribername, MessageListener listener)
        {
            PublicChannel channel;
            Audience audience;
            if (!_channels.TryGetValue(channelname, out channel))
            {
                channel = new PublicChannel()
                {
                    Name = channelname,
                };
                _channels.Add(channelname, channel);
                Console.WriteLine(string.Format("Added channel: {0}", channelname));
            }
            audience = new Audience()
            {
                Listener = listener,
                Name = subscribername,
            };
            channel.AddAudience(audience);
        }
        public void DisconnectChannel(string channelname, string subscribername)
        {
            if (_channels.TryGetValue(channelname, out PublicChannel channel))
            {
                channel.audiences.Remove(subscribername);
            }
        }
        public void SendMessageToChannel(string channelname, Message message, MessageControlType messageControlType = MessageControlType.PlainMessage)
        {
            if (_channels.TryGetValue(channelname, out PublicChannel channel))
            {
                channel.Write(new Message()
                {
                    ControlType = messageControlType,
                    CreateTime = DateTime.UtcNow,
                    Owner = message.Owner,
                    Text = message.Text
                });
            }
        }
    }
}
