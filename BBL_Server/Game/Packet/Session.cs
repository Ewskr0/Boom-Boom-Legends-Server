using BBL.Utils.PacketDispatcher.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL_Server.Packet
{
    public class Session : ISession
    {
        private ClientObject _client;
        public ClientObject Client => _client;

        private ClientNetwork _network;
        public ClientNetwork Network => _network;

        public Session(ClientObject client, ClientNetwork network)
        {
            _client = client;
            _network = network;
            _network.parentSession = this;
        }
    }
}
