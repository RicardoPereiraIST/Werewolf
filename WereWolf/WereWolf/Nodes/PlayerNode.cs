﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf.Nodes
{
    public abstract class PlayerNode
    {
        public InformationSet InfoSet;
        public string playerName;
        public string charName;
        public bool playerDead;

        public PlayerNode(string playerName, string charName, InformationSet infoSet)
        {
            this.playerName = playerName;
            this.charName = charName;
            this.InfoSet = infoSet;
            playerDead = false;
        }

        public abstract int PlayGame(RolloutGame game, int alpha, int beta, int depthLimit, string command = "");

        public abstract PlayerNode Copy();
    }
}
