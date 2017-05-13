using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereWolf
{
    public class Program
    {
        static void Main(string[] args)
        {
            int numberOfGames = 0;
            Console.WriteLine("WELCOME TO THE WEREWOLF GAME");
            Console.Write("Is a human playing? (Y/N) : ");
            bool isPlayerPlaying = Console.ReadLine().Equals("Y");

            if(isPlayerPlaying) Console.Write("What is your desired name? : ");
            string playerName = isPlayerPlaying ? Console.ReadLine() : string.Empty;

            GameManager gameManager = new GameManager();
            Console.WriteLine(gameManager.StartGame(isPlayerPlaying, playerName));
			Console.WriteLine("Will start game at (N:15, No depth limit) :" + DateTime.Now);

            if (isPlayerPlaying)
            {
                List<Player> players = gameManager.getPlayers();
                bool isPlayerWolf = false;
                String role = "";

                foreach (Player player in players)
                {
                    if (player.getPlayerName().Equals(playerName))
                    {
                        role = player.getCharName();
                        break;
                    }
                }
                
                if(role.Equals("Werewolf"))
                    isPlayerWolf = true;

                Console.WriteLine("You are a " + role);
                Console.WriteLine("\nThe names of the players are:");

                foreach (Player player in players)
                {
                    if (isPlayerWolf && player.getCharName().Equals("Werewolf"))
                        Console.WriteLine(player.getPlayerName() + " - Werewolf");
                    else
                        Console.WriteLine(player.getPlayerName());
                }
                Console.WriteLine();
            }
            do
            {
                gameManager.playRound();
                if (gameManager.isGameOver())
                {
                    Console.WriteLine("Will start another game with same players (role belief reset) :" + DateTime.Now);
                    gameManager.ReinitializeGame();
                    numberOfGames++;
                }
            } while (numberOfGames<20);

            Console.ReadLine();
        }
    }
}
