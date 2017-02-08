namespace MessageProgram
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
            this.btnBroadcast = new System.Windows.Forms.Button();
            this.txtSendMsg = new System.Windows.Forms.TextBox();
            this.txtReceivedMsgs = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnBroadcast
            // 
            this.btnBroadcast.Location = new System.Drawing.Point(177, 219);
            this.btnBroadcast.Name = "btnBroadcast";
            this.btnBroadcast.Size = new System.Drawing.Size(95, 23);
            this.btnBroadcast.TabIndex = 0;
            this.btnBroadcast.Text = "Send Broadcast";
            this.btnBroadcast.UseVisualStyleBackColor = true;
            this.btnBroadcast.Click += new System.EventHandler(this.sendBroadcast_Click);
            // 
            // txtSendMsg
            // 
            this.txtSendMsg.Location = new System.Drawing.Point(12, 193);
            this.txtSendMsg.Name = "txtSendMsg";
            this.txtSendMsg.Size = new System.Drawing.Size(260, 20);
            this.txtSendMsg.TabIndex = 1;
            // 
            // txtReceivedMsgs
            // 
            this.txtReceivedMsgs.Location = new System.Drawing.Point(12, 12);
            this.txtReceivedMsgs.Multiline = true;
            this.txtReceivedMsgs.Name = "txtReceivedMsgs";
            this.txtReceivedMsgs.Size = new System.Drawing.Size(260, 175);
            this.txtReceivedMsgs.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.txtReceivedMsgs);
            this.Controls.Add(this.txtSendMsg);
            this.Controls.Add(this.btnBroadcast);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBroadcast;
        private System.Windows.Forms.TextBox txtSendMsg;
        private System.Windows.Forms.TextBox txtReceivedMsgs;
    }
}

