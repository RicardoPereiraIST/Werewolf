using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf.Nodes
{
    public class RuleBasedNode : PlayerNode
    {
        public RuleBasedNode(string playerName, string charName, InformationSet infoSet) : base(playerName, charName, infoSet)
        {
        }

        public override int PlayGame(RolloutGame game, int alpha, int beta, int depthLimit, string command)
        {
            //int v = Int16.MinValue;
            int v = Int16.MinValue;
            string playCommand = command;

            if (/*game.reachedDepthLimit(depthLimit) ||*/ game.isGameOver() || playerDead)
            {
                return game.evalGame(charName);
            }

            if (string.IsNullOrEmpty(command))
            {
                //TODO order by gameState
                playCommand = InfoSet.ruledBasedAccuse();
            }

            game.applyMove(playCommand);

            int moveValue = game.getNextPlayer().PlayGame(game, alpha, beta, depthLimit);

            game.UndoMove(playCommand);

            return v;
        }

        public override PlayerNode Copy()
        {
            return new RuleBasedNode(playerName, charName, InfoSet);
        }
    }
}
