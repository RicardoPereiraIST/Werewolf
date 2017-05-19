using System;
using System.Collections.Generic;
using System.Linq;
using WereWolf.General;
using System.Text;

namespace WereWolf
{
    public class GameManager
    {
        private List<Player> players;
        private Dictionary<string, int> roundVotes;
        private string healedPlayer;
        private List<string> playerNames;

        private int round;
        private int victoryRound;

        private GameStates gameState;
        Random rand;

        public GameManager()
        {
            players = new List<Player>();
            round = 1;
            roundVotes = new Dictionary<string, int>();
            playerNames = new List<string>() {
                "Alexa", "Ana",
                "Beto", "Bruno",
                "Carlos", "Catarina",
                "David",
                "Eder",
                "Fabio",
                "Joaquim", "Jose",
                "Mafalda", "Manuel", "Maria",
                "Nuno",
                "Paulo",
                "Rafael", "Renato", "Rosa",
                "Sofia" };

            gameState = GameStates.KILL;
            rand = new Random(Guid.NewGuid().GetHashCode());
            shuffleList(playerNames);
            healedPlayer = string.Empty;
        }

        private void shuffleList<T>(List<T> listToShuffle)
        {
            for (int i = listToShuffle.Count; i > 1; i--)
            {
                int pos = rand.Next(i);
                var x = listToShuffle[i - 1];
                listToShuffle[i - 1] = listToShuffle[pos];
                listToShuffle[pos] = x;
            }
        }
        public string StartGame(bool isPlayerPlaying, string playerName)
        {
            try
            {
                for (int i = 0; i <Constants.WEREWOLF_NUMBER; i++)
                {
                    players.Add(new Player("Werewolf", playerNames[i], rand.Next(3) == 1));
                    playerNames.RemoveAt(i);
                }

                for (int i = 0; i < Constants.SEER_NUMBER; i++)
                {
                    players.Add(new Player("Seer", playerNames[i], rand.Next(3) == 1));
                    playerNames.RemoveAt(i);
                }

                for (int i = 0; i < Constants.DOCTOR_NUMBER; i++)
                {
                    players.Add(new Player("Doctor", playerNames[i], rand.Next(3) == 1));
                    playerNames.RemoveAt(i);
                }

                for (int i = 0; i < Constants.VILLAGER_NUMBER; i++)
                {
                    players.Add(new Player("Villager", playerNames[i], rand.Next(3) == 1));
                    playerNames.RemoveAt(i);
                }

                shuffleList(players);

                if (isPlayerPlaying)
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
            return "Game Setup Sucessfully\n";
        }


        public List<Player> getPlayers()
        {
            return players;
		}

        public void ReinitializeGame()
        {
            shuffleList(players);

            for (int i = 0; i < Constants.WEREWOLF_NUMBER; i++)
            {
                players[i].reinitializePlayer("Werewolf");
            }

            for (int i = 0; i < Constants.SEER_NUMBER; i++)
            {
                players[Constants.WEREWOLF_NUMBER +i].reinitializePlayer("Seer");
            }

            for (int i = 0; i < Constants.DOCTOR_NUMBER; i++)
            {
                players[Constants.WEREWOLF_NUMBER + Constants.SEER_NUMBER + i].reinitializePlayer("Doctor");
            }

            for (int i = 0; i < Constants.VILLAGER_NUMBER; i++)
            {
                players[Constants.WEREWOLF_NUMBER + Constants.SEER_NUMBER + Constants.DOCTOR_NUMBER + i].reinitializePlayer("Villager");
            }

            shuffleList(players);

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

            gameState = GameStates.KILL;
        }

        public bool isGameOver()
        {
            //All players that are werewolfs are dead OR all players that are not werewolfs are dead.
            bool allWereWolfsDead = players.All(p => p.getCharName().Equals("Werewolf") && p.isPlayerDead() || !p.getCharName().Equals("Werewolf"));
            bool allNonWerewolfsDead = players.Where(p => p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count() >= players.Where(p => !p.getCharName().Equals("Werewolf") && !p.isPlayerDead()).Count();
            return allWereWolfsDead || allNonWerewolfsDead;
        }

        public bool werewolfsWon()
        {
            bool allWereWolfsDead = players.All(p => p.getCharName().Equals("Werewolf") && p.isPlayerDead() || !p.getCharName().Equals("Werewolf"));
            return allWereWolfsDead ? false : true;
        }

        private string gameOverMessage()
        {
            //All players that are werewolfs are dead OR all players that are not werewolfs are dead.
            if (players.All(p => p.getCharName().Equals("Werewolf") && p.isPlayerDead() || !p.getCharName().Equals("Werewolf"))) return "Werewolves lose.";
            else return "Werewolves win.";
        }

        public void playRound()
        {
            StringBuilder roundSummary = new StringBuilder();
            Console.WriteLine("----------------------");
            foreach (Player player in players)
            {
                if (player.isPlayerDead()) continue;
                string instructions = player.playRound(gameState);

                if (!string.IsNullOrEmpty(instructions))
                {
                    string instructionInfo = runInstructions(instructions, player);
                    roundSummary.Append(instructionInfo);
                    Console.Write(instructionInfo);
                }
            }

            if (gameState == GameStates.HEAL)
            {
                foreach (Player player in players)
                {
                    if (player.isPlayerDead() && player.getCharName().Equals("Doctor"))
                        Console.WriteLine("Doctor is dead. Players cannot be healed anymore");
                }
            }

            if (gameState == GameStates.KILL)
            {
                Console.WriteLine("A player was chosen to be killed by the werewolves");
            }
            
            if(gameState == GameStates.ACCUSE)
            {
                string accuseLogicResult = accuseLogic();
                Console.WriteLine(accuseLogicResult);
                roundSummary.AppendLine(accuseLogicResult);
                roundVotes.Clear();
                Console.WriteLine("----------------------");
                Console.WriteLine(string.Format("## Day has ended. (Round {0})", round++));
                Console.WriteLine("----------------------");
            }

            if (gameState == GameStates.QUESTION)
            {
                killLogic();
                Player mostVotedPlayer = getMostVotedPlayer();
                //ONLY for testing purposes this shouldnt happen with intelligent AI
                if (mostVotedPlayer != null)
                {
                    Console.WriteLine(string.Format("Player {0} was the choosen one to be killed.", mostVotedPlayer.getPlayerName()));
                    if (healedPlayer != mostVotedPlayer.getPlayerName())
                    {
                        Console.WriteLine("Player {0} is dead forever. His role was {1}", mostVotedPlayer.getPlayerName(), mostVotedPlayer.getCharName());
                        roundSummary.AppendLine(string.Format("Player {0} is dead forever. His role was {1}", mostVotedPlayer.getPlayerName(), mostVotedPlayer.getCharName()));
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Player {0} is still alive because he was healed.", mostVotedPlayer.getPlayerName()));
                        roundSummary.AppendLine(string.Format("Player {0} is still alive because he was healed.", mostVotedPlayer.getPlayerName()));
                        mostVotedPlayer.healPlayer();
                    }
                }
                healedPlayer = string.Empty;

                Console.WriteLine("----------------------");
                Console.WriteLine(string.Format("## Night has ended. (Round {0})", round++));
                Console.WriteLine("----------------------");
                roundVotes.Clear();
            }

            gameState = nextGameState();
            broadcastRoundSummary(roundSummary.ToString());

            if (isGameOver())
            {
                Console.WriteLine("----------------------");
                Console.WriteLine(string.Format("## Game is over {0}", gameOverMessage()));
                Console.WriteLine("----------------------");
                victoryRound = round-1;
                round = 1;
            }
                
        }

        private string accuseLogic()
        {
            string result = "No player was accused this round";

            //Accuse Player Logic
            Player accusedPlayer = getMostVotedPlayer();

            if (accusedPlayer != null)
            {
                result = string.Format("Player {0} was accused and is now dead. His role was {1}", accusedPlayer.getPlayerName(), accusedPlayer.getCharName());
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
            if(killedPlayer != null)
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
                    result = string.Format("A player was chosen to be healed.\n");
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

        public int getVictoryRound() { return victoryRound; }
    }
}
