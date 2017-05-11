using System;
using System.Collections.Generic;
using System.Linq;

namespace WereWolf
{
    public class BasicAgent : Agent
    {
        private InformationSet infoSet;
        private const int N = 10;

        public BasicAgent(string playerName)
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

        public void addFriend(string friend)
        {
            infoSet.addFriend(friend);
        }

        public string talkRound()
        {
            //Value of the talks, every one has a 0 value in the beginning.
            Dictionary<String, int> possibleTalks = infoSet.getPossibleTalks();
            return string.Format("talk {0}", possibleTalks.FirstOrDefault(x => x.Value == possibleTalks.Values.Max()).Key);
        }

        public string accuseRound()
        {
            string accused = infoSet.ruledBasedAccuse();
            if(!string.IsNullOrEmpty(accused))
                return string.Format("accuse {0}", accused);
            return "pass";
        }

        public string killRound()
        {
            Dictionary<String, int> possibleKills = infoSet.getPossibleKills();

            return string.Format("kill {0}", infoSet.ruledBasedAccuse());
        }

        public string healRound()
        {
            Dictionary<String, int> possibleHeals = infoSet.getPossibleHeals();

            return string.Format("heal {0}", possibleHeals.FirstOrDefault(x => x.Value == possibleHeals.Values.Max()).Key);
        }

        public string questionRound()
        {
            Dictionary<String, int> possibleQuestions = infoSet.getPossibleQuestions();

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
