using ConsoleModule.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleModule
{
    //플레이어
    public class Player
    {
    }

    //영역
    public class Zone
    {

    }
    public class GameObject
    {
        public GameObjectMetaInfo ObjectMetaInfo { get; set; }

        public enum GameObjectType
        {
            Card, // 카드, (카드랑 동등취급되는) 토큰, 복사본
            Effect, // 적용 지속중인 효과 객체
            SubObject, // 카운터, 지시자
        }
        public GameObjectType ObjectType { get; set; }
        public bool IsVolatile { get; set; } // true: 현재 영역 이동시 사라지는 취급
        

    }
}
