using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WereWolf.General;

namespace WereWolf.Nodes
{
    public class RuleBasedNode : PlayerNode
    {
        public RuleBasedNode(string playerName, string charName, InformationSet infoSet) : base(playerName, charName, infoSet)
        {
        }

        public RuleBasedNode(string playerName, string charName, InformationSet infoSet, bool playerdead) : base(playerName, charName, infoSet, playerdead)
        {
        }

        public override int PlayGame(RolloutGame game, int alpha, int beta, int depthLimit, string command)
        {
            string playCommand = command;

            if (game.reachedDepthLimit(depthLimit) || game.isGameOver())
            {
                return game.evalGame();
            }

            if (string.IsNullOrEmpty(command))
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                List<string> possiblePlays;
                switch (game.getGameState())
                {
                    case GameStates.ACCUSE:
                        possiblePlays = game.getPossibleAccuses(playerName);
                        playCommand = possiblePlays[rnd.Next(possiblePlays.Count)];
                        break;
                    case GameStates.KILL:
                        if (character.canKill())
                        {
                            possiblePlays = game.getPossibleAccuses(playerName);
                            playCommand = possiblePlays[rnd.Next(possiblePlays.Count)];
                        }
                        break;
                    case GameStates.HEAL:
                        if (character.canHeal())
                         {
                            possiblePlays = game.getPossibleHeals(playerName);
                            playCommand = possiblePlays[rnd.Next(possiblePlays.Count)];
                        }
                        break;
                    case GameStates.TALK:
                        if (character.canHeal())
                        {
                            possiblePlays = InfoSet.getPossibleTalks().Select(x => x.Key).ToList();
                            playCommand = possiblePlays[rnd.Next(possiblePlays.Count)];
                        }
                        break;
                    case GameStates.QUESTION:
                        possiblePlays = game.getPossibleQuestions(playerName);
                        playCommand = possiblePlays[rnd.Next(possiblePlays.Count)];
                        break;
                    default:
                        playCommand = "pass";
                        break;
                }
            }

            RolloutGame playGame = game.Copy();

            playGame.applyMove(playCommand);
            int moveValue = playGame.getNextPlayer().PlayGame(playGame, alpha, beta, depthLimit);

            return moveValue;
        }

        public override PlayerNode Copy()
        {
            return new RuleBasedNode(playerName, charName, InfoSet, playerDead);
        }
    }
}
