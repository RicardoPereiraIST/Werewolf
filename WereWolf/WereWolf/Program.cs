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
            Console.Write("How many human players will play : ");
            int playerNumber = int.Parse(Console.ReadLine());

            GameManager gameManager = new GameManager();
            Console.WriteLine(gameManager.StartGame(playerNumber));
            Console.WriteLine(gameManager.playRound());
            Console.ReadLine();
        }
    }
}
