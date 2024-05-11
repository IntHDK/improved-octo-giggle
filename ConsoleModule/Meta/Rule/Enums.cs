using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleModule.Meta.Rule
{
    public enum Card_CardFace_Type
    {
        Front, // 앞면
        FaceDown // 덮음
    }
    public enum Card_Type
    {
        None,
        Headquater, // 게임 개시시에 놓는 영웅
        Companion, // 덱 구성에 사용, 
        Asset,
        Action,
    }
    public enum Card_SuperType
    {
        None,
        Legendary, // 같은 표기명에 대해서 플레이어 당 하나만 필드에 둘 수 있음
        Illusion, // 무한대로 덱 구성에 사용 가능
        Token,
    }
    public enum Card_SubType
    {

    }
    public enum Faction
    {
        None,
        Red,
        White,
        Green,
        Black,
    }
}
