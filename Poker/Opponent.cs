using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Opponent : Player
    {
        // Have some behavioral profile
        public Opponent(string name, Game game) : base(name, game)
        {
            this.game = game;
        }

        public override void MakeDecision(int maximumBet)
        {
            Status = (Status)(((int)Status + 1) % (Enum.GetNames(typeof(Status)).Length));
            // Do opponent profile logic here
            // Wait some time before continuing
        }


    }
}
