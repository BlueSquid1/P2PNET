namespace Test
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
            this.SendObj = new System.Windows.Forms.Button();
            this.SendFile = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // SendObj
            // 
            this.SendObj.Location = new System.Drawing.Point(101, 170);
            this.SendObj.Name = "SendObj";
            this.SendObj.Size = new System.Drawing.Size(75, 23);
            this.SendObj.TabIndex = 0;
            this.SendObj.Text = "SendObj";
            this.SendObj.UseVisualStyleBackColor = true;
            this.SendObj.Click += new System.EventHandler(this.SendObj_Click);
            // 
            // SendFile
            // 
            this.SendFile.Location = new System.Drawing.Point(101, 199);
            this.SendFile.Name = "SendFile";
            this.SendFile.Size = new System.Drawing.Size(75, 23);
            this.SendFile.TabIndex = 1;
            this.SendFile.Text = "SendFile";
            this.SendFile.UseVisualStyleBackColor = true;
            this.SendFile.Click += new System.EventHandler(this.SendFile_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(52, 61);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(149, 20);
            this.txtFilePath.TabIndex = 2;
            this.txtFilePath.Text = "06-train-cat-shake-hands.jpg";
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Location = new System.Drawing.Point(101, 88);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(100, 20);
            this.txtIpAddress.TabIndex = 3;
            this.txtIpAddress.Text = "192.168.1.112";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.txtIpAddress);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.SendFile);
            this.Controls.Add(this.SendObj);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SendObj;
        private System.Windows.Forms.Button SendFile;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.TextBox txtIpAddress;
    }
}

