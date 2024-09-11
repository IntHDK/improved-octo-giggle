using WebReactApp.Server.Services.MessageChannel;

namespace WebReactApp.Server.Services.TimerTaskService
{
    //1분마다 특정 채널에 메세지 쏨
    public class TimerOnOneMinuteTaskSingleton : IDisposable
    {
        private readonly MessageChannelSingleton messageChannelSingleton;
        private bool active = false;
        public TimerOnOneMinuteTaskSingleton(MessageChannelSingleton messageChannelSingleton)
        {
            this.messageChannelSingleton = messageChannelSingleton;
        }
        public void Start()
        {
            active = true;
            Task.Run(() =>
            {
                var prevtimestamp = DateTime.Now;
                while (active)
                {
                    var aftertimestamp = DateTime.Now;
                    if (prevtimestamp.Minute != aftertimestamp.Minute) //분침이 바뀜
                    {
                        messageChannelSingleton.SendMessageToChannel("notice_minutely", new MessageChannelSingleton.Message()
                        {
                            ControlType = MessageChannelSingleton.MessageControlType.PlainMessage,
                            CreateTime = DateTime.Now,
                            Owner = "",
                            Text = DateTime.Now.ToString()
                        });
                    }
                    prevtimestamp = aftertimestamp;
                    Task.Delay(TimeSpan.FromMilliseconds(50)).Wait();
                }
            });
        }
        public void Stop()
        {
            active = false;
        }
        public void Dispose()
        {
            Stop();
        }
    }
}
