using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    public class Agent
    {
        private List<String> players;

        //Dummy agent
        Random rnd;

        public Agent()
        {
            players = new List<String>();
            rnd = new Random(Guid.NewGuid().GetHashCode());
        }

        public void setPlayersList(List<String> p)
        {
            players = p;
        }

        public void killPlayer(string p)
        {
            players.Remove(p);
        }

        public string talkRound()
        {
            return string.Empty;
        }

        public string accuseRound()
        {
            return string.Format("accuse {0}", players[rnd.Next(players.Count)]);
        }

        public string killRound()
        {
            return string.Format("kill {0}", players[rnd.Next(players.Count)]);
        }

        public string healRound()
        {
            return string.Format("heal {0}", players[rnd.Next(players.Count)]);
        }

        public string questionRound()
        {
            return string.Format("question {0}", players[rnd.Next(players.Count)]);
        }

        public void accusePlayedRound(string playerName, string accusedPlayerName)
        {
            return;
        }

        public void printList()
        {
            foreach(string s in players)
                Console.WriteLine(s);
        }
    }
}
