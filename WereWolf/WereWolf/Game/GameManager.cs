using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WereWolf.General;

namespace WereWolf
{
    public class GameManager
    {
        public const int WEREWOLF_NUMBER = 2;
        public const int SEER_NUMBER = 1;
        public const int DOCTOR_NUMBER = 1;
        public const int VILLAGER_NUMBER = 5;

        private List<Player> players;
        private Dictionary<string, int> roundVotes;
        private string healedPlayer;
        private List<string> playerNames;

        private int round;

        private GameStates gameState;
        Random rand;

        public GameManager()
        {
            players = new List<Player>();
            round = 1;
            roundVotes = new Dictionary<string, int>();
            playerNames = new List<String>() { "aaron", "abdul", "abe", "abel", "abraham", "adam", "adan", "adolfo", "adolph", "adrian", "abby", "abigail", "adele", "adrian", "abbott", "acosta", "adams", "adkins", "aguilar" };

            gameState = GameStates.KILL;
            rand = new Random(Guid.NewGuid().GetHashCode());
            playerNames = playerNames.OrderBy(c => rand.Next()).Select(c => c).ToList();
            healedPlayer = string.Empty;
        }

        public string StartGame(bool isPlayerPlaying, string playerName)
        {
            try
            {
                for (int i = 0; i < WEREWOLF_NUMBER; i++)
                {
                    players.Add(new Player("Werewolf", playerNames[i]));
                    playerNames.RemoveAt(i);
                }

                for (int i = 0; i < SEER_NUMBER; i++)
                {
                    players.Add(new Player("Seer", playerNames[i]));
                    playerNames.RemoveAt(i);
                }

                for (int i = 0; i < DOCTOR_NUMBER; i++)
                {
                    players.Add(new Player("Doctor", playerNames[i]));
                    playerNames.RemoveAt(i);
                }

                for (int i = 0; i < VILLAGER_NUMBER; i++)
                {
                    players.Add(new Player("Villager", playerNames[i]));
                    playerNames.RemoveAt(i);
                }

                players = players.OrderBy(c => rand.Next()).Select(c => c).ToList();

                if(isPlayerPlaying)
                 players[rand.Next(players.Count)].setPlayerAsHuman(playerName);

                List<String> names = new List<String>();
                List<String> werewolves = new List<String>();

                foreach(Player p in players)
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
            catch(Exception ex)
            {
                throw new Exception("Error on starting game (StartGame function): " + ex.Message);
            }
            return "Game Setup Sucessfull\n";
        }

        public bool isGameOver()
        {
            //All players that are werewolfs are dead OR all players that are not werewolfs are dead.
            bool allWereWolfsDead = players.All(p => p.getCharName().Equals("Werewolf") && p.isPlayerDead() || !p.getCharName().Equals("Werewolf"));
            bool allNonWerewolfsDead = players.Where(p => p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count() >= players.Where(p => !p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count();
            return allWereWolfsDead || allNonWerewolfsDead;
        }

        private string gameOverMessage()
        {
            //All players that are werewolfs are dead OR all players that are not werewolfs are dead.
            if (players.All(p => p.getCharName().Equals("Werewolf") && p.isPlayerDead() || !p.getCharName().Equals("Werewolf"))) return "Werewolfes lose";
            else return "Werewolfes win";
        }

        public void playRound()
        {
            StringBuilder roundSummary = new StringBuilder();
            roundSummary.AppendLine("----------------------");
            foreach (Player player in players)
            {
                if (player.isPlayerDead()) continue;
                string instructions = player.playRound(gameState);

                if (!string.IsNullOrEmpty(instructions))
                {
                    roundSummary.Append(runInstructions(instructions, player));
                }
            }

            if(gameState == GameStates.KILL)
            {
                roundSummary.AppendLine("A player has been chosen to be killed by the werewolfes");
            }
            
            if(gameState == GameStates.ACCUSE)
            {
                roundSummary.AppendLine(accuseLogic());
                roundVotes.Clear();
            }

            if (gameState == GameStates.QUESTION)
            {
                killLogic();
                Player mostVotedPlayer = getMostVotedPlayer();
                //ONLY for testing purposes this shouldnt happen with intelligent AI
                if (mostVotedPlayer != null)
                {
                    roundSummary.AppendLine(string.Format("Player {0} was the choosen one to be killed.", mostVotedPlayer.getPlayerName()));
                    if (healedPlayer != mostVotedPlayer.getPlayerName())
                    {
                        roundSummary.AppendLine(string.Format("Player {0} is dead forever.", mostVotedPlayer.getPlayerName()));
                    }
                    else
                    {
                        roundSummary.AppendLine(string.Format("Player {0} is still alive because he was healed.", mostVotedPlayer.getPlayerName()));
                        mostVotedPlayer.healPlayer();
                    }
                }
                healedPlayer = string.Empty;

                roundSummary.AppendLine(string.Format("\nRound {0} finished.", round));
                roundVotes.Clear();
            }

            roundSummary.AppendLine("----------------------");

            gameState = nextGameState();
            round = gameState == GameStates.TALK ? round + 1 : round;

            broadcastRoundSummary(roundSummary.ToString());
            if (isGameOver())
                Console.WriteLine(string.Format("\nGame is over {0}", gameOverMessage()));
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
                player.applyRoundSummary(roundSummary);
            }
        }

        private void killLogic()
        {
            //Kill Player Logic
            Player killedPlayer = getMostVotedPlayer();

            //This should never happen, just for testing sake
            killedPlayer.killPlayer();
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
                    return string.Empty;

                case "kill":
                    VotePlayer(instructionList[1]);
                    return string.Empty;

                case "talk":
                    return string.Format("Player {0} says {1}\n", player.getPlayerName(), string.Join(" ", instructionList.Where(s => !s.Equals("talk"))));

                case "accuse":
                    VotePlayer(instructionList[1]);
                    return string.Format("Player {0} accuses {1}\n", player.getPlayerName(), instructionList[1]);

                default:
                    return string.Format("Player {0} passes\n", player.getPlayerName());
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
            string result = "No player was healed this round\n";

            //Kill Player Logic
            string killedPlayerName = getMostVotedPlayerName();

            if (playerName.Equals(killedPlayerName))
            {
                Player killedPlayer = getPlayerByName(killedPlayerName);

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
            switch(gameState)
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
