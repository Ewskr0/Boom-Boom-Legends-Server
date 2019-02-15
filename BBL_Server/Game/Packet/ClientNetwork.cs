using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace BBL_Server.Packet
{
    public class ClientNetwork
    {
        public int connectionID;
        public byte[] receiveBuffer;
        public TcpClient socket;
        public NetworkStream myStream;
        public Session parentSession;

        public ClientNetwork(TcpClient _socket, int _connectionID)
        {
            if (_socket == null)
                return;
            socket = _socket;
            connectionID = _connectionID;

            socket.NoDelay = true;
            socket.ReceiveBufferSize = 4096;
            socket.SendBufferSize = 4096;

            myStream = socket.GetStream();
            receiveBuffer = new byte[4096];
            myStream.BeginRead(receiveBuffer, 0, socket.ReceiveBufferSize, ReceiveCallback, null);
            Console.WriteLine("id: {0} - Incoming connection from : {1}", connectionID, socket.Client.RemoteEndPoint.ToString());
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int readBytes = myStream.EndRead(result);

                // Means client is no longer connected.
                if (readBytes <= 0)
                {
                    CloseConnection();
                    return;
                }

                // Invalid packet
                if (readBytes < 4)
                {
                    return;
                }

                byte[] newBytes = new byte[readBytes];
                Buffer.BlockCopy(receiveBuffer, 0, newBytes, 0, readBytes);
                // Handle Data here.
                ServerHandleData.HandleData(parentSession, newBytes);
                // Reading back.
                myStream.BeginRead(receiveBuffer, 0, socket.ReceiveBufferSize, ReceiveCallback, null);

            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }
        }

        private void CloseConnection()
        {
            Console.WriteLine("Connection ended from : {0}", socket.Client.RemoteEndPoint.ToString());
            ServerTCP.Close(connectionID);
            socket.Close();

            // Legacy: temp keep in case we need to check
            socket = null;
        }
    }
}
