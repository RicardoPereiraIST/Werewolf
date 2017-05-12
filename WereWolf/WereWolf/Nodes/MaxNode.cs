using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WereWolf.General;

namespace WereWolf.Nodes
{
    public class MaxNode : PlayerNode
    {
        public MaxNode(string playerName, string charName, InformationSet infoSet) : base(playerName, charName, infoSet)
        {
        }

        public override PlayerNode Copy()
        {
            return new MaxNode(playerName, charName, InfoSet);
        }

        public override int PlayGame(RolloutGame game, int alpha, int beta, int depthLimit, string command)
        {
            //int v = Int16.MinValue;
            List<string> possiblePlays = new List<string>();
            int v = Int16.MinValue;

            if (/*game.reachedDepthLimit(depthLimit) ||*/ game.isGameOver())
            {
                return game.evalGame(charName);
            }

            if (string.IsNullOrEmpty(command))
            {
                switch(game.getGameState())
                {
                    case GameStates.ACCUSE:
                        possiblePlays = game.getPossibleAccuses(playerName);
                        break;
                    case GameStates.KILL:
                        possiblePlays = game.getPossibleKills(playerName);
                        break;
                    case GameStates.HEAL:
                        possiblePlays = game.getPossibleHeals(playerName);
                        break;
                }
            }
            else
            {
                possiblePlays.Add(command);
            }

            foreach (string playCommand in possiblePlays)
            {
                game.applyMove(playCommand);
                
                int moveValue = game.getNextPlayer().PlayGame(game, alpha, beta, depthLimit);

                if (moveValue > v)
                {
                    v = moveValue;
                }

                game.UndoMove(playCommand);

                if (v >= beta)
                {
                    return v;
                }

                if (v > alpha)
                {
                    alpha = v;
                }
            }

            return v;
        }
    }
}
