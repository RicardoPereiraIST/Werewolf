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

            talks = new List<string>(){ "The player {0} is a werewolf","I don't know", "The player {0} is not a werewolf"};
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
            players = new List<String>(p);
        }

        public List<Player> accuseSample()
        {
            List<Player> accuseSample = new List<Player>(players.Count);

            //Lets infer first information - Player will not accuse himself
            foreach (string player in players)
            {
                if (player.Equals(playerName)) continue;
                int randomNumber = rnd.Next(4);
                if (randomNumber == 1)
                {
                    accuseSample.Add(new Player("Villager", false, player, true));
                }
                if (randomNumber == 2)
                {
                    accuseSample.Add(new Player("Seer", false, player, true));
                }
                if (randomNumber == 3)
                {
                    accuseSample.Add(new Player("Doctor", false, player, true));
                }
                if (randomNumber == 0)
                {
                    accuseSample.Add(new Player("Werewolf", false, player, true));
                }
            }
            return accuseSample;
        }

        public string ruledBasedAccuse()
        {
            List<String> accuseList = players.Where(p => p != playerName).ToList();
            if (accuseList.Count > 0)
                return accuseList[rnd.Next(accuseList.Count)];
            else return string.Empty;
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
