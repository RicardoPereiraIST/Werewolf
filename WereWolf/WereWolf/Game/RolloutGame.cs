using System;
using System.Collections.Generic;
using WereWolf.General;
using System.Linq;
using System.Text;
using WereWolf.Nodes;

namespace WereWolf
{
    public class RolloutGame
    {
        public const int WEREWOLF_NUMBER = 2;
        public const int SEER_NUMBER = 1;
        public const int DOCTOR_NUMBER = 1;
        public const int VILLAGER_NUMBER = 2;

        private List<PlayerNode> players;
        private Dictionary<string, int> roundVotes;
        private GameStates gameState;
        private int depth;
        private int playerNumber = 0;
        private string healedPlayer;

        public RolloutGame(List<PlayerNode> players, GameStates gameState)
        {
            this.players = new List<PlayerNode>(players);
            this.gameState = gameState;
            roundVotes = new Dictionary<string, int>();
            depth = 0;
            healedPlayer = string.Empty;
        }

        public int sampleGame(string command)
        {
            PlayerNode player = players[0];
            int gameUtility = player.PlayGame(this, Int16.MinValue, Int16.MaxValue, Int16.MaxValue, command);
            return gameUtility;
        }

        public PlayerNode getNextPlayer()
        {
            return players[playerNumber];
        }

        public void UndoMove(string command)
        {
            playerNumber = playerNumber == 0 ? players.Count - 1 : playerNumber--;
        }

        private bool werewolfesWin()
        {
            //All players that are werewolfs are dead OR all players that are not werewolfs are dead.
            return players.Where(p => p.charName.Equals("Werewolf") && !p.playerDead).Count() >= players.Where(p => !p.charName.Equals("Werewolf") && !p.playerDead).Count();
        }

        public int evalGame(string team)
        {
            int result = 0;
            if (team.Equals("Werewolf"))
            {
                result = players.Where(p => p.charName.Equals("Seer") && p.playerDead).Count() * 4;
                result += players.Where(p => p.charName.Equals("Doctor") && p.playerDead).Count() * 3;
                result += players.Where(p => p.charName.Equals("Villager") && p.playerDead).Count() * 2;
                result += players.Where(p => p.charName.Equals("Werewolf") && !p.playerDead).Count();
                result = -players.Where(p => p.charName.Equals("Seer") && !p.playerDead).Count() * 4;
                result -= players.Where(p => p.charName.Equals("Doctor")   && !p.playerDead).Count() * 3;
                result -= players.Where(p => p.charName.Equals("Villager") && !p.playerDead).Count();
                result -= players.Where(p => p.charName.Equals("Werewolf") && p.playerDead).Count() * 2;
            }

            if (!team.Equals("Werewolf"))
            {
                result = players.Where(p => p.charName.Equals("Seer") && !p.playerDead).Count() * 3;
                result += players.Where(p => p.charName.Equals("Doctor") && !p.playerDead).Count() * 2;
                result += players.Where(p => p.charName.Equals("Villager") && !p.playerDead).Count();
                result += players.Where(p => p.charName.Equals("Werewolf") && p.playerDead).Count() * 5;
                result -= players.Where(p => p.charName.Equals("Seer") && p.playerDead).Count() * 3;
                result -= players.Where(p => p.charName.Equals("Doctor") && p.playerDead).Count() * 2;
                result -= players.Where(p => p.charName.Equals("Villager") && p.playerDead).Count();
                result -= players.Where(p => p.charName.Equals("Werewolf") && !p.playerDead).Count() * 5;
            }
            return result;
        }


        public void applyMove(string command)
        {
            StringBuilder roundSummary = new StringBuilder();

            if (!string.IsNullOrEmpty(command))
            {
                roundSummary.AppendLine(runInstructions(command, players[0]));
            }

            if (gameState == GameStates.ACCUSE)
            {
                roundSummary.AppendLine(accuseLogic());
                roundVotes.Clear();
            }

            if (gameState == GameStates.QUESTION)
            {
                killLogic();
                PlayerNode mostVotedPlayer = getMostVotedPlayer();
                //ONLY for testing purposes this shouldnt happen with intelligent AI
                if (mostVotedPlayer != null)
                {
                    if (healedPlayer == mostVotedPlayer.playerName)
                    {
                        mostVotedPlayer.playerDead = false;
                    }
                }
                healedPlayer = string.Empty;
                roundVotes.Clear();
            }

            gameState = nextGameState();
            depth++;
            playerNumber = (playerNumber+1) % players.Count;
        }

