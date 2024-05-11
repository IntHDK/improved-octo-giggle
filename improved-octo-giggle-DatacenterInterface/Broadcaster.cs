using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace improved_octo_giggle_DatacenterInterface
{
    public enum DataSignalType
    {
        Created, //리소스 생성
        Modified, //리소스 변경
        Deleted //리소스 제거
    }
    public enum ResourceType
    {
        Account,
        AccountLoginMethod_IDPW,
        AccountLoginMethod_Google,
        AccountLoginMethod_Apple,
        SessionInfo,
    }
    public delegate bool Callback(DataSignalType dataSignal);

    public interface DatacenterBroadcaster // 실시간 변경 추적용 인터페이스
    {
        public string Listen(ResourceType ResourceType, string ResourceID, Callback callback); // Result: 관리용 Listening ID
        public void Dismiss(string ListeningID); // 더 이상 해당 Listening ID에 대해 수신하지 않음
    }
}
