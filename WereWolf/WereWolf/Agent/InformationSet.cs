using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    public class InformationSet
    {
        private List<String> players;
        private Dictionary<String, List<String>> accusedPlayers;

        private List<String> talks;

        //Dummy agent
        Random rnd;

        public InformationSet()
        {
            players = new List<string>();
            rnd = new Random(Guid.NewGuid().GetHashCode());

            talks = new List<string>(){ "I believe", "I'm sure", "I'm not", "Player is" };
        }

        public void addAccusePlay(String playerName, String accusedName)
        {

        }

        public void addKillPlay(String playerName)
        {
            players.Remove(playerName);
        }

        public void setPlayersList(List<String> p)
        {
            players = p;
        }

        public string accuseSample()
        {
            return players[rnd.Next(players.Count)];
        }
        public string killSample()
        {
            return players[rnd.Next(players.Count)];
        }
        public string talkSample()
        {
            return talks[rnd.Next(talks.Count)];
        }
        public string healSample()
        {
            return players[rnd.Next(players.Count)];
        }

        public string questionSample()
        {
            return players[rnd.Next(players.Count)];
        }

    }
}
