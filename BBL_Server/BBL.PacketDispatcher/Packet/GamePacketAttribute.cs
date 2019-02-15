using System;
using System.Collections.Generic;
using System.Text;
using BBL.Utils.Packages;

namespace BBL.Utils.PacketDispatcher.Packet
{
    public class GamePacketAttribute : Attribute
    {
        public ClientPackages Id;

        public GamePacketAttribute(ClientPackages id)
        {
            Id = id;
        }
    }
}
