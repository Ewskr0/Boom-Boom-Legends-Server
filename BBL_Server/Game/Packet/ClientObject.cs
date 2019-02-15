using BBL_Server.Entity;
using BBL_Server.Packet;
using System;
using System.Net.Sockets;
using BBL.Utils.ByteBuffer;

namespace BBL_Server.Packet
{
    public class ClientObject 
    {


        public ByteBuffer buffer;
        public Account account;
        public Game currentGame;

        public ClientObject()
        {
            account = new Account();
            currentGame = null;
            // Is now connected.
        }
    }
}
