using System;
using System.Collections.Generic;
using WereWolf.General;
using System.Linq;
using System.Text;

namespace WereWolf
{
    public class RolloutGame
    {
        public const int WEREWOLF_NUMBER = 2;
        public const int SEER_NUMBER = 1;
        public const int DOCTOR_NUMBER = 1;
        public const int VILLAGER_NUMBER = 2;

        private List<Player> players;
        private Dictionary<string, int> roundVotes;
        private GameStates gameState;

        public RolloutGame(List<Player> players, GameStates gameState)
        {
            this.players = new List<Player>(players);
            this.gameState = gameState;
            roundVotes = new Dictionary<string, int>();

            List<String> names = new List<String>();
            List<String> werewolves = new List<String>();
            foreach (Player p in players)
            {
                names.Add(p.getPlayerName());
                if (p.getCharName().Equals("Werewolf"))
                {
                    werewolves.Add(p.getPlayerName());
                }
            }

            foreach (Player p in players)
            {
                p.setPlayersList(names);
                if (p.getCharName().Equals("Werewolf"))
                {
                    p.addFriends(werewolves);
                }
            }
        }

        public void sampleGame(string command)
        {
            playRound(command);
            while (!isGameOver())
            {
                playRound(string.Empty);
            }
        }

        private bool werewolfesWin()
        {
            //All players that are werewolfs are dead OR all players that are not werewolfs are dead.
            return players.Where(p => p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count() >= players.Where(p => !p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count();
        }

        public int evalGame2(string team)
        {
            if (werewolfesWin() && team.Equals("Werewolf"))
                return players.Where(p => p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count();
            else if(!werewolfesWin() && team.Equals("Werewolf"))
                return -players.Where(p => !p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count();

            if(!werewolfesWin() && !team.Equals("Werewolf"))
                return players.Where(p => !p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count();
            else return -players.Where(p => p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count();
        }

        public int evalGame(string team)
        {
            int result = 0;
            if (werewolfesWin() && team.Equals("Werewolf"))
            {
                result = players.Where(p => p.getCharName().Equals("Seer") && p.isPlayerDead()).Count() * 4;
                result += players.Where(p => p.getCharName().Equals("Doctor") && p.isPlayerDead()).Count() * 3;
                result += players.Where(p => p.getCharName().Equals("Villager") && p.isPlayerDead()).Count() * 2;
                result += players.Where(p => p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count();
            }
            else if (!werewolfesWin() && team.Equals("Werewolf"))
            {
                result = -players.Where(p => p.getCharName().Equals("Seer") && !p.isPlayerDead()).Count() * 4;
                result -= players.Where(p => p.getCharName().Equals("Doctor")   && !p.isPlayerDead()).Count() * 3;
                result -= players.Where(p => p.getCharName().Equals("Villager") && !p.isPlayerDead()).Count() * 2;
                result -= players.Where(p => p.getCharName().Equals("Werewolf") && p.isPlayerDead()).Count() * 2;
            }

            if (!werewolfesWin() && !team.Equals("Werewolf"))
            {
                result = players.Where(p => p.getCharName().Equals("Seer") && !p.isPlayerDead()).Count() * 3;
                result += players.Where(p => p.getCharName().Equals("Doctor") && !p.isPlayerDead()).Count() * 2;
                result += players.Where(p => p.getCharName().Equals("Villager") && !p.isPlayerDead()).Count();
                result += players.Where(p => p.getCharName().Equals("Werewolf") && p.isPlayerDead()).Count() * 5;
            }
            else
            {
                result -= players.Where(p => p.getCharName().Equals("Seer") && p.isPlayerDead()).Count() * 3;
                result -= players.Where(p => p.getCharName().Equals("Doctor") && p.isPlayerDead()).Count() * 2;
                result -= players.Where(p => p.getCharName().Equals("Villager") && p.isPlayerDead()).Count();
                result -= players.Where(p => p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count() * 5;
            }
            return result;
        }
        public void playRound(string command)
        {
            StringBuilder roundSummary = new StringBuilder();
            foreach (Player player in players)
            {
                string instructions = string.Empty;
                if (player.isPlayerDead()) continue;

                if (!string.IsNullOrEmpty(command))
                {
                    instructions = command;
                    command = string.Empty;
                }
                else
                {
                    instructions = player.playRound(gameState);
                }

                if (!string.IsNullOrEmpty(instructions))
                {
                    roundSummary.AppendLine(runInstructions(instructions, player));
                }
            }

            if (gameState == GameStates.KILL)
            {
                roundSummary.AppendLine(killLogic());
            }

            if (gameState == GameStates.ACCUSE)
            {
                roundSummary.AppendLine(accuseLogic());
                roundVotes.Clear();
            }

            if (gameState == GameStates.QUESTION)
            {
                Player mostVotedPlayer = getMostVotedPlayer();
                //ONLY for testing purposes this shouldnt happen with intelligent AI
                if (mostVotedPlayer != null)
                {
                    roundSummary.AppendLine(string.Format("Player {0} was the choosen one to be killed.", mostVotedPlayer.getPlayerName()));
                    if (mostVotedPlayer.isPlayerDead()) roundSummary.AppendLine(string.Format("Player {0} is dead forever.", mostVotedPlayer.getPlayerName()));
                    if (!mostVotedPlayer.isPlayerDead()) roundSummary.AppendLine(string.Format("Player {0} is still alive because he was healed.", mostVotedPlayer.getPlayerName()));
                }

                roundSummary.AppendLine("\nRound finished.");
                roundVotes.Clear();
            }
            gameState = nextGameState();

            broadcastRoundSummary(roundSummary.ToString());
        }

        public bool isGameOver()
        {
            //All players that are werewolfs are dead OR all players that are not werewolfs are dead.
            bool allWereWolfsDead = players.All(p => p.getCharName().Equals("Werewolf") && p.isPlayerDead() || !p.getCharName().Equals("Werewolf"));
            bool allNonWerewolfsDead = players.Where(p => p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count() >= players.Where(p => !p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count();
            return allWereWolfsDead || allNonWerewolfsDead;
        }

        private string accuseLogic()
        {
            string result = "No player was accused this round";

            //Accuse Player Logic
            Player accusedPlayer = getMostVotedPlayer();

            if (accusedPlayer != null)
            {
                result = string.Format("Player {0} was accused and is now dead", accusedPlayer.getPlayerName());
                accusedPlayer.killPlayer();
            }

            return result;
        }

        private void broadcastRoundSummary(string roundSummary)
        {
            foreach (Player player in players)
            {
                if (player.isPlayerDead()) continue;
                player.applyRoundSummary(roundSummary);
            }
        }

        private string killLogic()
        {
            string result = "No player was killed this round";

            //Kill Player Logic
            Player killedPlayer = getMostVotedPlayer();

            //This should never happen, just for testing sake
            if (killedPlayer != null)
            {
                result = string.Format("A player has been chosen to be killed by the werewolfes");
                killedPlayer.killPlayer();
            }

            return result;
        }


        private string runInstructions(string instructions, Player player)
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
                    return string.Format("Player {0} says {1}", player.getPlayerName(), string.Join(" ", instructionList.Where(s => !s.Equals("talk"))));

                case "accuse":
                    VotePlayer(instructionList[1]);
                    return string.Format("Player {0} accuses {1}", player.getPlayerName(), instructionList[1]);

                default:
                    return string.Format("Player {0} passes", player.getPlayerName());
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

        private void QuestionPlayer(string playerName, Player player)
        {
            Player playerQuestioned = getPlayerByName(playerName);
            player.seerAnswer(playerName, playerQuestioned.getCharName());
        }

        private string HealPlayer(string playerName)
        {
            string result = "No player was healed this round";

            //Kill Player Logic
            string killedPlayerName = getMostVotedPlayerName();

            if (playerName.Equals(killedPlayerName))
            {
                Player killedPlayer = getPlayerByName(killedPlayerName);

                //This should never happen, just for testing sake
                if (killedPlayer != null)
                {
                    result = string.Format("A player has been choosed to be healed.");
                    killedPlayer.healPlayer();
                }
            }

            return result;
        }

        private string getMostVotedPlayerName()
        {
            return roundVotes.FirstOrDefault(x => x.Value == roundVotes.Values.Max()).Key;
        }

        private Player getMostVotedPlayer()
        {
            string votedPlayerName = roundVotes.FirstOrDefault(x => x.Value == roundVotes.Values.Max()).Key;
            return players.FirstOrDefault(p => p.getPlayerName().Equals(votedPlayerName));
        }

        private Player getPlayerByName(string playerName)
        {
            return players.FirstOrDefault(p => p.getPlayerName().Equals(playerName));
        }

        private GameStates nextGameState()
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
    }
}
