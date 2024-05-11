using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleModule.Meta
{
    public enum Textblock_Type
    {
        Text,
        LevelStatus // (헤드쿼터, 컴패니언) 레벨업 구간별 텍스트
    }
    public class Textblock
    {
        public int ID;
        public Textblock_Type Type;
        public string Text;
        public Dictionary<string, int> Parameters_Numbers;
        public Dictionary<string, int> Parameters_IDs;
    }
}
