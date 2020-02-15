using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Main_Server.Data;
using Newtonsoft.Json;

namespace Main_Server
{
    class Program
    {
        private static Socket server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        const int PORT = 15000;
        private static List<Player> ConnectedPlayers = new List<Player>();
        private static byte[] sendingBuffer;
        private static byte[] receivingBuffer;
        private static List<Room> Rooms = new List<Room>();        

        static void Main(string[] args)
        {
            // start listening and accepting clients requests
            server_socket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            server_socket.Listen(5);
            sendingBuffer = new byte[2048];
            receivingBuffer = new byte[2048];
            
            server_socket.BeginAccept(AcceptingClients_callback, null);
            Console.ReadKey();

            // closing all clients
            foreach(Player player in ConnectedPlayers)
            {
                player.PlayerSocket.Shutdown(SocketShutdown.Both);
            }
        }

        #region Begin accepting clients
        static void AcceptingClients_callback(IAsyncResult result)
        {
            // getting client socket
            Socket socket = server_socket.EndAccept(result);

            // begin receiving data from clients
            socket.BeginReceive(receivingBuffer, 0, receivingBuffer.Length, SocketFlags.None, ReceivingData_callback, socket);
            

            // accepting new client
            server_socket.BeginAccept(AcceptingClients_callback, null);
        }
        #endregion

        private static Player GetPlayerInfo(string data)
        {
            string name = data.Split(',')[0];
            Player player = new Player {
                PlayerName = name,
                Id = data.Split(',')[1]
            };
            return player;
        }
        private static Player GetPlayerWithId(string id)
        {
            Player player = null;
            foreach(Player p in ConnectedPlayers)
            {
                if(p.Id == id)
                {
                    player = p;
                }
            }
            return player;
        }
        private static Room GetRoomInfo(string data)
        {
            Player owner = GetPlayerWithId(data.Split(',')[3]);
            Room room = new Room(owner)
            {
                Criteria = data.Split(',')[0],
                RoomName = data.Split(',')[1],                
                RoomId = data.Split(',')[3]
            };
            return room;
        }
        private static string SetupRoomsInfo(List<Room> rooms)
        {
            string roomStr = string.Empty;
            for(int i = 0; i < rooms.Count; i++)
            {
                roomStr += rooms[i].RoomId + ":" + "room:" + rooms[i].RoomName;
                if(i != rooms.Count - 1)
                {
                    roomStr += ",";
                }
            }
            return roomStr;
        }

        private static Player GetPlayerFromId(string id)
        {
            Player p = null;
            foreach(Player player in ConnectedPlayers)
            {
                if(player.Id == id)
                {
                    p = player;
                }
            }
            return p;
        }

        #region Begin Receiving data from the clients
        static void ReceivingData_callback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            int amountOfData = 0;
            // we are handling wether there is user that is disconnected
            try
            {
                amountOfData = socket.EndReceive(result);
            }
            catch(SocketException)
            {
                Console.WriteLine("===============================================");
                Console.WriteLine(socket.RemoteEndPoint + " Disconnected");
                Console.WriteLine("===============================================");
                
            }
            // creating a new player
            byte[] temp = new byte[amountOfData];
            Array.Copy(receivingBuffer, 0, temp, 0, temp.Length);
            
            // checking the type of the incoming message
            string data = Encoding.Default.GetString(temp);
            
            if (data.Contains(Constants.SendName))
            {
                // then the player is sending an object along with his name
                // so we will cast the data to Player object
                Player player = GetPlayerInfo(data);
                // adding the captured socket to the player socket
                player.PlayerSocket = socket;
                // adding the player to the connected players list
                ConnectedPlayers.Add(player);

                // sending rooms data to the client
                string roomsData = SetupRoomsInfo(Rooms);
                sendingBuffer = Encoding.Default.GetBytes(roomsData);
                player.PlayerSocket.BeginSend(sendingBuffer, 0, sendingBuffer.Length, SocketFlags.None, SendingData_callback, player.PlayerSocket);
            }
            else if(data.Contains(Constants.CreateRoom))
            {
                // getting data from a client that wants to create a room                
                Room room = GetRoomInfo(data);
                foreach (Player p in ConnectedPlayers)
                {
                    if(p.PlayerSocket == socket)
                    {
                        room.OwnerPlayer = p;
                    }
                }
                Rooms.Add(room);                
                Console.WriteLine($"Room {room.RoomName} created by");
            }
            else if(data.Contains(Constants.Join))
            {
                //// receveing the data from a client that wants to join a room
                Console.WriteLine(data);
                Room roomToJoin = null;
                string roomId = data.Split(' ')[0];
                Player SenderPlayer = GetPlayerFromId(roomId);
                // checking the room status
                foreach (Room room in Rooms)
                {
                    if (room.RoomId == roomId)
                    {
                        roomToJoin = room;
                        room.Players.Add(SenderPlayer);
                        room.Status = "Playing";
                        Console.WriteLine(Constants.Play);
                        byte[] buffer = Encoding.Default.GetBytes(Constants.Play);
                        room.OwnerPlayer.PlayerSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendPlay_Callback, room.OwnerPlayer.PlayerSocket);
                    }
                }

                // if is full true we will allow him to enter the room as a player
                // otherwise we will show a message                
                //TODO: check if the room is full with 2 players or not
                // if so => send a request msg to the room owner 
                // otherwise => allow him to enter the room as a watcher                
            }
            else if(data.Contains("yes join"))
            {
                // sending data to owner and sending data to joiner
                //Player owner = JsonConvert.DeserializeObject<Player>(data);
                //recMsg.SenderPlayer.msgType = "yes join";
                //string sentData = JsonConvert.SerializeObject(recMsg);
                //sendingBuffer = Encoding.Default.GetBytes(sentData);
                //Socket senderSocket = null;
                //foreach(Player p in ConnectedPlayers)
                //{
                //    if(p.Id == recMsg.SenderPlayer.Id)
                //    {
                //        senderSocket = p.PlayerSocket;
                //    }
                //}
                //senderSocket.BeginSend(sendingBuffer, 0, sendingBuffer.Length, SocketFlags.None, SendingData_callback, senderSocket);
            }

            // showing the data on the console screen
            foreach (Player p in ConnectedPlayers)
            {
                Console.WriteLine($"playerId: {p.Id}\n playerName: {p.PlayerName} \n playerStatus:  {p.Status} \n PlayerSocket: {p.PlayerSocket.ToString()}");
            }
            // begin receiving data from the client
            try
            {
                socket.BeginReceive(receivingBuffer, 0, receivingBuffer.Length, SocketFlags.None, ReceivingData_callback, socket);
            }
            catch(SocketException)
            {
                Console.WriteLine("===============================================");
                Console.WriteLine(socket.RemoteEndPoint + " Disconnected");
                Console.WriteLine("===============================================");
                // removing the disconnected client from the players list
                foreach (Player player in ConnectedPlayers)
                {
                    if (player.PlayerSocket == socket)
                    {
                        ConnectedPlayers.Remove(player);
                        break;
                    }
                }
            }
        }

        private static void SendPlay_Callback(IAsyncResult ar)
        {
            Socket ownerSocket = (Socket)ar.AsyncState;
            ownerSocket.EndSend(ar);
        }        
        #endregion

        #region sending data to all clients
        private static void SendingData_callback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            socket.EndSend(result);

            //socket.BeginSend(receivingBuffer, 0, receivingBuffer.Length, SocketFlags.None, SendingData_callback, socket);
        }
        #endregion        
    }
}
