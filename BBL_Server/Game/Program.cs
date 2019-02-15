using BBL_Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBL.Servers.Game
{
    class Program
    {
        private static Thread consoleThread;

        static void Main(string[] args)
        {
            InitializeConsoleThread();
            ServerTCP.InitializeServer();
        }

        private static void InitializeConsoleThread()
        {
            consoleThread = new Thread(ConsoleLoop);
            consoleThread.Name = "ConsoleThread";
            consoleThread.Start();

        }

        private static void ConsoleLoop()
        {
            while (true)
            {

            }
        }
    }
}
