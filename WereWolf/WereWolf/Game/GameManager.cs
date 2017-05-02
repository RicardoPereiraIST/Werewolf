using System;
using System.Collections.Generic;

namespace WereWolf
{
    public class GameManager
    {
        public const int WEREWOLF_NUMBER = 2;
        public const int SEER_NUMBER = 1;
        public const int DOCTOR_NUMBER = 1;
        public const int VILLAGER_NUMBER = 2;

        private List<Player> players;
        private bool isOver;
        private int round;
        private bool nightTime;

        public GameManager()
        {
            players = new List<Player>();
            isOver = false;
            round = 0;
            nightTime = false;
        }

        public string StartGame(int numPlayers)
        {
            try
            {
                for (int i = 0; i < WEREWOLF_NUMBER; i++)
                {
                    players.Add(new Player("Werewolf", true, "W"+i));
                }

                for (int i = 0; i < SEER_NUMBER; i++)
                {
                    players.Add(new Player("Seer", true, "S" + i));
                }

                for (int i = 0; i < DOCTOR_NUMBER; i++)
                {
                    players.Add(new Player("Doctor", true, "D" +i));
                }

                for (int i = 0; i < VILLAGER_NUMBER; i++)
                {
                    players.Add(new Player("Villager", true, "V"+i ));
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error on starting game (StartGame function): " + ex.Message);
            }
            return "Game Setup Sucessfull";
        }

        public bool isGameOver()
        {
            return isOver;
        }

        public string playRound()
        {
            foreach(Player player in players)
            {
                string instructions = player.playRound(nightTime);
                runInstructions(instructions);
            }

            return string.Format("Round {0} at {1} finished.", round, nightTime ? "NightTime":"DayTime");            
        }

        private void runInstructions(string instructions)
        {
            return;
        }
    }
}
