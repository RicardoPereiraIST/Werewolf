using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    class PlayerBelief
    {
        private Dictionary<String, float> percents;
        private bool liar;
        private string playerName;

        //PlayerName - PlayerRole
        private List<Dictionary<String, String>> log;

        public PlayerBelief(String playerName)
        {
            this.playerName = playerName;
            log = new List<Dictionary<String, String>>();
            percents = new Dictionary<String, float>();
            percents.Add("Werewolf", 0);
            percents.Add("Seer", 0);
            percents.Add("Doctor", 0);
            percents.Add("Villager", 0);
        }

        public List<Dictionary<String, String>> getLog()
        {
            return log;
        }

        //FIXME
        public void addLog(String playerName, String playerRole)
        {
            string actualValue;
            foreach (Dictionary<String, String> talk in log)
            {
                if (talk.TryGetValue(playerName, out actualValue) && actualValue.Equals(playerRole))
                    return;
            }


            Dictionary<String, String> newDict = new Dictionary<String, String>();
            newDict.Add(playerName, playerRole);
            log.Add(newDict);
        }

        public bool isLiar()
        {
            return liar;
        }

        public float getPercentOfRole(String role)
        {
            return percents[role];
        }

        public Tuple<String, float> getRole()
        {
            float max = percents.FirstOrDefault(x => x.Value == percents.Values.Max()).Value;
            Tuple<String, float> tuple;
            if (max == 0)
            {
                tuple = new Tuple<String, float>("Villager", 0);
            }
            else
            {
                tuple = new Tuple<String, float>(percents.FirstOrDefault(x => x.Value == percents.Values.Max()).Key,max);
            }
            return tuple;
        }

        public void addRole(String role)
        {
            percents[role] = 100;
        }

        public void setRole(String role, float value)
        {
            percents[role] = value;
        }

        public void reinitializeRoles()
        {
            percents = new Dictionary<String, float>();
            percents.Add("Werewolf", 0);
            percents.Add("Seer", 0);
            percents.Add("Doctor", 0);
            percents.Add("Villager", 0);
        }

        public void updateBeliefs(List<String> players, Dictionary<String, List<String>> accusedPlayers, List<String> savedPeople, Dictionary<String, PlayerBelief> beliefsPerPlayer)
        {

            if (accusedPlayers.ContainsKey(playerName))
            {
                List<String> accusedByPlayer = accusedPlayers[playerName];
                foreach (String name in accusedByPlayer)
                {
                    if (beliefsPerPlayer.ContainsKey(name))
                    {
                        if (!players.Contains(name) && !beliefsPerPlayer[name].getRole().Item1.Equals("Werewolf"))
                        {
                            percents["Werewolf"] += 10;
                        }
                    }
                }
            }

            foreach(String name in beliefsPerPlayer.Keys)
            {
                if (!name.Equals(playerName))
                {
                    Tuple<String, float> tuple = beliefsPerPlayer[name].getRole();
                    foreach (Dictionary<String, String> talk in log) {
                        if (talk.Keys.First().Equals(name)) {
                            if (tuple.Item2 >= 80)
                            {
                                if (talk[name] != tuple.Item1)
                                {
                                    liar = true;
                                    break;
                                }
                                else
                                {
                                    liar = false;
                                }
                            }
                        }
                    }
                }
            }

            //SEER AND LIARS
            foreach (String name in beliefsPerPlayer.Keys)
            {
                if (!name.Equals(playerName))
                {
                    Tuple<String, float> tuple = beliefsPerPlayer[name].getRole();
                    if(tuple.Item1.Equals("Seer"))
                    {
                        if (!beliefsPerPlayer[name].isLiar())
                        {
                            foreach (Dictionary<String, String> talk in beliefsPerPlayer[name].getLog())
                            {
                                if (beliefsPerPlayer.ContainsKey(talk.Keys.First()))
                                {
                                    beliefsPerPlayer[talk.Keys.First()].addRole(talk.Values.First());
                                }
                            }

                            if (accusedPlayers.ContainsKey(name))
                            {
                                List<String> accusedByPlayer = accusedPlayers[name];
                                foreach (String n in accusedByPlayer)
                                {
                                    if (beliefsPerPlayer.ContainsKey(n))
                                    {
                                        beliefsPerPlayer[n].setRole("Werewolf", 100);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Dictionary<String, String> talk in beliefsPerPlayer[name].getLog())
                            {
                                if (beliefsPerPlayer.ContainsKey(talk.Keys.First()))
                                {
                                    beliefsPerPlayer[talk.Keys.First()].setRole(talk.Values.First(), 0);
                                }
                            }

                            if (accusedPlayers.ContainsKey(name))
                            {
                                List<String> accusedByPlayer = accusedPlayers[name];
                                foreach (String n in accusedByPlayer)
                                {
                                    if (beliefsPerPlayer.ContainsKey(n))
                                    {
                                        beliefsPerPlayer[n].setRole("Werewolf", 0);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!beliefsPerPlayer[name].isLiar())
                        {
                            foreach (Dictionary<String, String> talk in beliefsPerPlayer[name].getLog())
                            {
                                if (beliefsPerPlayer.ContainsKey(talk.Keys.First()))
                                {
                                    beliefsPerPlayer[talk.Keys.First()].setRole(talk.Values.First(), beliefsPerPlayer[talk.Keys.First()].getPercentOfRole(talk.Values.First()) + 10);
                                }
                            }

                            if (accusedPlayers.ContainsKey(name))
                            {
                                List<String> accusedByPlayer = accusedPlayers[name];
                                foreach (String n in accusedByPlayer)
                                {
                                    if (beliefsPerPlayer.ContainsKey(n))
                                    {
                                        beliefsPerPlayer[n].setRole("Werewolf", beliefsPerPlayer[n].getPercentOfRole("Werewolf") + 10);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Dictionary<String, String> talk in beliefsPerPlayer[name].getLog())
                            {
                                if (beliefsPerPlayer.ContainsKey(talk.Keys.First()))
                                {
                                    beliefsPerPlayer[talk.Keys.First()].setRole(talk.Values.First(), beliefsPerPlayer[talk.Keys.First()].getPercentOfRole(talk.Values.First()) - 10);
                                }
                            }

                            if (accusedPlayers.ContainsKey(name))
                            {
                                List<String> accusedByPlayer = accusedPlayers[name];
                                foreach (String n in accusedByPlayer)
                                {
                                    if (beliefsPerPlayer.ContainsKey(n))
                                    {
                                        beliefsPerPlayer[n].setRole("Werewolf", beliefsPerPlayer[n].getPercentOfRole("Werewolf") - 10);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (savedPeople.Contains(playerName))
            {
                percents["Werewolf"] = 0;
            }

            clampRoles();
        }

        private void clampRoles()
        {
            Dictionary<String, float> temp = new Dictionary<string, float>(percents);
            foreach (String role in percents.Keys)
            {
                if (temp[role] < 0)
                    temp[role] = 0;
                if (temp[role] > 100)
                    temp[role] = 100;
            }
            percents = temp;
        }
    }
}
