using System;
using System.Text;
using System.Collections.Generic;
using WereWolf.General;
using System.IO;

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
            agent = new Agent();
        }

        public void setPlayersList(List<String> players)
        {
            agent.setPlayersList(players);
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
                StringReader summaryList = new StringReader(roundSummary);
                string play;
                while ((play = summaryList.ReadLine()) != null)
                {
                    if (play.Contains("----------------------")) continue;
                    if (play.Contains("passes")) continue;
                    if (play.Contains("No player")) continue;

                    String[] playList = play.Split(' ');
                    if (playList.Length > 2)
                    {
                         if(play.Contains("dead") || play.Contains("killed"))
                           agent.killPlayer(playList[1]);

                        if (play.Contains("accuses"))
                            agent.accusePlayedRound(playList[1], playList[3]);
                    }

                    

                }
            }
        }

        public void printList()
        {
            agent.printList();
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
                if (character.canHeal() && gameState == GameStates.HEAL)
                {
                    return agent.healRound();
                }
                else if (character.canKill() && gameState == GameStates.KILL)
                {
                    return agent.killRound();
                }
                else if (character.canQuestion() && gameState == GameStates.QUESTION)
                {
                    return agent.questionRound();
                }
                else if (gameState == GameStates.TALK)
                {
                    return agent.talkRound();
                }
                else if (gameState == GameStates.ACCUSE)
                {
                    return agent.accuseRound();
                }
                return string.Empty;
            }
        }
    }
}
