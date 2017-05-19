using System;
using System.Collections.Generic;
using WereWolf.General;

namespace WereWolf
{
    public class Program
    {

        private const int NUMBER_OF_GAMES = 50;

        static void Main(string[] args)
        {
            //LOG
            Logger.Instance.resetLogID();
            Logger.Instance.beginLog();

            int numberOfGames = 0;

            Console.WriteLine("WELCOME TO THE WEREWOLF GAME");
            Console.Write("Is a human playing? (Y/N) : ");
            bool isPlayerPlaying = Console.ReadLine().Equals("Y");

            if(isPlayerPlaying) Console.Write("What is your desired name? : ");
            string playerName = isPlayerPlaying ? Console.ReadLine() : string.Empty;

            GameManager gameManager = new GameManager();
            Console.WriteLine(gameManager.StartGame(isPlayerPlaying, playerName));
			Console.WriteLine(string.Format("Will start game at (N:{0}, Depth Limit : {1}), Time : {2}", Constants.N, Constants.DEPTH_LIMIT, DateTime.Now));

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
                    //LOG
                    Logger.Instance.logRound(gameManager.getRound());
                    if (gameManager.werewolfsWon())
                        Logger.Instance.logWinner("W");
                    else
                        Logger.Instance.logWinner("V");

                    gameManager.ReinitializeGame();
                    numberOfGames++;
                    if(numberOfGames <= NUMBER_OF_GAMES)
                        Console.WriteLine("Will start another game with same players (role belief reset) :" + DateTime.Now);
                }
            } while (numberOfGames <= NUMBER_OF_GAMES);

            //LOG
            Logger.Instance.endLog();

            Console.WriteLine("All games over! Press enter to close.");
            Console.ReadLine();
        }
    }
}
