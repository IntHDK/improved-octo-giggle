using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Net.WebSockets;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using WebReactApp.Server.Services.MessageChannel;

namespace WebReactApp.Server.Controllers.Channel
{
    [Route("api/channel")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly MessageChannelSingleton messageChannelSingleton;
        private readonly ILogger<ChannelController> logger;
        public ChannelController(ILogger<ChannelController> logger, MessageChannelSingleton messageChannelSingleton)
        {
            this.logger = logger;
            this.messageChannelSingleton = messageChannelSingleton;
        }

        //유사 메세지큐 구현을 위한 객체
        //WS 접근한 클라이언트와 통신하는 인터페이스 역할
        public class ChannelWSListenerQueue
        {
            private Guid ID { get; set; } = Guid.NewGuid();
            public string Owner { get; set; }
            public string ChannelName { get; set; }
            public Queue<MessageChannelSingleton.Message> RecvMessages { get; set; } = new Queue<MessageChannelSingleton.Message>();
            public Queue<MessageChannelSingleton.Message> SendMessages { get; set; } = new Queue<MessageChannelSingleton.Message>();
            public WebSocket WS { get; set; }
            public MessageChannelSingleton MSGChannelSingleton { get; set; }
            public bool IsAlive { get; set; } = true;
            private CancellationTokenSource disconnectioncontrolfromsingleton = new CancellationTokenSource();

            public void RecvFromWSClient()
            {
                var buffer = new byte[1024 * 4];
                Task<WebSocketReceiveResult>? receiveTask = null;
                WebSocketReceiveResult? receiveResult = null;
                try
                {
                    receiveTask = WS.ReceiveAsync(new ArraySegment<byte>(buffer), disconnectioncontrolfromsingleton.Token);
                    receiveTask.Wait();
                    receiveResult = receiveTask.Result;
                }
                catch
                {
                    //TODO: Receive 에러 컨트롤
                }

                if (receiveResult != null && receiveTask != null)
                {
                    while (!receiveTask.IsCanceled && !receiveResult.CloseStatus.HasValue && !disconnectioncontrolfromsingleton.Token.IsCancellationRequested)
                    {
                        string msg = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                        RecvMessages.Enqueue(new MessageChannelSingleton.Message()
                        {
                            CreateTime = DateTime.UtcNow,
                            Owner = Owner,
                            Text = msg
                        });

                        receiveTask = WS.ReceiveAsync(
                            new ArraySegment<byte>(buffer), disconnectioncontrolfromsingleton.Token);
                        receiveTask.Wait();
                        receiveResult = receiveTask.Result;
                    }

                    WS.CloseAsync(
                        receiveResult.CloseStatus.Value,
                        receiveResult.CloseStatusDescription,
                        CancellationToken.None).Wait();
                    MSGChannelSingleton.DisconnectChannel(ChannelName, Owner);
                }

                IsAlive = false;

                return;
            }
            public void SendFromWSServer()
            {
                while (WS.State == WebSocketState.Open && !disconnectioncontrolfromsingleton.Token.IsCancellationRequested)
                {
                    if (SendMessages.Count == 0)
                    {
                        continue;
                    }
                    var message = SendMessages.Dequeue();
                    //var messagestrline = string.Format("{0}\t{1}\t{2}", message.CreateTime.ToString(DateTimeFormatInfo.InvariantInfo.RFC1123Pattern), message.Owner, message.Text);
                    message.MQID = this.ID;
                    var messagestrline = JsonSerializer.Serialize(message);
                    var buffer = Encoding.UTF8.GetBytes(messagestrline);
                    var seg = new ArraySegment<byte>(buffer, 0, buffer.Length);
                    WS.SendAsync(
                        seg,
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None).Wait();

                    if (message.ControlType == MessageChannelSingleton.MessageControlType.Disconnection)
                    {
                        disconnectioncontrolfromsingleton.Cancel();
                    }
                }

                IsAlive = false;

                return;
            }

            public void RecvFromChannel(MessageChannelSingleton.Message message)
            {
                SendMessages.Enqueue(message);
            }
            public void SendToChannel()
            {
                while(!disconnectioncontrolfromsingleton.Token.IsCancellationRequested)
                {
                    if (RecvMessages.Count == 0)
                    {
                        continue;
                    }
                    var message = RecvMessages.Dequeue();
                    MSGChannelSingleton.SendMessageToChannel(ChannelName, message);
                }

                IsAlive = false;

                return;
            }
        }

        //Public Channel
        [Authorize]
        [HttpGet("listen/{channelname}")]
        public async Task GetListen(string channelname)
        {
            var claimaccountid = User.Claims.Where(c => c.Type == "AccountID").FirstOrDefault();
            if (claimaccountid == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    ChannelWSListenerQueue channelListenerQueue = new ChannelWSListenerQueue()
                    {
                        Owner = claimaccountid.Value,
                        ChannelName = channelname,
                        MSGChannelSingleton = this.messageChannelSingleton,
                        WS = webSocket
                    };

                    messageChannelSingleton.ListenChannel(channelname, claimaccountid.Value, channelListenerQueue.RecvFromChannel);

                    var task_chansend = Task.Run(() => channelListenerQueue.SendToChannel());
                    var task_wsrecv = Task.Run(() => channelListenerQueue.RecvFromWSClient());
                    var task_wssend = Task.Run(() => channelListenerQueue.SendFromWSServer());

                    task_wssend.Wait();
                    task_wsrecv.Wait();
                    task_chansend.Wait();
                }
                return;
            }
            else
            {
                logger.LogInformation("Not a websocket request");
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