        public List<String> getPossibleAccuses(string playerName)
        {
            return players.Select(x => x).Where(x => x.playerName != playerName && !x.playerDead).Select(x => x.playerName).ToList();       
        }

        public bool isGameOver()
        {
            //All players that are werewolfs are dead OR all players that are not werewolfs are dead.
            bool allWereWolfsDead = players.All(p => p.charName.Equals("Werewolf") && p.playerDead || !p.charName.Equals("Werewolf"));
            bool allNonWerewolfsDead = players.Where(p => p.charName.Equals("Werewolf") && !p.playerDead).Count() >= players.Where(p => !p.charName.Equals("Werewolf") && !p.playerDead).Count();
            return allWereWolfsDead || allNonWerewolfsDead;
        }

        private string accuseLogic()
        {
            string result = "No player was accused this round";

            //Accuse Player Logic
            PlayerNode accusedPlayer = getMostVotedPlayer();

            if (accusedPlayer != null)
            {
                result = string.Format("Player {0} was accused and is now dead", accusedPlayer.playerName);
                accusedPlayer.playerDead = true;
            }

            return result;
        }

        private string killLogic()
        {
            string result = "No player was killed this round";

            //Kill Player Logic
            PlayerNode killedPlayer = getMostVotedPlayer();

            //This should never happen, just for testing sake
            if (killedPlayer != null)
            {
                result = string.Format("A player has been chosen to be killed by the werewolfes");
                killedPlayer.playerDead = true;
            }

            return result;
        }


        private string runInstructions(string instructions, PlayerNode player)
        {
            String[] instructionList = instructions.Split(' ');
            string instruction = instructionList[0];

            switch (instruction)
            {
                case "heal":
                    return HealPlayer(instructionList[1]);

                case "question":
                    QuestionPlayer(instructionList[1], player);
                    return string.Format("questions {0}", instructionList[1]);

                case "kill":
                    VotePlayer(instructionList[1]);
                    return string.Empty;

                case "talk":
                    return string.Format("Player {0} says {1}", player.playerName, string.Join(" ", instructionList.Where(s => !s.Equals("talk"))));

                case "accuse":
                    VotePlayer(instructionList[1]);
                    return string.Format("Player {0} accuses {1}", player.playerName, instructionList[1]);

                default:
                    return string.Format("Player {0} passes", player.playerName);
            }
        }

        private void VotePlayer(string playerName)
        {
            int playerVotes = 1;
            if (roundVotes.ContainsKey(playerName))
            {
                roundVotes.TryGetValue(playerName, out playerVotes);
                ++playerVotes;
                roundVotes[playerName] = playerVotes;
            }
            else
            {
                roundVotes.Add(playerName, playerVotes);
            }
        }

        private void QuestionPlayer(string playerName, PlayerNode player)
        {
            PlayerNode playerQuestioned = getPlayerByName(playerName);
            //player.seerAnswer(playerName, playerQuestioned.charName);
        }

        private string HealPlayer(string playerName)
        {
            string result = "No player was healed this round\n";

            //Kill Player Logic
            string killedPlayerName = getMostVotedPlayerName();

            if (playerName.Equals(killedPlayerName))
            {
                PlayerNode killedPlayer = getPlayerByName(killedPlayerName);

                //This should never happen, just for testing sake
                if (killedPlayer != null)
                {
                    result = string.Format("A player has been choosed to be healed.\n");
                    healedPlayer = playerName;
                }
            }
            return result;
        }

        private string getMostVotedPlayerName()
        {
            return roundVotes.FirstOrDefault(x => x.Value == roundVotes.Values.Max()).Key;
        }

        private PlayerNode getMostVotedPlayer()
        {
            string votedPlayerName = roundVotes.FirstOrDefault(x => x.Value == roundVotes.Values.Max()).Key;
            return players.FirstOrDefault(p => p.playerName.Equals(votedPlayerName));
        }

        private PlayerNode getPlayerByName(string playerName)
        {
            return players.FirstOrDefault(p => p.playerName.Equals(playerName));
        }

        private GameStates nextGameState()
        {
            if (playerNumber == players.Count - 1)
            {
                switch (gameState)
                {
                    case GameStates.TALK:
                        return GameStates.ACCUSE;

                    case GameStates.ACCUSE:
                        return GameStates.KILL;

                    case GameStates.KILL:
                        return GameStates.HEAL;

                    case GameStates.HEAL:
                        return GameStates.QUESTION;

                    case GameStates.QUESTION:
                        return GameStates.TALK;

                    default:
                        return GameStates.TALK;
                }
            }
            return gameState;
        }
    }
}
