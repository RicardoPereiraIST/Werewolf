using System.Collections.Generic;

namespace WereWolf
{
    public class GameManager
    {
        List<Player> players;

        GameManager()
        {
            players = new List<Player>();
        }

        public void StartGame(int numPlayers)
        {
            for(int i = 0; i <)
            players.Add(PlayerAbstractFactory.CreatePlayer("lol"));
        }
    }
}
