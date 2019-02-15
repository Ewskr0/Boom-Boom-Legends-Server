using BBL.Utils.PacketDispatcher.Packet;
using BBL.Utils.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BBL.Utils.ByteBuffer;

namespace BBL_Server.Packet
{
    public class Packet : IPacket
    {
        private ClientPackages _id;
        ClientPackages IPacket.Id => _id;

        private ByteBuffer _data;
        public ByteBuffer Data => _data;

        public Packet(int id, ByteBuffer data)
        {
            _data = data;
            _id = (ClientPackages)id;
        }
    }
}
