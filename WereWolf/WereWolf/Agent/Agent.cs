using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    public class Agent
    {
        InformationSet infoSet;

        public Agent()
        {
            infoSet = new InformationSet();
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
            return string.Format("talk {0}", infoSet.talkSample());
        }

        public string accuseRound()
        {
            return string.Format("accuse {0}", infoSet.accuseSample());
        }

        public string killRound()
        {
            return string.Format("kill {0}", infoSet.killSample());
        }

        public string healRound()
        {
            return string.Format("heal {0}", infoSet.healSample());
        }

        public string questionRound()
        {
            return string.Format("question {0}", infoSet.questionSample());
        }

        public void accusePlayedRound(string playerName, string accusedPlayerName)
        {
            infoSet.addAccusePlay(playerName, accusedPlayerName);
        }
    }
}
