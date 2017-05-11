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

        private Dictionary<String, KeyValuePair<String, int>> roleBeliefs;
        private string playerName;

        private List<string> friends;

        //Dummy agent
        Random rnd;

        public InformationSet(string playerName)
        {
            players = new List<string>();
            rnd = new Random(Guid.NewGuid().GetHashCode());
            friends = new List<string>();
            roleBeliefs = new Dictionary<String, KeyValuePair<String, int>>();
            accusedPlayers = new Dictionary<String, List<String>>();
            this.playerName = playerName;
        }

        public void addAccusePlay(String playerName, String accusedName)
        {
            //Add accused playing
            List<String> accusedList;
            if (accusedPlayers.TryGetValue(playerName, out accusedList))
            {
                accusedList.Add(accusedName);
                accusedPlayers[playerName] = accusedList;
            }
            else
            {
                accusedPlayers.Add(playerName, new List<string> { accusedName });
            }
        }

        public void addSeerAnswer(String playerName, String roleName)
        {
            KeyValuePair<string, int> percentageRole;

            if (roleBeliefs.TryGetValue(playerName, out percentageRole))
            {
                percentageRole = new KeyValuePair<string, int>(roleName, 100);
                roleBeliefs[playerName] = percentageRole;
            }
            else
            {
                roleBeliefs.Add(playerName, new KeyValuePair<string, int>(roleName, 100));
            }
        }

        public void addKillPlay(String playerName)
        {
            players.Remove(playerName);
        }

        public void addFriend(string friend)
        {
            friends.Add(friend);
        }

        public void setPlayersList(List<String> p)
        {
            players = new List<String>(p);
        }

        public List<Player> Sample()
        {
            //TODO
            //Update beliefs based on accuses
            List<Player> accuseSample = new List<Player>(players.Count);

            //Lets infer first information - Player will not accuse himself
            foreach (string player in players)
            {
                bool isRoleDecided = false;
                if (player.Equals(playerName)) continue;

                KeyValuePair<string, int> percentageRole;

                if (roleBeliefs.TryGetValue(player, out percentageRole))
                {
                    int percentageSuccess = rnd.Next(100);
                    if(percentageSuccess <= percentageRole.Value)
                    {
                        accuseSample.Add(new Player(percentageRole.Key, false, player, true));
                        isRoleDecided = true;
                    }
                }
                if(!isRoleDecided)
                {
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

        public string questionSample()
        {
            return players[rnd.Next(players.Count)];
        }

        public Dictionary<String, int> getPossibleTalks()
        {
            Dictionary<String, int> possibleTalks = new Dictionary<String, int>();
            if (roleBeliefs.Count > 0)
            {
                foreach (KeyValuePair<string, KeyValuePair<string, int>> role in roleBeliefs)
                {
                    possibleTalks.Add(string.Format("The player {0} is a {1}", role.Key, role.Value.Key), 0);
                }
            }
            else possibleTalks.Add("I don't know", 0);

            return possibleTalks;
        }

        public Dictionary<String, int> getPossibleAccuses()
        {
            return players.Select(x => x).Where(x => x != playerName && !friends.Contains(x)).ToDictionary(x => x , x => 0);
        }

        public Dictionary<String, int> getPossibleKills()
        {
            return players.Select(x => x).Where(x => x != playerName && !friends.Contains(x)).ToDictionary(x => x, x => 0);
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
