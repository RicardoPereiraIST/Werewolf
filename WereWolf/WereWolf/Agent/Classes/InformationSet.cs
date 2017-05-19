using System;
using System.Collections.Generic;
using System.Linq;
using WereWolf.General;
using WereWolf.Nodes;

namespace WereWolf
{
    public class InformationSet
    {
        private List<String> players;
        private Dictionary<String, List<String>> accusedPlayers;
        private Dictionary<String, int> playersLeft;
        private List<String> savedPeople;
        private string playerName;
        private string playerRole;
        private List<string> friends;
        private Dictionary<String, PlayerBelief> beliefsPerPlayer;
        private bool liar;

        //Dummy agent
        Random rnd;

        public InformationSet(string playerName, string playerRole, bool isLiar)
        {
            players = new List<string>();
            rnd = new Random(Guid.NewGuid().GetHashCode());
            friends = new List<string>();
            accusedPlayers = new Dictionary<String, List<String>>();
            beliefsPerPlayer = new Dictionary<string, PlayerBelief>();
            savedPeople = new List<string>();
            this.playerName = playerName;
            this.playerRole = playerRole;
            playersLeft = new Dictionary<string, int>();
            playersLeft.Add("Werewolf", Constants.WEREWOLF_NUMBER);
            playersLeft.Add("Seer", Constants.SEER_NUMBER);
            playersLeft.Add("Doctor", Constants.DOCTOR_NUMBER);
            playersLeft.Add("Villager", Constants.VILLAGER_NUMBER);
            liar = isLiar;
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

            if (playerName == this.playerName) return;
            beliefsPerPlayer[playerName].updateBeliefsByAccuses(accusedPlayers, beliefsPerPlayer);
        }

        public void addSeerAnswer(String playerName, String roleName)
        {
            beliefsPerPlayer[playerName].addRole(roleName);
        }

        public void addKillPlay(String playerName)
        {
            players.Remove(playerName);
        }

        public void addFriend(string friend)
        {
            friends.Add(friend);
            beliefsPerPlayer[friend].addRole("Werewolf");
        }

        public void reinitializeBeliefs()
        {
            foreach (PlayerBelief belief in beliefsPerPlayer.Values)
            {
                belief.reinitializeRoles();
            }

            friends.Clear();
            players.Clear();
            accusedPlayers.Clear();
            playersLeft.Clear();
            savedPeople.Clear();

            playersLeft.Add("Werewolf", Constants.WEREWOLF_NUMBER);
            playersLeft.Add("Seer", Constants.SEER_NUMBER);
            playersLeft.Add("Doctor", Constants.DOCTOR_NUMBER);
            playersLeft.Add("Villager", Constants.VILLAGER_NUMBER);
        }

        public void addTalk(string talker, string playerName, string role)
        {
            if (players.Contains(playerName) && players.Contains(talker))
            {
                beliefsPerPlayer[talker].addLog(playerName, role);
                beliefsPerPlayer[talker].updateBeliefsByTalk(beliefsPerPlayer);
            }
            if(playerName == this.playerName && role == playerRole)
            {
                beliefsPerPlayer[talker].addTrustRate(10);
                beliefsPerPlayer[talker].setRole("Seer", beliefsPerPlayer[talker].getPercentOfRole("Seer") + beliefsPerPlayer[talker].getTrustRate());
            }
        }

        public void addSave(string playerName)
        {
            if(!savedPeople.Contains(playerName))
                savedPeople.Add(playerName);

            if (playerName == this.playerName) return;

            beliefsPerPlayer[playerName].setRole("Werewolf", 0);

            foreach (KeyValuePair<String, PlayerBelief> belief in beliefsPerPlayer)
            {
                foreach (Dictionary<String, String> log in belief.Value.getLog())
                {
                    if (log.Keys.ElementAt(0) == playerName)
                    {
                        if (log.Values.ElementAt(0).Equals("Werewolf"))
                        {
                            belief.Value.addTrustRate(-5);
                        }
                    }
                    belief.Value.clampTrust();
                }
            }
        }

        public void addRole(String playerName, String playerRole)
        {
            playersLeft[playerRole] = playersLeft[playerRole]-1;
            if (beliefsPerPlayer.ContainsKey(playerName))
            {
                beliefsPerPlayer[playerName].addRole(playerRole);
            }

            if (this.playerName == playerName) return;

            List<Dictionary<String, String>> logs = beliefsPerPlayer[playerName].getLog();
            if(playerRole.Equals("Seer"))
            {
                foreach (Dictionary<String, String> log in logs)
                {
                    if (log.Keys.ElementAt(0) == this.playerName) continue;
                    beliefsPerPlayer[log.Keys.ElementAt(0)].setRole(log.Values.ElementAt(0), 
                        beliefsPerPlayer[log.Keys.ElementAt(0)].getPercentOfRole(log.Values.ElementAt(0)) + beliefsPerPlayer[playerName].getTrustRate());
                }
            }

            if (playerRole.Equals("Werewolf"))
            {
                if (accusedPlayers.ContainsKey(playerName))
                {
                    List<String> playerNames = accusedPlayers[playerName];
                    foreach (Dictionary<String, String> log in logs)
                    {
                        if (log.Keys.ElementAt(0) == this.playerName) continue;
                        playerNames.Remove(log.Keys.ElementAt(0));
                    }

                    foreach (string player in playerNames)
                    {
                        if (player == this.playerName) continue;
                        beliefsPerPlayer[player].setRole("Werewolf", beliefsPerPlayer[player].getPercentOfRole("Werewolf") + 15);
                        beliefsPerPlayer[player].clampRoles();
                    }
                }
            }

            foreach (KeyValuePair<String, PlayerBelief> belief in beliefsPerPlayer)
            {
                foreach (Dictionary<String, String> log in belief.Value.getLog())
                {
                    if(log.Keys.ElementAt(0) == playerName)
                    {
                        if(log.Values.ElementAt(0).Equals(playerRole))
                        {
                            belief.Value.addTrustRate(10);
                        }
                        else
                        {
                            belief.Value.addTrustRate(-15);
                        }
                    }
                    belief.Value.clampTrust();
                }
            }
        }

