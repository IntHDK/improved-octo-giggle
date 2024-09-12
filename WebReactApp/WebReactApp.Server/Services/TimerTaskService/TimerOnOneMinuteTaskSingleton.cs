using WebReactApp.Server.ModelObjects;
using WebReactApp.Server.ModelObjects.Identity;
using WebReactApp.Server.Services.ItemService;
using WebReactApp.Server.Services.MessageChannel;

namespace WebReactApp.Server.Services.TimerTaskService
{
    //1분마다 특정 채널에 메세지, 지정된 AccountPost (우편) 1개 발생
    public class TimerOnOneMinuteTaskSingleton : IDisposable
    {
        private readonly MessageChannelSingleton messageChannelSingleton;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private bool active = false;
        public TimerOnOneMinuteTaskSingleton(
            MessageChannelSingleton messageChannelSingleton, IServiceScopeFactory serviceScopeFactory)
        {
            this.messageChannelSingleton = messageChannelSingleton;
            this.serviceScopeFactory = serviceScopeFactory;
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
                    if (aftertimestamp - prevtimestamp >= TimeSpan.FromMinutes(1)) //1분 간격
                    {
                        Console.WriteLine("Minute event triggered!");
                        messageChannelSingleton.SendMessageToChannel("notice_minutely", new MessageChannelSingleton.Message()
                        {
                            ControlType = MessageChannelSingleton.MessageControlType.PlainMessage,
                            CreateTime = DateTime.Now,
                            Owner = "",
                            Text = DateTime.Now.ToString()
                        });
                        Task.Run(() =>
                        {
                            using (var scope = serviceScopeFactory.CreateScope())
                            {
                                var itemman = scope.ServiceProvider.GetRequiredService<ItemManager>();
                                if (itemman != null)
                                {
                                    itemman.AddPostByAccountCondition(new AccountPost()
                                    {
                                        Context = DateTime.Now.ToString() + " Post",
                                        AccountPostenclosure = [
                                        new AccountPostEnclosure(){
                                            ID = Guid.NewGuid(),
                                            CreatedTime = DateTime.Now,
                                            ItemMetaName = "Test01",
                                            Quantity = 1,
                                            Parameters = [],
                                            ExpireAt = DateTime.MaxValue,
                                            Type = AccountItemType.Item,
                                        },
                                        new AccountPostEnclosure(){
                                            ID = Guid.NewGuid(),
                                            CreatedTime= DateTime.Now,
                                            ItemMetaName = "",
                                            Quantity = 100,
                                            Type = AccountItemType.Currency_Point,
                                            Parameters = [],
                                            ExpireAt = DateTime.MaxValue,
                                        }
                                        ],
                                        ExpireAt = DateTime.Now.AddMinutes(30),
                                    }, (a => true));
                                }
                            }
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
