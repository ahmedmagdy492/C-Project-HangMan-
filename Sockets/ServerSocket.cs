using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Sockets
{    
    public class ServerSocket
    {
        private Socket server_socket;
        private byte[] buffer;        

        public ServerSocket()
        {
            server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            buffer = new byte[1024];
        }

        public void Bind(int port)
        {
            // bind this socket to any ip that the server has
            server_socket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Listen(int backLog)
        {
            // listening and waiting for connections
            server_socket.Listen(backLog);
        }

        public void Accept(AsyncCallback callback)
        {
            // begin accepting clients requests
            server_socket.BeginAccept(callback, null);
        }        

        public void RecieveCallback(IAsyncResult result)
        {
            Socket clientSocket = (Socket)result.AsyncState;
            SocketError se;
            int bufferSize = 0;
            try
            {
                clientSocket.EndReceive(result, out se);
                if (se == SocketError.Success)
                {
                    byte[] packet = new byte[bufferSize];
                    Array.Copy(buffer, 0, packet, 0, bufferSize);

                    // receiving data from more than one client
                    buffer = new byte[1024];
                    clientSocket.BeginReceive(packet, 0, packet.Length, SocketFlags.None, RecieveCallback, clientSocket);
                }
            }
            catch(SocketException)
            {
                
            }            
        }

        public void Send(string data)
        {
            buffer = Encoding.Default.GetBytes(data);
            server_socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, Send_Callback, null);
        }

        private void Send_Callback(IAsyncResult result)
        {
            server_socket.EndSend(result);
        }
    }
}
