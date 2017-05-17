using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    class PlayerBelief
    {
        private Dictionary<String, int> percents;
        private int trustRate;
        private string playerName;
        private bool certainRole;

        //PlayerName - PlayerRole
        private List<Dictionary<String, String>> log;

        public PlayerBelief(String playerName)
        {
            this.playerName = playerName;
            trustRate = 50;
            log = new List<Dictionary<String, String>>();
            certainRole = false;
            percents = new Dictionary<String, int>();
            percents.Add("Werewolf", 0);
            percents.Add("Seer", 0);
            percents.Add("Doctor", 0);
            percents.Add("Villager", 0);
        }

        public bool isCertainRole()
        {
            return certainRole;
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

        public int getTrustRate()
        {
            return trustRate;
        }

        public void addTrustRate(int trust)
        {
            trustRate += trust;
        }

        public int getPercentOfRole(String role)
        {
            return percents[role];
        }

        public Tuple<String, int> getRole()
        {
            int max = percents.FirstOrDefault(x => x.Value == percents.Values.Max()).Value;
            Tuple<String, int> tuple;
            if (max == 0)
            {
                tuple = new Tuple<String, int>("Villager", 0);
            }
            else
            {
                tuple = new Tuple<String, int>(percents.FirstOrDefault(x => x.Value == percents.Values.Max()).Key,max);
            }
            return tuple;
        }

        public void addRole(String role)
        {
            certainRole = true;
            percents.Clear();
            percents.Add("Werewolf", 0);
            percents.Add("Seer", 0);
            percents.Add("Doctor", 0);
            percents.Add("Villager", 0);
            percents[role] = 100;
        }

        public void setRole(String role, int value)
        {
            percents[role] = value;
        }

        public void reinitializeRoles()
        {
            percents.Clear();
            percents.Add("Werewolf", 0);
            percents.Add("Seer", 0);
            percents.Add("Doctor", 0);
            percents.Add("Villager", 0);
            log.Clear();
            certainRole = false;
        }

        public void updateBeliefs(List<String> players, Dictionary<String, List<String>> accusedPlayers, List<String> savedPeople, Dictionary<String, PlayerBelief> beliefsPerPlayer)
        {

            ////Percentage of being a werewolf is larger if you're being accused by a possible non-werewolf.
            //if (accusedPlayers.ContainsKey(playerName))
            //{
            //    List<String> accusedByPlayer = accusedPlayers[playerName];
            //    foreach (String name in accusedByPlayer)
            //    {
            //        if (beliefsPerPlayer.ContainsKey(name))
            //        {
            //            if (!players.Contains(name) && !beliefsPerPlayer[name].getRole().Item1.Equals("Werewolf"))
            //            {
            //                percents["Werewolf"] += 10;
            //            }
            //        }
            //    }
            //}

            if (getLog().Count == 0) return;
            Dictionary<String, String> log = getLog().Last();
            string name = log.Keys.ElementAt(0);
            string role = log.Values.ElementAt(0);

            if (!name.Equals(playerName) && beliefsPerPlayer.ContainsKey(name))
            {
                if (beliefsPerPlayer[name].isCertainRole() && !beliefsPerPlayer[name].getRole().Item1.Equals(role))
                {
                    trustRate -= 25;
                }
                else if (beliefsPerPlayer[name].isCertainRole() && beliefsPerPlayer[name].getRole().Item1.Equals(role))
                {
                    trustRate += 20;
                    setRole("Seer", getPercentOfRole("Seer") + 25);
                }
                else
                {
                    beliefsPerPlayer[name].setRole(role, beliefsPerPlayer[name].getPercentOfRole(role) + trustRate);
                }
            }


            //SEER AND LIARS
            //foreach (String name in beliefsPerPlayer.Keys)
            //{
            //    if (!name.Equals(playerName))
            //    {
            //        Tuple<String, int> tuple = beliefsPerPlayer[name].getRole();
            //        if(tuple.Item1.Equals("Seer"))
            //        {
            //            if (!(beliefsPerPlayer[name].getTrustRate() > rnd.Next(100)))
            //            {
            //                foreach (Dictionary<String, String> talk in beliefsPerPlayer[name].getLog())
            //                {
            //                    if (beliefsPerPlayer.ContainsKey(talk.Keys.First()) && !beliefsPerPlayer[talk.Keys.First()].isCertainRole())
            //                    {
            //                        beliefsPerPlayer[talk.Keys.First()].addRole(talk.Values.First());
            //                    }
            //                }

            //                if (accusedPlayers.ContainsKey(name))
            //                {
            //                    List<String> accusedByPlayer = accusedPlayers[name];
            //                    foreach (String n in accusedByPlayer)
            //                    {
            //                        if (beliefsPerPlayer.ContainsKey(n) && !beliefsPerPlayer[n].isCertainRole())
            //                        {
            //                            beliefsPerPlayer[n].setRole("Werewolf", 100);
            //                        }
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                foreach (Dictionary<String, String> talk in beliefsPerPlayer[name].getLog())
            //                {
            //                    if (beliefsPerPlayer.ContainsKey(talk.Keys.First()) && !beliefsPerPlayer[talk.Keys.First()].isCertainRole())
            //                    {
            //                        beliefsPerPlayer[talk.Keys.First()].setRole(talk.Values.First(), 0);
            //                    }
            //                }

            //                if (accusedPlayers.ContainsKey(name))
            //                {
            //                    List<String> accusedByPlayer = accusedPlayers[name];
            //                    foreach (String n in accusedByPlayer)
            //                    {
            //                        if (beliefsPerPlayer.ContainsKey(n) && !beliefsPerPlayer[n].isCertainRole())
            //                        {
            //                            beliefsPerPlayer[n].setRole("Werewolf", 0);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (!(beliefsPerPlayer[name].getTrustRate() > rnd.Next(100)))
            //            {
            //                foreach (Dictionary<String, String> talk in beliefsPerPlayer[name].getLog())
            //                {
            //                    if (beliefsPerPlayer.ContainsKey(talk.Keys.First()) && !beliefsPerPlayer[talk.Keys.First()].isCertainRole())
            //                    {
            //                        beliefsPerPlayer[talk.Keys.First()].setRole(talk.Values.First(), beliefsPerPlayer[talk.Keys.First()].getPercentOfRole(talk.Values.First()) + 10);
            //                    }
            //                }

            //                if (accusedPlayers.ContainsKey(name))
            //                {
            //                    List<String> accusedByPlayer = accusedPlayers[name];
            //                    foreach (String n in accusedByPlayer)
            //                    {
            //                        if (beliefsPerPlayer.ContainsKey(n) && beliefsPerPlayer[n].getPercentOfRole("Werewolf") < rnd.Next(100) && !beliefsPerPlayer[n].isCertainRole())
            //                        {
            //                            beliefsPerPlayer[n].setRole("Werewolf", beliefsPerPlayer[n].getPercentOfRole("Werewolf") + 10);
            //                        }
            //                        else if(beliefsPerPlayer.ContainsKey(n) && !beliefsPerPlayer[n].isCertainRole())
            //                        {
            //                            beliefsPerPlayer[n].setRole("Villager", beliefsPerPlayer[n].getPercentOfRole("Villager") + 10);
            //                            beliefsPerPlayer[n].setRole("Seer", beliefsPerPlayer[n].getPercentOfRole("Seer") + 5);
            //                            beliefsPerPlayer[n].setRole("Doctor", beliefsPerPlayer[n].getPercentOfRole("Doctor") + 5);
            //                            beliefsPerPlayer[n].setRole("Werewolf", beliefsPerPlayer[n].getPercentOfRole("Werewolf") - 10);
            //                        }
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                foreach (Dictionary<String, String> talk in beliefsPerPlayer[name].getLog())
            //                {
            //                    if (beliefsPerPlayer.ContainsKey(talk.Keys.First()) && !beliefsPerPlayer[talk.Keys.First()].isCertainRole())
            //                    {
            //                        beliefsPerPlayer[talk.Keys.First()].setRole(talk.Values.First(), beliefsPerPlayer[talk.Keys.First()].getPercentOfRole(talk.Values.First()) - 10);
            //                    }
            //                }

            //                if (accusedPlayers.ContainsKey(name))
            //                {
            //                    List<String> accusedByPlayer = accusedPlayers[name];
            //                    foreach (String n in accusedByPlayer)
            //                    {
            //                        if (beliefsPerPlayer.ContainsKey(n) && beliefsPerPlayer[n].getPercentOfRole("Werewolf") < rnd.Next(100) && !beliefsPerPlayer[n].isCertainRole())
            //                        {
            //                            beliefsPerPlayer[n].setRole("Werewolf", beliefsPerPlayer[n].getPercentOfRole("Werewolf") - 10);
            //                        }
            //                        else if (beliefsPerPlayer.ContainsKey(n) && !beliefsPerPlayer[n].isCertainRole())
            //                        {
            //                            beliefsPerPlayer[n].setRole("Villager", beliefsPerPlayer[n].getPercentOfRole("Villager") - 10);
            //                            beliefsPerPlayer[n].setRole("Seer", beliefsPerPlayer[n].getPercentOfRole("Seer") - 5);
            //                            beliefsPerPlayer[n].setRole("Doctor", beliefsPerPlayer[n].getPercentOfRole("Doctor") - 5);
            //                            beliefsPerPlayer[n].setRole("Werewolf", beliefsPerPlayer[n].getPercentOfRole("Werewolf") + 10);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //if (savedPeople.Contains(playerName))
            //{
            //    percents["Werewolf"] = 0;
            //}

            clampRoles();
        }

        private void clampRoles()
        {
            Dictionary<String, int> temp = new Dictionary<string, int>(percents);
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
