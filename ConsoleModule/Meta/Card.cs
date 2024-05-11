using ConsoleModule.Meta.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleModule.Meta
{
    public class Card : GameObjectMetaInfo
    {
        public bool IsCollectable;
        public Dictionary<Card_CardFace_Type, CardFace> Faces;
    }
    public class CardFace
    {
        public int ID;
        public string RuleName; // 이름, TODO: 이 부분은 실제 명칭 대신 코드로

        public HashSet<Card_Type> Types;
        public HashSet<Card_SuperType> SuperTypes;
        public HashSet<Card_SubType> SubTypes;
        public HashSet<Faction> Faction;

        public int Cost; // 플레이 비용
        public int Earning; // (헤드쿼터, 컴패니언) 플레이 중 매 턴 발생할 수 있는 코스트값
        public int Link; // (헤드쿼터, 컴패니언) 
        public int AP, HP;

        public List<Textblock> Textblocks;
    }
}
