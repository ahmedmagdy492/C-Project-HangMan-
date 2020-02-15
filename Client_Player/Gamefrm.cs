using Client_Player;
using Client_Player.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace guess_the_name
{
    public partial class GameFrm : Form
    {
        #region Global Members
        public string[] words;
        public string currentWord = "";
        public string showCurrentWord = "";
        public Socket Owner { get; set; }
        /*public Room Room { get; set; }
        public List<Player> InGamePlayers { get; set; }
        public List<Player> Watchers { get; set; }*/
        public Roomslist parentForm { get; set; }
        public byte[] recBuffer;
        public byte[] sendBuffer;
        int wrongLetter = 0;
        #endregion 

        public GameFrm(Socket OwnerSocket ,Roomslist parentFrm)
        {
            InitializeComponent();            
            this.Owner = OwnerSocket;
            parentForm = parentFrm;            
            recBuffer = new byte[2048];
            sendBuffer = new byte[2048];            
        }

        #region Game Logic
        private void readalllines()
        {            
            string[] readlines = File.ReadAllLines("words.txt");
            words = new string[readlines.Length];
            int index = 0;
            foreach(string s in readlines)
            {              
                words[index++] = s;
            }            
        }
        private void randamWordChoice()
        {
            int randamIndex = (new Random()).Next(words.Length);
            currentWord = words[randamIndex];
            showCurrentWord = "";
            for(int i=0;i<currentWord.Length;i++)
            {
                showCurrentWord += "_";
            }
            displayCurrentWord();
        }
        private void displayCurrentWord()
        {
            labelShowWord.Text = "";
            for (int i = 0; i < showCurrentWord.Length; i++)
            {
                labelShowWord.Text += showCurrentWord.Substring(i, 1);
                labelShowWord.Text += " ";
            }
        }      
        private void btnA_Click(object sender, EventArgs e)
        {
            Button choice = sender as Button;//generic object
            choice.Enabled = false;
            if(currentWord.Contains(choice.Text))
            {
                char[] tempChar = showCurrentWord.ToCharArray();
                char[] findToArray = currentWord.ToCharArray();
                char guessChar = choice.Text.ElementAt(0);
                for(int i=0;i<findToArray.Length;i++)
                {
                    if(findToArray[i]==guessChar)
                    {
                        tempChar[i] = guessChar;
                    }
                }
                showCurrentWord = new string(tempChar);
                displayCurrentWord();
            }
            else
            {
                wrongLetter++;
            }
            if(showCurrentWord.Equals(currentWord))
            {
                MessageBox.Show("you win");
            }
        }        
        private void DisableAllButtons()
        {
            labelShowWord.Visible = false;
            foreach(Button btn in keysBox.Controls)
            {
                btn.Enabled = false;
            }
        }
        private void EnableAllButtons()
        {
            labelShowWord.Visible = true;
            foreach(Control c in keysBox.Controls)
            {
                if(c is Button)
                {
                    ((Button)c).Enabled = true;
                }
            }
        }
        #endregion

        private void StartGame()
        {
            int size = Owner.Receive(recBuffer);
            byte[] temp = new byte[size];
            Array.Copy(recBuffer, temp, size);
            string dataInString = Encoding.Default.GetString(temp);
            
            if (dataInString.Contains(Constants.Play))
            {
                // then we shall get a dialog box                
                MessageBox.Show(dataInString);
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            DisableAllButtons();
            lblPlayerTurn.Text = "Wating for another player";
            readalllines();
            randamWordChoice();

            Task recTask = new Task(StartGame);
            recTask.Start();
            await recTask;
        }

        #region receiving data callback
        private void ReceiveData(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;            
            int amountOfData = socket.EndReceive(result);
            byte[] temp = new byte[amountOfData];
            Array.Copy(recBuffer, 0, temp, 0, temp.Length);
            string dataInString = Encoding.Default.GetString(temp);
            if (dataInString.Contains(Constants.Join))
            {
                // then we shall get a dialog box                
                MessageBox.Show(dataInString);
            }

            Owner.BeginReceive(recBuffer, 0, recBuffer.Length, SocketFlags.None, ReceiveData, Owner);
        }
        #endregion

        private void GameFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentForm.Show();
        }   
                
    }    
    
}
