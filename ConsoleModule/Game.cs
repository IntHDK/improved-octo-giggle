using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleModule
{
    public partial class Game
    {
        public enum GamePhase
        {
            None,
            Prepare,
            Ongoing,
            Done
        }

        public GamePhase Phase {
            get { return _phase; }
        }
        private GamePhase _phase;

    }

}
