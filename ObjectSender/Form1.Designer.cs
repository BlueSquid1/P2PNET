namespace ObjectSender
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
            this.btnSendDog = new System.Windows.Forms.Button();
            this.btnSendPerson = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtDogName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCatName = new System.Windows.Forms.TextBox();
            this.btnSendCat = new System.Windows.Forms.Button();
            this.chkIncludeCat = new System.Windows.Forms.CheckBox();
            this.chkIncludeFish = new System.Windows.Forms.CheckBox();
            this.chkIncludeDog = new System.Windows.Forms.CheckBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFishName = new System.Windows.Forms.TextBox();
            this.btnSendFish = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkFreshWater = new System.Windows.Forms.CheckBox();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSendDog
            // 
            this.btnSendDog.Location = new System.Drawing.Point(119, 71);
            this.btnSendDog.Name = "btnSendDog";
            this.btnSendDog.Size = new System.Drawing.Size(75, 23);
            this.btnSendDog.TabIndex = 0;
            this.btnSendDog.Text = "Send Dog";
            this.btnSendDog.UseVisualStyleBackColor = true;
            this.btnSendDog.Click += new System.EventHandler(this.btnSendDog_Click);
            // 
            // btnSendPerson
            // 
            this.btnSendPerson.Location = new System.Drawing.Point(119, 219);
            this.btnSendPerson.Name = "btnSendPerson";
            this.btnSendPerson.Size = new System.Drawing.Size(75, 23);
            this.btnSendPerson.TabIndex = 1;
            this.btnSendPerson.Text = "SendPerson";
            this.btnSendPerson.UseVisualStyleBackColor = true;
            this.btnSendPerson.Click += new System.EventHandler(this.btnSendPerson_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtDogName);
            this.groupBox1.Controls.Add(this.btnSendDog);
            this.groupBox1.Location = new System.Drawing.Point(27, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dog";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtLastName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtFirstName);
            this.groupBox2.Controls.Add(this.btnSendPerson);
            this.groupBox2.Location = new System.Drawing.Point(246, 175);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 252);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Person";
            // 
            // txtDogName
            // 
            this.txtDogName.Location = new System.Drawing.Point(47, 30);
            this.txtDogName.Name = "txtDogName";
            this.txtDogName.Size = new System.Drawing.Size(147, 20);
            this.txtDogName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtCatName);
            this.groupBox3.Controls.Add(this.btnSendCat);
            this.groupBox3.Location = new System.Drawing.Point(246, 59);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 100);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Cat";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // txtCatName
            // 
            this.txtCatName.Location = new System.Drawing.Point(47, 30);
            this.txtCatName.Name = "txtCatName";
            this.txtCatName.Size = new System.Drawing.Size(147, 20);
            this.txtCatName.TabIndex = 1;
            // 
            // btnSendCat
            // 
            this.btnSendCat.Location = new System.Drawing.Point(119, 71);
            this.btnSendCat.Name = "btnSendCat";
            this.btnSendCat.Size = new System.Drawing.Size(75, 23);
            this.btnSendCat.TabIndex = 0;
            this.btnSendCat.Text = "Send Cat";
            this.btnSendCat.UseVisualStyleBackColor = true;
            this.btnSendCat.Click += new System.EventHandler(this.btnSendCat_Click);
            // 
            // chkIncludeCat
            // 
            this.chkIncludeCat.AutoSize = true;
            this.chkIncludeCat.Location = new System.Drawing.Point(15, 42);
            this.chkIncludeCat.Name = "chkIncludeCat";
            this.chkIncludeCat.Size = new System.Drawing.Size(97, 17);
            this.chkIncludeCat.TabIndex = 3;
            this.chkIncludeCat.Text = "Include the cat";
            this.chkIncludeCat.UseVisualStyleBackColor = true;
            // 
            // chkIncludeFish
            // 
            this.chkIncludeFish.AutoSize = true;
            this.chkIncludeFish.Location = new System.Drawing.Point(15, 65);
            this.chkIncludeFish.Name = "chkIncludeFish";
            this.chkIncludeFish.Size = new System.Drawing.Size(98, 17);
            this.chkIncludeFish.TabIndex = 4;
            this.chkIncludeFish.Text = "Include the fish";
            this.chkIncludeFish.UseVisualStyleBackColor = true;
            // 
            // chkIncludeDog
            // 
            this.chkIncludeDog.AutoSize = true;
            this.chkIncludeDog.Location = new System.Drawing.Point(15, 19);
            this.chkIncludeDog.Name = "chkIncludeDog";
            this.chkIncludeDog.Size = new System.Drawing.Size(100, 17);
            this.chkIncludeDog.TabIndex = 5;
            this.chkIncludeDog.Text = "Include the dog";
            this.chkIncludeDog.UseVisualStyleBackColor = true;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(66, 19);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(128, 20);
            this.txtFirstName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "First Name";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkFreshWater);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtFishName);
            this.groupBox4.Controls.Add(this.btnSendFish);
            this.groupBox4.Location = new System.Drawing.Point(475, 59);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 131);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Fish";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Name";
            // 
            // txtFishName
            // 
            this.txtFishName.Location = new System.Drawing.Point(47, 30);
            this.txtFishName.Name = "txtFishName";
            this.txtFishName.Size = new System.Drawing.Size(147, 20);
            this.txtFishName.TabIndex = 1;
            // 
            // btnSendFish
            // 
            this.btnSendFish.Location = new System.Drawing.Point(119, 98);
            this.btnSendFish.Name = "btnSendFish";
            this.btnSendFish.Size = new System.Drawing.Size(75, 23);
            this.btnSendFish.TabIndex = 0;
            this.btnSendFish.Text = "Send Fish";
            this.btnSendFish.UseVisualStyleBackColor = true;
            this.btnSendFish.Click += new System.EventHandler(this.btnSendFish_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Last Name";
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(66, 45);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(128, 20);
            this.txtLastName.TabIndex = 6;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chkIncludeDog);
            this.groupBox5.Controls.Add(this.chkIncludeCat);
            this.groupBox5.Controls.Add(this.chkIncludeFish);
            this.groupBox5.Location = new System.Drawing.Point(9, 103);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(185, 100);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Pets";
            // 
            // chkFreshWater
            // 
            this.chkFreshWater.AutoSize = true;
            this.chkFreshWater.Location = new System.Drawing.Point(9, 71);
            this.chkFreshWater.Name = "chkFreshWater";
            this.chkFreshWater.Size = new System.Drawing.Size(89, 17);
            this.chkFreshWater.TabIndex = 6;
            this.chkFreshWater.Text = "Is fresh water";
            this.chkFreshWater.UseVisualStyleBackColor = true;
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Location = new System.Drawing.Point(340, 22);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(100, 20);
            this.txtIpAddress.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(279, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "ip address";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 433);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtIpAddress);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSendDog;
        private System.Windows.Forms.Button btnSendPerson;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDogName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkIncludeDog;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.CheckBox chkIncludeFish;
        private System.Windows.Forms.CheckBox chkIncludeCat;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCatName;
        private System.Windows.Forms.Button btnSendCat;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFishName;
        private System.Windows.Forms.Button btnSendFish;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chkFreshWater;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label label6;
    }
}