        public void setPlayersList(List<String> p)
        {
            players = new List<String>(p);
            foreach (String name in players)
            {
                if(!name.Equals(playerName) && !beliefsPerPlayer.ContainsKey(name))
                    beliefsPerPlayer.Add(name, new PlayerBelief(name));
            }
        }

        public List<PlayerNode> Sample(string playerRole)
        {
            //TODO
            //Update beliefs based on accuses
            List<PlayerNode> accuseSample = new List<PlayerNode>(players.Count);
            Dictionary<string, int> playersToSample = new Dictionary<string, int>(playersLeft);
            List<String> noRolePlayers = new List<string>();

            List<KeyValuePair<String,PlayerBelief>> listOrderByBelief = beliefsPerPlayer.OrderByDescending(x => x.Value.getMaxPercentage()).ToList();
            playersToSample[playerRole] = playersToSample[playerRole] - 1;

            //Lets infer first information - Player will not accuse himself
            foreach (KeyValuePair<String, PlayerBelief> player in listOrderByBelief)
            {
                PlayerBelief playerBelief;
                if (!players.Contains(player.Key)) continue;

                if (beliefsPerPlayer.TryGetValue(player.Key, out playerBelief))
                {
                    int percentageSuccess = rnd.Next(100);
                    Tuple<string,int> roleBelief = playerBelief.getRole();
                    if (percentageSuccess <= roleBelief.Item2)
                    {
                        if (playersToSample[roleBelief.Item1] > 0)
                        {
                            accuseSample.Add(new RuleBasedNode(player.Key, roleBelief.Item1, this));
                            playersToSample[roleBelief.Item1] = playersToSample[roleBelief.Item1] - 1;
                            continue;
                        }
                    }
                }
                noRolePlayers.Add(player.Key);
            }

            foreach(string player in noRolePlayers)
            {
                KeyValuePair<string, int> playersToCreate = playersToSample.Where(x => x.Value > 0).ElementAt(0);
                accuseSample.Add(new RuleBasedNode(player, playersToCreate.Key, this));
                playersToSample[playersToCreate.Key] = playersToSample[playersToCreate.Key] - 1;
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
            List<KeyValuePair<String, PlayerBelief>> playersNotCertain = beliefsPerPlayer.Where(x => !x.Value.isCertainRole()).ToList();
            return playersNotCertain[rnd.Next(playersNotCertain.Count)].Key;
        }

        public Dictionary<String, int> getPossibleTalks()
        {
            Dictionary<String, int> possibleTalks = new Dictionary<String, int>();
            if (beliefsPerPlayer.Count > 0)
            {
                foreach (KeyValuePair<string, PlayerBelief> playerBelief in beliefsPerPlayer)
                {
                    Tuple<string, int> belief = playerBelief.Value.getRole();
                    
                    if (!players.Contains(playerBelief.Key) || friends.Contains(playerBelief.Key)) continue;

                    if (belief.Item2 >= 100 && !liar)
                        possibleTalks.Add(string.Format("talk {0} is a {1}", playerBelief.Key, belief.Item1), 0);
                    else if (liar && belief.Item2 < 100 && rnd.Next(2) == 1)
                    {
                        possibleTalks.Add(string.Format("talk {0} is a {1}", playerBelief.Key, liarTalk()), 0);
                        break;
                    }
                }
            }

            if(possibleTalks.Count == 0) possibleTalks.Add("talk I don't know", 0);

            return possibleTalks;
        }

        public Dictionary<String, int> getPossibleAccuses()
        {
            Dictionary<String, int> possibleAccuses = players.Select(x => x).Where(x => x != playerName && !friends.Contains(x)).ToDictionary(x => string.Format("accuse {0}", x), x => 0);
            possibleAccuses.Add("pass", 0);
            return possibleAccuses;
        }

        private string liarTalk()
        {
            int randomRole = rnd.Next(4);
            switch(randomRole)
            {
                case 0:
                    return "Werewolf";
                case 1:
                    return "Seer";
                case 2:
                    return "Doctor";
                case 3:
                    return "Villager";
                default:
                    return string.Empty;
            }
        }

        public Dictionary<String, int> getPossibleKills()
        {
            return players.Select(x => x).Where(x => x != playerName && !friends.Contains(x)).ToDictionary(x => string.Format("kill {0}", x), x => 0);
        }

        public Dictionary<String, int> getPossibleQuestions()
        {
            return players.Select(x => x).Where(x => x != playerName).ToDictionary(x => string.Format("question {0}", x), x => 0);
        }

        public Dictionary<String, int> getPossibleHeals()
        {
            return players.Select(x => x).ToDictionary(x => string.Format("heal {0}", x), x => 0);
        }
    }
}
