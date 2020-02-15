using System;
using System.Windows.Forms;

namespace Client_Player
{
    public partial class CreateRoom : Form
    {
        public string criteria = string.Empty;
        public string RoomName = string.Empty;

        public CreateRoom()
        {
            InitializeComponent();
            cmbCriteria.Items.AddRange(new string[] { "Animals", "Footballers", "Actors" });
        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtRoomName.Text.Trim()))
            {
                this.DialogResult = DialogResult.OK;
                RoomName = txtRoomName.Text;
                criteria = cmbCriteria.Text;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
