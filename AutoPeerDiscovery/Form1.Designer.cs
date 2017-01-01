namespace AutoPeerDiscovery
{
    partial class Form1
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
            this.ListViewPeer = new System.Windows.Forms.ListView();
            this.PeerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // ListViewPeer
            // 
            this.ListViewPeer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PeerName});
            this.ListViewPeer.FullRowSelect = true;
            this.ListViewPeer.GridLines = true;
            this.ListViewPeer.Location = new System.Drawing.Point(34, 35);
            this.ListViewPeer.Name = "ListViewPeer";
            this.ListViewPeer.Size = new System.Drawing.Size(168, 141);
            this.ListViewPeer.TabIndex = 5;
            this.ListViewPeer.UseCompatibleStateImageBehavior = false;
            this.ListViewPeer.View = System.Windows.Forms.View.Details;
            // 
            // PeerName
            // 
            this.PeerName.Text = "Peer Name";
            this.PeerName.Width = 120;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ListViewPeer);
            this.Name = "Form1";
            this.Text = "Auto Peer Discovery";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView ListViewPeer;
        private System.Windows.Forms.ColumnHeader PeerName;
    }
}

