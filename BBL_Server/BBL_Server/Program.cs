using System;
using System.Threading;
using BBL.Utils.PacketDispatcher;

namespace BBL_Server
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
