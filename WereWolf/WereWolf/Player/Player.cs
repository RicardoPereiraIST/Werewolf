using System;
using System.Text;
using WereWolf.General;

namespace WereWolf
{
    public class Player
    {
        private Character character;
        private Agent agent;
        private string playerName;
        private bool isHuman;
        private string characterName;

        public Player(string name, bool isHuman, string playerName)
        {
            character = CharacterAbstractFactory.CreatePlayer(name);
            this.isHuman = isHuman;
            this.playerName = playerName;
            characterName = name;
        }

        public string getPlayerName()
        {
            return playerName;
        }

        public void killPlayer()
        {
            character.kill();
        }

        public void healPlayer()
        {
            character.heal();
        }

        public bool isPlayerDead()
        {
            return character.isDead();
        }

        public string getCharName()
        {
            return characterName;
        }

        public void seerAnswer(string playerName, string character)
        {
            if (isHuman)
            {
                Console.WriteLine(character);
                Console.ReadLine();
            }
            else
            {
                //Agent logic for question
            }
        }

        public void applyRoundSummary(string roundSummary)
        {
            if (isHuman)
            {
                Console.WriteLine(roundSummary);
            }
            else
            {
                //Agent RoundSummary Interpretation
            }
        }

        public string playRound(GameStates gameState)
        {
            StringBuilder instructions = new StringBuilder();
            if (isHuman)
            {
                instructions.AppendLine(string.Format("Player {0} instructions:",playerName));

                if (character.canHeal() && gameState == GameStates.HEAL)
                {
                    instructions.AppendLine("- heal PlayerName");
                }
                else if (character.canKill() && gameState == GameStates.KILL)
                {
                    instructions.AppendLine("- kill PlayerName");
                }
                else if (character.canQuestion() && gameState == GameStates.QUESTION)
                {
                    instructions.AppendLine("- question PlayerName");
                }
                else if (gameState == GameStates.TALK)
                {
                    instructions.AppendLine("- talk \"phrase\"");
                }
                else if (gameState == GameStates.ACCUSE)
                {
                    instructions.AppendLine("- accuse PlayerName");
                }
                else return string.Empty;

                instructions.AppendLine("- pass");

                Console.Write(instructions.ToString());
                return Console.ReadLine();
            }
            else
            {
                //Agent Decisions
                if (gameState == GameStates.ACCUSE)
                {
                    return "pass";
                }
                return string.Empty;
            }
        }
    }
}
