using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    public static class PlayerAbstractFactory
    {

        public static Player CreatePlayer(string name)
        {
            return new Werewolf();
        }
    }
}
