using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main_Server
{
    public class Room
    {        
        public string RoomId { get; set; }
        public string RoomName { get; set; }        
        public string Criteria { get; set; }        
        public List<Player> Players { get; set; }
        [JsonIgnore]
        public Player OwnerPlayer { get; set; }
        public string MsgType { get; set; }
        public string Status { get; set; }
        
        public Room(Player ownerPlayer)
        {
            Status = "Waiting";
            Players = new List<Player>();
            OwnerPlayer = ownerPlayer;
            Players.Add(OwnerPlayer);
        }
    }
}
