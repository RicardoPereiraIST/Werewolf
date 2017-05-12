using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WereWolf.Nodes;

namespace WereWolf
{
    public class InformationSet
    {
        private List<String> players;
        private Dictionary<String, List<String>> accusedPlayers;
        private List<String> savedPeople;

        private Dictionary<String, KeyValuePair<String, int>> roleBeliefs;
        private string playerName;

        private List<string> friends;

        private Dictionary<String, PlayerBelief> beliefsPerPlayer;

        //Dummy agent
        Random rnd;

        public InformationSet(string playerName)
        {
            players = new List<string>();
            rnd = new Random(Guid.NewGuid().GetHashCode());
            friends = new List<string>();
            roleBeliefs = new Dictionary<String, KeyValuePair<String, int>>();
            accusedPlayers = new Dictionary<String, List<String>>();
            beliefsPerPlayer = new Dictionary<string, PlayerBelief>();
            savedPeople = new List<string>();
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

            beliefsPerPlayer[playerName].addRole(roleName);
        }

        public void addKillPlay(String playerName)
        {
            players.Remove(playerName);
            beliefsPerPlayer.Remove(playerName);
        }

        public void addFriend(string friend)
        {
            friends.Add(friend);
            roleBeliefs.Add(friend, new KeyValuePair<string, int>("Werewolf", 100));
            beliefsPerPlayer[friend].addRole("Werewolf");
        }

        public void addTalk(string talker, string playerName, string role)
        {
            beliefsPerPlayer[talker].addLog(playerName, role);
        }

        public void addSave(string playerName)
        {
            if(!savedPeople.Contains(playerName))
                savedPeople.Add(playerName);
        }

        public void updateBeliefs()
        {
            foreach(String player in beliefsPerPlayer.Keys)
                if(!player.Equals(playerName))
                    beliefsPerPlayer[player].updateBeliefs(players, accusedPlayers, savedPeople, beliefsPerPlayer);
        }

        public void setPlayersList(List<String> p)
        {
            players = new List<String>(p);
            foreach (String name in players)
            {
                if(!name.Equals(playerName))
                    beliefsPerPlayer.Add(name, new PlayerBelief(name));
            }
        }

        public List<PlayerNode> Sample()
        {
            //TODO
            //Update beliefs based on accuses
            List<PlayerNode> accuseSample = new List<PlayerNode>(players.Count);

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
                        accuseSample.Add(new RuleBasedNode(player, percentageRole.Key, this));
                        isRoleDecided = true;
                    }
                }
                if(!isRoleDecided)
                {
                    int randomNumber = rnd.Next(4);
                    if (randomNumber == 1)
                    {
                        accuseSample.Add(new RuleBasedNode(player , "Villager", this));
                    }
                    if (randomNumber == 2)
                    {
                        accuseSample.Add(new RuleBasedNode(player, "Seer", this));
                    }
                    if (randomNumber == 3)
                    {
                        accuseSample.Add(new RuleBasedNode(player, "Doctor", this));
                    }
                    if (randomNumber == 0)
                    {
                        accuseSample.Add(new RuleBasedNode(player, "Werewolf", this));
                    }
                }
            }
            return accuseSample;
        }

        public string ruledBasedAccuse()
        {
            List<String> accuseList = players.Where(p => p != playerName).ToList();
            if (accuseList.Count > 0)
                return string.Format("accuse {0}", accuseList[rnd.Next(accuseList.Count)]);
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
            return players.Select(x => x).Where(x => x != playerName && !friends.Contains(x)).ToDictionary(x => string.Format("accuse {0}",x) , x => 0);
        }

        public Dictionary<String, int> getPossibleKills()
        {
            return players.Select(x => x).Where(x => x != playerName && !friends.Contains(x)).ToDictionary(x => string.Format("kill {0}", x), x => 0);
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
