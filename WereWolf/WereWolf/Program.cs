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
            Console.WriteLine("WELCOME TO THE WEREWOLF GAME");
            Console.Write("Is a human playing? (Y/N) : ");
            bool isPlayerPlaying = Console.ReadLine().Equals("Y");

            if(isPlayerPlaying) Console.Write("What is your desired name? : ");
            string playerName = Console.ReadLine();

            GameManager gameManager = new GameManager();
            Console.WriteLine(gameManager.StartGame(isPlayerPlaying, playerName));
            do
            {
                gameManager.playRound();
            } while (!gameManager.isGameOver());

            Console.ReadLine();
        }
    }
}
