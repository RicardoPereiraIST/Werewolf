using System;
using System.Collections.Generic;
using System.Linq;

namespace WereWolf
{
    public class PIMCAgent : Agent
    {
        private InformationSet infoSet;
        private const int N = 10;
        private Player player;

        public PIMCAgent(Player player)
        {
            infoSet = new InformationSet(player.getPlayerName());
            this.player = player;
        }

        public void setPlayersList(List<String> p)
        {
            infoSet.setPlayersList(p);
        }

        public void killPlayer(string playerName)
        {
            infoSet.addKillPlay(playerName);
        }

        public void addFriend(string friend)
        {
            infoSet.addFriend(friend);
        }

        public string talkRound()
        {
            //Value of the talks, every one has a 0 value in the beginning.
            Dictionary<String, int> possibleTalks = infoSet.getPossibleTalks();

            for (int i = 0; i < N; i++)
            {
                List<Player> accuseSample = new List<Player>();
                accuseSample.Add(player.Copy());
                accuseSample.AddRange(infoSet.Sample());

                RolloutGame game;
                int talkUtility;

                foreach (string possibleTalk in possibleTalks.Keys)
                {


                }
            }

            return string.Format("talk {0}", possibleTalks.FirstOrDefault(x => x.Value == possibleTalks.Values.Max()).Key);
        }

        public string accuseRound()
        {
            Dictionary<String, int> possibleAccuses = infoSet.getPossibleAccuses();

            for (int i = 0; i < N; i++)
            {
                List<Player> accuseSample = new List<Player>();
                accuseSample.Add(player.Copy());
                accuseSample.AddRange(infoSet.Sample());

                RolloutGame game;

                foreach (string possibleAccuse in infoSet.getPossibleAccuses().Keys)
                {
                    List<Player> accuseSampleGame = accuseSample.Select(x => x.Copy()).ToList();

                    game = new RolloutGame(accuseSampleGame, General.GameStates.ACCUSE);
                    game.sampleGame(string.Format("accuse {0}", possibleAccuse));
                    possibleAccuses[possibleAccuse] += game.evalGame(player.getCharName());
                }
            }

            return string.Format("accuse {0}", possibleAccuses.FirstOrDefault(x => x.Value == possibleAccuses.Values.Max()).Key);
        }

        public string killRound()
        {
            Dictionary<String, int> possibleKills = infoSet.getPossibleKills();

            for (int i = 0; i < N; i++)
            {
                List<Player> accuseSample = new List<Player>();
                accuseSample.Add(player.Copy());
                accuseSample.AddRange(infoSet.Sample());
                RolloutGame game;
                int accuseUtility;

                foreach (string possibleKill in possibleKills.Keys)
                {


                }
            }

            return string.Format("kill {0}", infoSet.ruledBasedAccuse());
        }

        public string healRound()
        {
            Dictionary<String, int> possibleHeals = infoSet.getPossibleHeals();

            for (int i = 0; i < N; i++)
            {
                List<Player> accuseSample = new List<Player>();
                accuseSample.Add(player.Copy());
                accuseSample.AddRange(infoSet.Sample());
                RolloutGame game;
                int accuseUtility;

                foreach (string possibleHeal in possibleHeals.Keys)
                {

                }
            }

            return string.Format("heal {0}", possibleHeals.FirstOrDefault(x => x.Value == possibleHeals.Values.Max()).Key);
        }

        public string questionRound()
        {
            Dictionary<String, int> possibleQuestions = infoSet.getPossibleQuestions();

            for (int i = 0; i < N; i++)
            {
                string talkSample = infoSet.questionSample();
                RolloutGame game;
                int accuseUtility;

                foreach (string possibleQuestion in possibleQuestions.Keys)
                {

                }
            }

            return string.Format("question {0}", possibleQuestions.FirstOrDefault(x => x.Value == possibleQuestions.Values.Max()).Key);
        }

        public void accusePlayedRound(string playerName, string accusedPlayerName)
        {
            infoSet.addAccusePlay(playerName, accusedPlayerName);
        }

        public void seerQuestion(string playerName, string roleName)
        {
            infoSet.addSeerAnswer(playerName, roleName);
        }
    }
}
