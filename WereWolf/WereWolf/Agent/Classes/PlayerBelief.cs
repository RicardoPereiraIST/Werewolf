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

        //PlayerName - PlayerRole
        private List<Dictionary<String, String>> log;

        public PlayerBelief()
        {
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

        public String getRole()
        {
            float max = percents.FirstOrDefault(x => x.Value == percents.Values.Max()).Value;
            if (max == 0)
                return "Villager";
            return percents.FirstOrDefault(x => x.Value == percents.Values.Max()).Key;
        }


    }
}
