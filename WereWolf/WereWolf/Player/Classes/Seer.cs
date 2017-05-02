using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    public class Seer : Character
    {
        private bool dead;

        public Seer()
        {
            dead = false;
        }

        public bool canHeal()
        {
            return false;
        }

        public bool canKill()
        {
            return false;
        }

        public bool canQuestion()
        {
            return true;
        }

        public bool isDead()
        {
            return dead;
        }

        public bool isNightPlayer()
        {
            return true;
        }

        public void kill()
        {
            dead = true;
        }
    }
}
