using System;
using System.Collections.Generic;
using System.Linq;

namespace WereWolf
{
    public class PIMCAgent
    {
        private InformationSet infoSet;
        private int N;

        public PIMCAgent(string playerName)
        {
            infoSet = new InformationSet(playerName);
        }

        public void setPlayersList(List<String> p)
        {
            infoSet.setPlayersList(p);
        }

        public void killPlayer(string playerName)
        {
            infoSet.addKillPlay(playerName);
        }

        public string talkRound()
        {
            //Value of the talks, every one has a 0 value in the beginning.
            Dictionary<String, int> possibleTalks = infoSet.getPossibleTalks();

            for (int i = 0; i < N; i++)
            {
                string talkSample = infoSet.talkSample();
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
                List<KeyValuePair<string,string>> talkSample = infoSet.accuseSample();
                RolloutGame game;
                int accuseUtility;

                foreach(string possibleAccuse in possibleAccuses.Keys)
                { 

                }
            }

            return string.Format("accuse {0}", possibleAccuses.FirstOrDefault(x => x.Value == possibleAccuses.Values.Max()).Key);
        }

        public string killRound()
        {
            Dictionary<String, int> possibleKills = infoSet.getPossibleKills();

            for (int i = 0; i < N; i++)
            {
                string talkSample = infoSet.killSample();
                RolloutGame game;
                int accuseUtility;

                foreach (string possibleKill in possibleKills.Keys)
                {


                }
            }

            return string.Format("kill {0}", possibleKills.FirstOrDefault(x => x.Value == possibleKills.Values.Max()).Key);
        }

        public string healRound()
        {
            Dictionary<String, int> possibleHeals = infoSet.getPossibleHeals();

            for (int i = 0; i < N; i++)
            {
                string talkSample = infoSet.healSample();
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
    }
}
