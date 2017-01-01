namespace HelloWorld
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
            this.TxtMsgBox = new System.Windows.Forms.TextBox();
            this.BtnSend = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtIpAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtReceived = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TxtMsgBox
            // 
            this.TxtMsgBox.Location = new System.Drawing.Point(28, 93);
            this.TxtMsgBox.Name = "TxtMsgBox";
            this.TxtMsgBox.Size = new System.Drawing.Size(193, 20);
            this.TxtMsgBox.TabIndex = 2;
            this.TxtMsgBox.Text = "Hello World";
            // 
            // BtnSend
            // 
            this.BtnSend.Location = new System.Drawing.Point(28, 119);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(87, 23);
            this.BtnSend.TabIndex = 3;
            this.BtnSend.Text = "Send by TCP";
            this.BtnSend.UseVisualStyleBackColor = true;
            this.BtnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "message to send:";
            // 
            // TxtIpAddress
            // 
            this.TxtIpAddress.Location = new System.Drawing.Point(28, 41);
            this.TxtIpAddress.Name = "TxtIpAddress";
            this.TxtIpAddress.Size = new System.Drawing.Size(100, 20);
            this.TxtIpAddress.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Ip address to connect to:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Message Received:";
            // 
            // TxtReceived
            // 
            this.TxtReceived.Enabled = false;
            this.TxtReceived.Location = new System.Drawing.Point(28, 207);
            this.TxtReceived.Name = "TxtReceived";
            this.TxtReceived.Size = new System.Drawing.Size(193, 20);
            this.TxtReceived.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.TxtReceived);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TxtIpAddress);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnSend);
            this.Controls.Add(this.TxtMsgBox);
            this.Name = "Form1";
            this.Text = "Hello World";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox TxtMsgBox;
        private System.Windows.Forms.Button BtnSend;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtIpAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtReceived;
    }
}

