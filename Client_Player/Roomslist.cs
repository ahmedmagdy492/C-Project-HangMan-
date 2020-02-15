using Client_Player.Data;
using guess_the_name;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Client_Player
{
    public partial class Roomslist : Form
    {
        public string PlayerName{ get; set; }
        public string PlayerId { get; set; }
        public byte[] recBuffer { get; set; }
        public byte[] sendBuffer { get; set; }        
        public delegate void ShowItems();
        public ShowItems showItems;
        public Roomslist thisFrm;
        public Socket PlayerSocket;

        public Roomslist(string playerName, string PId, Socket socket)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            PlayerName = playerName;
            PlayerId = PId;
            PlayerSocket = socket;
            recBuffer = new byte[2048];
            sendBuffer = new byte[2048];
            thisFrm = this;
        }
        
        private string[] GetRooms(string data)
        {
            return data.Split(',');
        }

        private void RecieveData(IAsyncResult result)
        {
            int amountOfdata = PlayerSocket.EndReceive(result);
            byte[] temp = new byte[amountOfdata];            
            Array.Copy(recBuffer, 0, temp, 0, temp.Length);
            string dataInString = Encoding.Default.GetString(recBuffer);            

            // checking what type of response has the server sent
            if(dataInString.Contains(Constants.Room))
            {
                #region receiving list of rooms                
                string[] rooms = GetRooms(dataInString);                
                lsRooms.Items.Clear();
                foreach (string room in rooms)
                {
                    lsRooms.Items.Add(room.Split(':')[0] + " " + room.Split(':')[2]);
                }
                #endregion
            }
            else if(dataInString.Contains("yes join"))
            {
                //JoinMsg joinMsg = JsonConvert.DeserializeObject<JoinMsg>(dataInString);
                //Room room = null;
                //foreach(Room r in Rooms)
                //{
                //    if(r.RoomId == joinMsg.RoomId)
                //    {
                //        room = r;
                //        break;
                //    }
                //}                
                //this.Hide();
                //GameFrm gameFrm = new GameFrm(joinMsg.SenderPlayer, room, this);
                //gameFrm.Show();
            }
            else
            {
                string recData = Encoding.Default.GetString(recBuffer);

                #region receiving rooms as well
                //if(recData.Contains("room"))
                //{
                //    // deserialzing the data as a room list object
                //    // recieving rooms
                //    Rooms = JsonConvert.DeserializeObject<List<Room>>(recData);

                //    foreach (Room room in Rooms)
                //    {
                //        lsRooms.Items.Add(room.RoomName + " " + room.RoomId);
                //    }
                //}                            
                #endregion
            }
            recBuffer = new byte[2048];
            PlayerSocket.BeginReceive(recBuffer, 0, recBuffer.Length, SocketFlags.None, RecieveData, null);         
        }       

        private void Roomslist_Load(object sender, EventArgs e)
        {
            lblPlayerTurn.Text = PlayerName;

            // receiving rooms info from the server
            try
            { 
                PlayerSocket.BeginReceive(recBuffer, 0, recBuffer.Length, SocketFlags.None, RecieveData, null);
            }
            catch (SocketException)
            {
                MessageBox.Show("Cannot connect because the server is off", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void Roomslist_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms[0].Close();
        }        
        private void SendData_callback(IAsyncResult result)
        {
            PlayerSocket.EndSend(result);
            //Player.PlayerSocket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, SendData_callback, null);
        }

        private string CreateRoom(string data)
        {
            string roomData = Constants.CreateRoom + "," + data;
            return roomData;
        }        
        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            // showing the new room window
            CreateRoom createRoom = new CreateRoom();
            DialogResult result = createRoom.ShowDialog();

            // checking on the returned form value
            if(result == DialogResult.OK)
            {
                // creating a new room by sending a request to the server                  
                string data = CreateRoom(createRoom.criteria +"," + createRoom.RoomName + "," + PlayerId);
                sendBuffer = Encoding.Default.GetBytes(data);
                PlayerSocket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, SendData_callback, null);                

                // recieving the updated rooms data from the server
                PlayerSocket.BeginReceive(recBuffer, 0, recBuffer.Length, SocketFlags.None, RecieveData, null);
                this.Hide();
                GameFrm game = new GameFrm(PlayerSocket, this);
                game.Show();
            }            

        }

        private void lsRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lsRooms.SelectedItems.Count == 1)
            {
                btnJoin.Enabled = true;
            }
            else
            {
                btnJoin.Enabled = false;
            }
        }

        private string JoinRoom()
        {
            return lsRooms.SelectedItem.ToString().Split(',')[0] + Constants.Join;
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            // sending a special message to the server
            // to indicate that the player wants to join a room            
            string joinData = JoinRoom();            
            
            recBuffer = Encoding.Default.GetBytes(joinData);

            ///// =?>
            PlayerSocket.BeginSend(recBuffer, 0, recBuffer.Length, SocketFlags.None, SendJoinData, null);

            this.Hide();

            GameFrm game = new GameFrm(PlayerSocket, this);
            game.Show();
        }

        private void SendJoinData(IAsyncResult result)
        {
            PlayerSocket.EndSend(result);
        }
    }
}
