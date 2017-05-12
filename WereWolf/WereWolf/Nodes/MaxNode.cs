using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf.Nodes
{
    public class MaxNode : PlayerNode
    {
        public MaxNode(string playerName, string charName, InformationSet infoSet) : base(playerName, charName, infoSet)
        {
        }

        public override int PlayGame(RolloutGame game, int alpha, int beta, int depthLimit, string command)
        {
            //int v = Int16.MinValue;
            Dictionary<string, int> possiblePlays;

            if (/*game.reachedDepthLimit(depthLimit) ||*/ game.isGameOver())
            {
                return game.evalGame(charName);
            }

            if (string.IsNullOrEmpty(command))
            {
                //TODO order by gameState
                possiblePlays = InfoSet.getPossibleAccuses();
            }

            //foreach (string command in possiblePlays)
            //{
            //    Move move = new Move(Id, c);
            //    game.ApplyMove(move);
            //    int moveValue = game.GetNextPlayer().PlayGame(game, alpha, beta, depthLimit);

            //    if (moveValue > v)
            //    {
            //        v = moveValue;
            //    }

            //    game.UndoMove(move);

            //    if (v >= beta)
            //    {
            //        return v;
            //    }

            //    if (v > alpha)
            //    {
            //        alpha = v;
            //    }
            //}

            //return v;
            return 0;
        }
    }
}
