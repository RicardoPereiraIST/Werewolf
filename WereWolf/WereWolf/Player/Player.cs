using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    public class Player
    {
        private Character character;
        private Agent agent;
        private string playerName;
        private bool isHuman; 

        public Player(string name, bool isHuman, string playerName)
        {
            character = CharacterAbstractFactory.CreatePlayer(name);
            this.isHuman = isHuman;
            this.playerName = playerName;
        }

        public string getPlayerName()
        {
            return playerName;
        }

        public void killPlayer()
        {
            character.kill();
        }
        public bool isPlayerDead()
        {
            return character.isDead();
        }


        public string playRound(bool nightTime)
        {
            StringBuilder instructions = new StringBuilder();
            if (isHuman)
            {
                instructions.AppendLine(string.Format("Player {0} instructions:",playerName));
                if (nightTime)
                {
                    if (character.canHeal())
                    {
                        instructions.AppendLine("- heal PlayerName");
                    }
                    if (character.canKill())
                    {
                        instructions.AppendLine("- kill PlayerName");
                    }
                    if (character.canQuestion())
                    {
                        instructions.AppendLine("- question PlayerName");
                    }
                }
                else
                {
                    instructions.AppendLine("- accuse PlayerName");
                    instructions.AppendLine("- talk \"phrase\"");
                    instructions.AppendLine("- pass");
                }
                Console.Write(instructions.ToString());
                return Console.ReadLine();
            }
            else
            {
                //Agent Decisions
                return "pass";
            }
        }
    }
}
