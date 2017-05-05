using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    public class Agent
    {
        private List<String> players;

        public Agent()
        {
            players = new List<String>();
        }

        public void setPlayersList(List<String> p)
        {
            players = p;
        }

        public void killPlayer(string p)
        {
            players.Remove(p);
        }

        public void printList()
        {
            foreach(string s in players)
                Console.WriteLine(s);
        }
    }
}
