using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    public interface Agent
    {
        void setPlayersList(List<String> p);

        void killPlayer(string playerName);

        string talkRound();

        string accuseRound();

        string killRound();

        string healRound();

        string questionRound();

        void accusePlayedRound(string playerName, string accusedPlayerName);
    }
}
