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
        private string playerName;

        //Dummy agent
        Random rnd;

        public InformationSet(string playerName)
        {
            players = new List<string>();
            rnd = new Random(Guid.NewGuid().GetHashCode());

            talks = new List<string>(){ "I believe", "I'm sure", "I'm not", "Player is" };
            this.playerName = playerName;
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

        public List<KeyValuePair<string,string>> accuseSample()
        {
            List<KeyValuePair<string, string>> accuseSample = new List<KeyValuePair<string, string>>(players.Count);

            //Lets infer first information - Player will not accuse himself
            foreach (string player in players)
            {
                string playerToBeAccused = string.Empty;
                do
                {
                    playerToBeAccused = players[rnd.Next(players.Count)];
                } while (playerToBeAccused == player);

                accuseSample.Add(new KeyValuePair<string, string>(player, playerToBeAccused));
            }
            return accuseSample;
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

        public Dictionary<String, int> getPossibleTalks()
        {
            return talks.ToDictionary(x => x, x => 0);
        }

        public Dictionary<String, int> getPossibleAccuses()
        {
            return players.Select(x => x).Where(x => x != playerName).ToDictionary(x => x , x => 0);
        }

        public Dictionary<String, int> getPossibleKills()
        {
            return players.Select(x => x).Where(x => x != playerName).ToDictionary(x => x, x => 0);
        }

        public Dictionary<String, int> getPossibleQuestions()
        {
            return players.Select(x => x).Where(x => x != playerName).ToDictionary(x => x, x => 0);
        }

        public Dictionary<String, int> getPossibleHeals()
        {
            return players.Select(x => x).Where(x => x != playerName).ToDictionary(x => x, x => 0);
        }
    }
}
