using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client_Player.Data;

namespace Client_Player
{
    public partial class frmLogin : Form
    {
        private Socket client_socket;        
        private IPAddress serverIP;
        private byte[] sendBuffer;       
        const int SERVER_PORT = 15000;

        public frmLogin()
        {
            InitializeComponent();
            client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverIP = IPAddress.Loopback;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // checking whether the user entered the data properly or not
            if(!string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                // connecting to the main server                
                string name = txtName.Text;
                string playerId = Guid.NewGuid().ToString("N");
                client_socket.BeginConnect(new IPEndPoint(serverIP, SERVER_PORT), Connecting_Callback, name + "," + playerId);      
                
                this.Hide();
                // showing the next form            
                Roomslist roomslist = new Roomslist(name, playerId, client_socket);
                roomslist.Show();
            }            
        }

        private byte[] ConcatNameAndMsg(string data)
        {
            string temp = data + "," + Constants.SendName;
            return Encoding.Default.GetBytes(temp);
        }

        #region connecting to the main server
        private void Connecting_Callback(IAsyncResult result)
        {
            // preparing the data to be send
            string playerInfo = (string)result.AsyncState;

            sendBuffer = ConcatNameAndMsg(playerInfo);

            // sending data to the main server
            client_socket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, SendingData_callback, null);
        }
        #endregion

        #region sending data to the server
        private void SendingData_callback(IAsyncResult result)
        {
            try
            {
                client_socket.EndSend(result);
            }
            catch (SocketException)
            {
                Application.Exit();
            }
            
        }
        #endregion

    }
}
