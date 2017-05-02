using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    public static class CharacterAbstractFactory
    {

        public static Character CreatePlayer(string name)
        {
            switch(name)
            {
                case "Seer":
                    return new Seer();

                case "Werewolf":
                    return new Werewolf();

                case "Doctor":
                    return new Doctor();

                default:
                    return new Villager();
            }

        }
    }
}
