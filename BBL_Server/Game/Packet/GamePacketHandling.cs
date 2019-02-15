using BBL.Utils.PacketDispatcher.Packet;
using BBL.Utils.Packages;
using System;
using System.Numerics;

namespace BBL_Server.Packet
{
    public class GamePacketHandling
    {
        [GamePacketAttribute(ClientPackages.ThankYou)]
        public static void HandleThankYou(Packet packet, Session session)
        {
            string msg = packet.Data.ReadString();
            Console.WriteLine("id: {0} - has send : {1}", session.Network.connectionID, msg);
        }


        [GamePacketAttribute(ClientPackages.Login)]
        public static void HandleCLogin(Packet packet, Session session)
        {
            string username = packet.Data.ReadString();
            string password = packet.Data.ReadString();
            if (!Database.LoginAccount(username, password, out string error)) {
                ServerTCP.PACKET_AlertMsg(session.Network.connectionID, error);
            }
            Console.WriteLine("id: {0} - Player succesfully logged : '{1}'", session.Network.connectionID, username);
            session.Client.account = Database.GetAccountData(username);
            ServerTCP.PACKET_AccountData(session.Network.connectionID, session.Client.account);
            ServerTCP.PACKET_AlertMsg(session.Network.connectionID, "You are logged in !");   
        }

        [GamePacketAttribute(ClientPackages.Register)]
        public static void HandleRegister(Packet packet, Session session)
        {
            string username = packet.Data.ReadString();
            string password = packet.Data.ReadString();
            Database.RegisterAccount(username, password, session.Network.connectionID);
        }

        [GamePacketAttribute(ClientPackages.GamePosition)]
        public static void HandleGamePosition(Packet packet, Session session)
        {
            Vector2 position = packet.Data.ReadVector2();
            Vector2 direction = packet.Data.ReadVector2();
            // TODO: Check if user sent valid coordinates
            Console.WriteLine("Received position({0}, {1}) direction({2}, {3})", position.X, position.Y, direction.X, direction.Y);
            ServerTCP.PACKET_GamePosition(session.Network.connectionID, position + 0.2f * direction);
        }

        [GamePacketAttribute(ClientPackages.GameDropBomb)]
        public static void HandleGameDropBomb(Packet packet, Session session)
        {
            Vector2 bombPosition = packet.Data.ReadVector2();
            // TODO: Check if user can plant bombs
            // TODO: Check if the location is free
            // TODO: Check if the user is planting near him
            // TODO: Send to everyone nearby
            Console.WriteLine("New bomb position({0}, {1})", bombPosition.X, bombPosition.Y);
            ServerTCP.PACKET_GameDropBomb(session.Network.connectionID, bombPosition);
        }
    }
}
