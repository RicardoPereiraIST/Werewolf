using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    public class Werewolf : Character
    {
        private bool dead;

        public Werewolf()
        {
            dead = false;
        }

        public bool canHeal()
        {
            return false;
        }

        public bool canKill()
        {
            return true;
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
            return true;
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
