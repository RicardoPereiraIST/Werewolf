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
        public MaxNode(string playerName, string charName, InformationSet infoSet, bool playerdead) : base(playerName, charName, infoSet, playerdead)
        {
        }

        public override PlayerNode Copy()
        {
            return new MaxNode(playerName, charName, InfoSet, playerDead);
        }

        public override int PlayGame(RolloutGame game, int alpha, int beta, int depthLimit, string command)
        {
            //int v = Int16.MinValue;
            List<string> possiblePlays = new List<string>() { "" };
            int v = Int16.MinValue;

            if (game.reachedDepthLimit(depthLimit) ||game.isGameOver())
            {
                return game.evalGame();
            }

            if (string.IsNullOrEmpty(command))
            {
                switch (game.getGameState())
                {
                    case GameStates.ACCUSE:
                        possiblePlays = game.getPossibleAccuses(playerName);
                        break;
                    case GameStates.KILL:
                        if (character.canKill()) possiblePlays = game.getPossibleKills(playerName);
                        break;
                    case GameStates.HEAL:
                        if (character.canHeal()) possiblePlays = game.getPossibleHeals(playerName);
                        break;
                    default:
                        possiblePlays.Add("pass");
                        break;
                }
            }
            else
            {
                possiblePlays[0] = command;
            }

            foreach (string playCommand in possiblePlays)
            {
                RolloutGame playGame = game.Copy();

                playGame.applyMove(playCommand);
                int moveValue = playGame.getNextPlayer().PlayGame(playGame, alpha, beta, depthLimit);

                if (moveValue > v)
                {
                    v = moveValue;
                }

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
