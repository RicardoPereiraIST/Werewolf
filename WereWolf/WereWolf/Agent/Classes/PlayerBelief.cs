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

        //FIXME
        public void addLog(String playerName, String playerRole)
        {
            Dictionary<String, String> newDict = new Dictionary<String, String>();
            newDict.Add(playerName, playerRole);
            if(!log.Contains(newDict))
                log.Add(newDict);
        }

        public bool isLiar()
        {
            return liar;
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

        public void updateBeliefs(List<String> players, Dictionary<String, List<String>> accusedPlayers, List<String> savedPeople, Dictionary<String, PlayerBelief> beliefsPerPlayer)
        {
            if (savedPeople.Contains(playerName))
            {
                percents["Werewolf"] = 0;
            }

            foreach(String name in beliefsPerPlayer.Keys)
            {
                if (!name.Equals(playerName))
                {
                    Tuple<String, float> tuple = beliefsPerPlayer[name].getRole();
                    foreach (Dictionary<String, String> talk in log) {
                        if (talk.Keys.First().Equals(name)) {
                            if (tuple.Item2 == 100) {
                                if (talk[name] != tuple.Item1)
                                {
                                    liar = true;
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
        }
    }
}
