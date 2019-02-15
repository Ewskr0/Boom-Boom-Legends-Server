using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BBL.Utils.Packages;

namespace BBL.Utils.PacketDispatcher.Packet
{
    public class GamePacket
    {
        public GamePacket(Action<IPacket, ISession> handler, GamePacketAttribute packetAttribute)
        {
            Handler = handler;
            PacketAttribute = packetAttribute;
            Id = PacketAttribute?.Id ?? 0;
        }

        public Action<IPacket, ISession> Handler { get; }

        public ClientPackages Id { get; }

        public GamePacketAttribute PacketAttribute { get; }


        public void Execute(IPacket packet, ISession session)
        {
            Handler(packet, session);
        }
    }
}
