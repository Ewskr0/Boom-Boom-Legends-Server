using BBL_Server.Packet;
using BBL.Utils.PacketDispatcher;
using BBL.Utils.PacketDispatcher.Packet;
using BBL.Utils.ByteBuffer;
using System;

namespace BBL_Server
{
    class ServerHandleData
    {
        public static void HandleData(Session session, byte[] data)
        {
            // Copying packet informations to edit / peek it.
            int connectionID = session.Network.connectionID;

            // Cheking if an instance of  bytebuffer exists for the client.
            if(session.Client.buffer == null)
            {
                session.Client.buffer = new ByteBuffer();
            }

            // Reading out the package.
            session.Client.buffer.WriteBytes(data);

            // Retrieve packet size
            int pLength = session.Client.buffer.ReadInt(true);

            // Invalid packet
            if(pLength <= 0)
            {
                session.Client.buffer.Clear();
                return;
            }

            if (pLength != session.Client.buffer.Length())
            {
                Console.WriteLine("Invalid packet size");
                session.Client.buffer.Clear();
                return;
            }
            // Collecting data.
            session.Client.buffer.FixReadPosition();
            HandleDataPackages(session, session.Client.buffer);
            session.Client.buffer.Clear();
        }

        private static bool HandleDataPackages(Session session, ByteBuffer data)
        {
            int packageID = data.ReadInt();
            data.FixReadPosition();
            Packet.Packet cPacket = new Packet.Packet(packageID, data);
            return PacketDispatcher<GamePacketHandling>.Instance.Dispatch(cPacket, session);
        }
    }
}
