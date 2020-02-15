using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sockets
{
    public class ClientSocket
    {
        private Socket client_socket;
        private byte[] buffer;
        public string Data { get; set; }

        public ClientSocket()
        {
            client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            buffer = new byte[1024];
        }

        public void Connect(string serverIp, int port)
        {
            client_socket.BeginConnect(new IPEndPoint(IPAddress.Parse(serverIp), port), ConnectCallback, null);
        }

        private void ConnectCallback(IAsyncResult result)
        {
            if(client_socket.Connected)
            {
                buffer = new byte[1024];
                // begin recieving data from the server
                client_socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, RecieveCallback, null);
            }
        }

        private void RecieveCallback(IAsyncResult result)
        {
            try
            {
                int bufferLen = client_socket.EndReceive(result);
                byte[] packet = new byte[bufferLen];
                Array.Copy(buffer, 0, packet, 0, bufferLen);
            }
            catch (SocketException)
            {
                
            }            
        }

        public void Send(string data)
        {
            Data = data;
            buffer = Encoding.Default.GetBytes(Data);
            client_socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, Send_Callback, null);
        }

        private void Send_Callback(IAsyncResult result)
        {
            client_socket.EndSend(result);
        }
    }
}
