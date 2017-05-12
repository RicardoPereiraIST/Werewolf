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

        void addFriend(string friend);

        void addTalk(string talker, string playerName, string role);

        void addSave(string playerName);

        string talkRound();

        string accuseRound();

        string killRound();

        string healRound();

        string questionRound();

        void accusePlayedRound(string playerName, string accusedPlayerName);

        void seerQuestion(string playerName, string roleName);

        void updateBeliefs();
    }
}
