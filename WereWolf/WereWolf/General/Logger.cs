using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WereWolf.General
{
    public sealed class Logger
    {
        private static volatile Logger instance;
        private static object syncRoot = new Object();

        Random randGenerator;
        String logID;

        private Logger() {
            randGenerator = new Random();
            logID = "";
            Directory.CreateDirectory(@"..\..\logs");
        }

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Logger();
                    }
                }

                return instance;
            }
        }

        public void resetLogID()
        {
            logID = "";
            for (int i = 0; i < 10; i++)
                logID = logID + randGenerator.Next(0, 9);
        }

        public void beginLog()
        {
            File.AppendAllText(@"..\..\logs\log" + logID + ".txt", "start" + Environment.NewLine);
            File.AppendAllText(@"..\..\logs\log" + logID + ".txt", "----------------------" + Environment.NewLine);
        }

        public void endLog()
        {
            File.AppendAllText(@"..\..\logs\log" + logID + ".txt", "----------------------" + Environment.NewLine);
            File.AppendAllText(@"..\..\logs\log" + logID + ".txt", "end" + Environment.NewLine); 
        }

        public void logRound(int i)
        {
            File.AppendAllText(@"..\..\logs\log" + logID + ".txt", "" + i + " | ");
        }

        public void logWinner(string s)
        {
            File.AppendAllText(@"..\..\logs\log" + logID + ".txt", s + Environment.NewLine);
        }

    }
}
