using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    public class Villager : Character
    {
        private bool dead;

        public Villager()
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
            return false;
        }

        public bool isDead()
        {
            return dead;
        }

        public bool isNightPlayer()
        {
            return false;
        }

        public void kill()
        {
            dead = true;
        }
        public void heal()
        {
            dead = false;
        }
    }
}
