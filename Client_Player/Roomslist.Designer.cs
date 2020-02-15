namespace Client_Player
{
    partial class Roomslist
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lsRooms = new System.Windows.Forms.ListBox();
            this.lblPlayerTurn = new System.Windows.Forms.Label();
            this.btnJoin = new System.Windows.Forms.Button();
            this.btnCreateRoom = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lsRooms
            // 
            this.lsRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsRooms.FormattingEnabled = true;
            this.lsRooms.ItemHeight = 20;
            this.lsRooms.Location = new System.Drawing.Point(0, 76);
            this.lsRooms.Name = "lsRooms";
            this.lsRooms.Size = new System.Drawing.Size(915, 385);
            this.lsRooms.TabIndex = 3;
            this.lsRooms.SelectedIndexChanged += new System.EventHandler(this.lsRooms_SelectedIndexChanged);
            // 
            // lblPlayerTurn
            // 
            this.lblPlayerTurn.AutoSize = true;
            this.lblPlayerTurn.Location = new System.Drawing.Point(23, 29);
            this.lblPlayerTurn.Name = "lblPlayerTurn";
            this.lblPlayerTurn.Size = new System.Drawing.Size(88, 20);
            this.lblPlayerTurn.TabIndex = 0;
            this.lblPlayerTurn.Text = "Player Turn";
            // 
            // btnJoin
            // 
            this.btnJoin.Enabled = false;
            this.btnJoin.Location = new System.Drawing.Point(813, 18);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(90, 42);
            this.btnJoin.TabIndex = 1;
            this.btnJoin.Text = "Join";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // btnCreateRoom
            // 
            this.btnCreateRoom.Location = new System.Drawing.Point(674, 18);
            this.btnCreateRoom.Name = "btnCreateRoom";
            this.btnCreateRoom.Size = new System.Drawing.Size(120, 42);
            this.btnCreateRoom.TabIndex = 2;
            this.btnCreateRoom.Text = "Create Room";
            this.btnCreateRoom.UseVisualStyleBackColor = true;
            this.btnCreateRoom.Click += new System.EventHandler(this.btnCreateRoom_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCreateRoom);
            this.panel1.Controls.Add(this.btnJoin);
            this.panel1.Controls.Add(this.lblPlayerTurn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(915, 76);
            this.panel1.TabIndex = 0;
            // 
            // Roomslist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 461);
            this.Controls.Add(this.lsRooms);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Roomslist";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Roomslist";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Roomslist_FormClosing);
            this.Load += new System.EventHandler(this.Roomslist_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox lsRooms;
        private System.Windows.Forms.Label lblPlayerTurn;
        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Button btnCreateRoom;
        private System.Windows.Forms.Panel panel1;
    }
}