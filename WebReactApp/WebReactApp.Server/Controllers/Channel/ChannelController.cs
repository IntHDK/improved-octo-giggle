using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Net.WebSockets;
using System.Runtime.Serialization;
using System.Text;
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

        public class ChannelWSListenerQueue
        {
            public string Owner { get; set; }
            public string ChannelName { get; set; }
            public Queue<MessageChannelSingleton.Message> RecvMessages { get; set; } = new Queue<MessageChannelSingleton.Message>();
            public Queue<MessageChannelSingleton.Message> SendMessages { get; set; } = new Queue<MessageChannelSingleton.Message>();
            public WebSocket WS { get; set; }
            public MessageChannelSingleton MSGChannelSingleton { get; set; }
            public bool IsAlive { get; set; } = true;

            public void RecvFromWSClient()
            {
                var buffer = new byte[1024 * 4];
                var receiveTask = WS.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                receiveTask.Wait();
                var receiveResult = receiveTask.Result;

                while (!receiveResult.CloseStatus.HasValue)
                {
                    string msg = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                    RecvMessages.Enqueue(new MessageChannelSingleton.Message()
                    {
                        CreateTime = DateTime.UtcNow,
                        Owner = Owner,
                        Text = msg
                    });

                    receiveTask = WS.ReceiveAsync(
                        new ArraySegment<byte>(buffer), CancellationToken.None);
                    receiveTask.Wait();
                    receiveResult = receiveTask.Result;
                }

                WS.CloseAsync(
                    receiveResult.CloseStatus.Value,
                    receiveResult.CloseStatusDescription,
                    CancellationToken.None).Wait();

                IsAlive = false;

                return;
            }
            public void SendFromWSServer()
            {
                while (WS.State == WebSocketState.Open)
                {
                    if (SendMessages.Count == 0)
                    {
                        continue;
                    }
                    var message = SendMessages.Dequeue();
                    var messagestrline = string.Format("{0}\t{1}\t{2}", message.CreateTime.ToString(DateTimeFormatInfo.InvariantInfo.RFC1123Pattern), message.Owner, message.Text);
                    var buffer = Encoding.UTF8.GetBytes(messagestrline);
                    var seg = new ArraySegment<byte>(buffer, 0, buffer.Length);
                    WS.SendAsync(
                        seg,
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None).Wait();
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
                while(IsAlive)
                {
                    if (RecvMessages.Count == 0)
                    {
                        continue;
                    }
                    var message = RecvMessages.Dequeue();
                    MSGChannelSingleton.SendMessageToChannel(ChannelName, message);
                }
                return;
            }
        }
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
