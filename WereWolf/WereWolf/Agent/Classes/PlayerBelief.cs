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
            trustRate = 10;
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

        public int getMaxPercentage()
        {
            return percents.FirstOrDefault(x => x.Value == percents.Values.Max()).Value;
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

        public void updateBeliefsByAccuses(Dictionary<String, List<String>> accusedPlayers, Dictionary<String, PlayerBelief> beliefsPerPlayer)
        {
            List<String> accusedByPlayer = accusedPlayers[playerName];

            //If player don't acusses a lot of players that we consider to be a werewolf he has a larger possibility
            //of being a werewolf.
            foreach (KeyValuePair<String, PlayerBelief> playerBelief in beliefsPerPlayer)
            {
                if (playerBelief.Value.isCertainRole()) continue;

                if (!accusedByPlayer.Contains(playerBelief.Key) && playerBelief.Value.getRole().Equals("Werewolf"))
                {
                    setRole("Werewolf", getPercentOfRole("Werewolf") + 10);
                }
            }

            //Depending on the accuse of the player and his believed role we will update our beliefs.
            String lastAccuse = accusedByPlayer.Last();

            if (beliefsPerPlayer.ContainsKey(lastAccuse))
            {
                PlayerBelief playerBelief = beliefsPerPlayer[lastAccuse];
                if (!playerBelief.isCertainRole())
                {
                    if (getRole().Equals("Werewolf"))
                    {
                        playerBelief.setRole("Doctor", playerBelief.getPercentOfRole("Doctor") + 5);
                        playerBelief.setRole("Seer", playerBelief.getPercentOfRole("Seer") + 10);
                        playerBelief.setRole("Villager", playerBelief.getPercentOfRole("Villager") + 2);
                    }

                    if (getRole().Equals("Seer"))
                    {
                        playerBelief.setRole("Werewolf", playerBelief.getPercentOfRole("Werewolf") + 10);
                    }
                    else if (!playerBelief.Equals("Werewolf"))
                    {
                        playerBelief.setRole("Werewolf", playerBelief.getPercentOfRole("Werewolf") + 5);
                    }
                }
            }

            clampRoles();
            clampTrust();
        }

        public void updateBeliefsByTalk(Dictionary<String, PlayerBelief> beliefsPerPlayer)
        {
            if (getLog().Count == 0) return;
            Dictionary<String, String> log = getLog().Last();
            string name = log.Keys.ElementAt(0);
            string role = log.Values.ElementAt(0);

            if (!name.Equals(playerName) && beliefsPerPlayer.ContainsKey(name))
            {
                if (beliefsPerPlayer[name].isCertainRole() && !beliefsPerPlayer[name].getRole().Item1.Equals(role))
                {
                    trustRate -= 10;
                }
                else if (beliefsPerPlayer[name].isCertainRole() && beliefsPerPlayer[name].getRole().Item1.Equals(role))
                {
                    trustRate += 10;
                    setRole("Seer", getPercentOfRole("Seer") + 30);
                }
                else if(!beliefsPerPlayer[name].isCertainRole())
                {
                    beliefsPerPlayer[name].setRole(role, beliefsPerPlayer[name].getPercentOfRole(role) + trustRate);
                    beliefsPerPlayer[name].clampRoles();
                }
            }

            clampRoles();
            clampTrust();
        }

        public void clampRoles()
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

        public void clampTrust()
        {
            if (trustRate > 100)
                trustRate = 100;
            if (trustRate < 0)
                trustRate = 0;
        }
    }
}
