using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
        private bool nightTime;

        public GameManager()
        {
            players = new List<Player>();
            isOver = false;
            round = 0;
            nightTime = false;
            roundVotes = new Dictionary<string, int>();
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
            roundVotes.Clear();

            foreach (Player player in players)
            {
                if (player.isPlayerDead()) continue;
                string instructions = player.playRound(nightTime);
                roundSummary.Append("Player : ");
                roundSummary.AppendLine(player.getPlayerName());
                roundSummary.AppendLine(runInstructions(instructions));
            }
            if(nightTime)
            {
                //NightTime logic
            }
            else
            {
                //DayTime logic
                roundSummary.AppendLine(dayTimeLogic());
            }

            roundSummary.AppendLine(string.Format("\nRound {0} at {1} finished.", round, nightTime ? "NightTime":"DayTime"));

            round = nightTime ? round + 1 : round;
            nightTime = !nightTime;

            return roundSummary.ToString();            
        }

        private string dayTimeLogic()
        {
            string result = "No player was accused this round";

            //Accuse Player Logic
            string accusedPlayerName = roundVotes.FirstOrDefault(x => x.Value == roundVotes.Values.Max()).Key;
            Player accusedPlayer = players.FirstOrDefault(p=>p.getPlayerName().Equals(accusedPlayerName));

            if (accusedPlayer != null)
            {
                result = string.Format("Player {0} was accused and is now dead", accusedPlayerName);
                accusedPlayer.killPlayer();
            }

            return result;
        }

        private string runInstructions(string instructions)
        {
            String[] instructionList = instructions.Split(' ');
            string instruction = instructionList[0];
            if (nightTime)
            {
                switch(instruction)
                {
                    case "heal":
                        return string.Format("heals {0}\n", instructionList[1]);
                    case "question":
                        return string.Format("questions {0}\n", instructionList[1]);
                    case "kill":
                        return string.Format("kills {0}\n", instructionList[1]);
                    default:
                        return "passes\n";
                }
            }
            else
            {
                switch (instruction)
                {
                    case "talk":
                        return string.Format("says {0}\n", string.Join(" ", instructionList.Where(s => !s.Equals("talk"))));
                    case "accuse":
                        AccusePlayer(instructionList[1]);
                        return string.Format("accuses {0}\n", instructionList[1]);
                    default:
                        return "passes\n";
                }
            }
        }

        private void AccusePlayer(string playerName)
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
    }
}
