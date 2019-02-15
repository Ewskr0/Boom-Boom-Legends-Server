using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BBL.Utils.Packages;

namespace BBL.Utils.PacketDispatcher.Packet
{
    public interface IPacket
    {
        ClientPackages Id { get; }
    }
}
