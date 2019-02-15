using System;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using BBL.Utils.ByteBuffer;
using BBL.Utils.Packages;
using BBL_Server.Packet;

namespace BBL_Server
{
    class ServerTCP
    {
        private static TcpListener serverSocket;
        public static Session[] clientSessions = new Session[Constants.MAX_PLAYERS];

        public static void InitializeServer()
        {
            InitialiseMySQLServer();
            InitializeServerSocket();
        }
        private static void InitialiseMySQLServer()
        {
            // Init MySQL connection.
            MySQL.mySQLSettings.server = Constants.mySQLServer;
            MySQL.mySQLSettings.database = Constants.mySQLDatabase;
            MySQL.mySQLSettings.user = Constants.mySQLUser;
            MySQL.mySQLSettings.password = Constants.mySQLPassword;
            MySQL.ConnectToMySQL();
        }
        private static void InitializeServerSocket()
        {
            serverSocket = new TcpListener(IPAddress.Any, 5555);
            serverSocket.Start();
            serverSocket.BeginAcceptSocket(new AsyncCallback(ClientConnectCallback), null);
        }

        private static void ClientConnectCallback(IAsyncResult result)
        {
            // Instantiate-server client to client-client.
            TcpClient tempClient = serverSocket.EndAcceptTcpClient(result);
            // Accept connections back.
            serverSocket.BeginAcceptSocket(new AsyncCallback(ClientConnectCallback), null);

            for (int i = 1; i < Constants.MAX_PLAYERS; i++)
            {
                if (clientSessions[i] == null)
                {
                    ClientNetwork network = new ClientNetwork(tempClient, i);
                    ClientObject obj = new ClientObject();
                    clientSessions[i] = new Session(obj, network);

                    ServerTCP.PACKET_WelcomeMsg(i, "Welcome to Boom Boom Legends !");
                    return;
                }
            }
        }

        public static void Close(int connectionID)
        {
            clientSessions[connectionID] = null;
        }

        public static void SendDataTo(int connectionID, byte[] data)
        {
            // ---- WE SEND THE SIZE THEN DATA ----
            ByteBuffer buffer = new ByteBuffer();
            // Calculating the size of the package.
            buffer.WriteInt((data.GetUpperBound(0) - data.GetLowerBound(0) + 1));
            buffer.WriteBytes(data);
            // Sending data.
            clientSessions[connectionID].Network.myStream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
            buffer.Dispose();
        }

        public static void PACKET_WelcomeMsg(int connectionID, string msg)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackages.WelcomeMsg);
            buffer.WriteString(msg);
            SendDataTo(connectionID, buffer.ToArray());

        }
        public static void PACKET_AlertMsg(int connectionID, string msg)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackages.AlertMsg);
            buffer.WriteString(msg);
            SendDataTo(connectionID, buffer.ToArray());
            Console.WriteLine("id: {0} - AlertMsg : {1}", connectionID, msg);
        }
        public static void PACKET_AccountData(int connectionID, Account account)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackages.AccountData);
            string packet = account.ToString();
            buffer.WriteString(packet);
            Console.WriteLine(packet);
            SendDataTo(connectionID, buffer.ToArray());
            Console.WriteLine("id: {0} - AccountData sent : {1}", connectionID, account.username);
        }

        public static void PACKET_GamePosition(int connectionID, Vector2 newPosition)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackages.GamePosition);
            buffer.WriteVector2(newPosition);
            SendDataTo(connectionID, buffer.ToArray());
            Console.WriteLine("id: {0} - New Position sent : {1}, {2}", connectionID, newPosition.X, newPosition.Y);
        }

        public static void PACKET_GameDropBomb(int connectionID, Vector2 bombPosition)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackages.GameDropBomb);
            buffer.WriteVector2(bombPosition);
            SendDataTo(connectionID, buffer.ToArray());
            Console.WriteLine("id: {0} - New Bomb sent : {1}, {2}", connectionID, bombPosition.X, bombPosition.Y);
        }

        public static void PACKET_GameFound(int connectionID, Vector2 bombPosition)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackages.GameFound);
            SendDataTo(connectionID, buffer.ToArray());
            Console.WriteLine("id: {0} - GameFound sent : {1}, {2}", connectionID);
        }

        public static void PACKET_GameStarted(int connectionID, Vector2 bombPosition)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)ServerPackages.GameStarted);
            SendDataTo(connectionID, buffer.ToArray());
            Console.WriteLine("id: {0} - GameStarted sent", connectionID);
        }
    }
}
