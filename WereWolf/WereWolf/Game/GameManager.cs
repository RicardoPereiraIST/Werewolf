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
        public const int VILLAGER_NUMBER = 2;

        private List<Player> players;
        private Dictionary<string, int> roundVotes;

        private bool isOver;
        private int round;

        private GameStates gameState;

        public GameManager()
        {
            players = new List<Player>();
            isOver = false;
            round = 1;
            roundVotes = new Dictionary<string, int>();

            gameState = GameStates.TALK;
        }

        public string StartGame(int numPlayers)
        {
            try
            {
                for (int i = 0; i < WEREWOLF_NUMBER; i++)
                {
                    players.Add(new Player("Werewolf", false, "W"+i));
                }

                for (int i = 0; i < SEER_NUMBER; i++)
                {
                    players.Add(new Player("Seer", true, "S" + i));
                }

                for (int i = 0; i < DOCTOR_NUMBER; i++)
                {
                    players.Add(new Player("Doctor", false, "D" +i));
                }

                for (int i = 0; i < VILLAGER_NUMBER; i++)
                {
                    players.Add(new Player("Villager", false, "V"+i ));
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
            return isOver;
        }

        public string playRound()
        {
            StringBuilder roundSummary = new StringBuilder();

            foreach (Player player in players)
            {
                if (player.isPlayerDead()) continue;
                string instructions = player.playRound(gameState);

                roundSummary.Append("Player : ");
                roundSummary.AppendLine(player.getPlayerName());
                roundSummary.AppendLine(runInstructions(instructions, player));
            }

            if(gameState == GameStates.KILL)
            {
                roundSummary.AppendLine(killLogic());
            }
            
            if(gameState == GameStates.ACCUSE)
            {
                roundSummary.AppendLine(accuseLogic());
                roundVotes.Clear();
            }

            if(gameState == GameStates.HEAL)
            {
                roundVotes.Clear();
            }

            if (gameState == GameStates.QUESTION)
            {
                roundSummary.AppendLine(string.Format("\nRound {0} finished.", round));
            }

            gameState = nextGameState();
            round = gameState == GameStates.TALK ? round + 1 : round;

            return roundSummary.ToString();            
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

        private string killLogic()
        {
            string result = "No player was killed this round";

            //Kill Player Logic
            Player killedPlayer = getMostVotedPlayer();

            //This should never happen, just for testing sake
            if (killedPlayer != null)
            {
                result = string.Format("Player {0} was killed and is now dead", killedPlayer.getPlayerName());
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
                    HealPlayer(instructionList[1]);
                    return string.Format("heals {0}\n", instructionList[1]);

                case "question":
                    QuestionPlayer(instructionList[1], player);
                    return string.Format("questions {0}\n", instructionList[1]);

                case "kill":
                    VotePlayer(instructionList[1]);
                    return string.Format("kills {0}\n", instructionList[1]);

                case "talk":
                    return string.Format("says {0}\n", string.Join(" ", instructionList.Where(s => !s.Equals("talk"))));

                case "accuse":
                    VotePlayer(instructionList[1]);
                    return string.Format("accuses {0}\n", instructionList[1]);

                default:
                    return "passes\n";
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
                    result = string.Format("Player {0} was healed and is now alive", killedPlayer);
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
