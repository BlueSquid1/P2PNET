using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using P2P_Advance_Sample.Messages;

namespace P2P_Advance_Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //48 bytes with BSON
            //47 bytes with JSON

            byte[] data = { 48, 25, 155, 250, 13, 0, 133, 63, 143, 62, 0, 25, 125, 57, 220 };
            FileMessage fileMsg = new FileMessage(data);
            byte[] payload = Serializer.SerializeObjectBSON(fileMsg);
            FileMessage returnObj =  Serializer.DeserializeObjectBSON< FileMessage>(payload);
            Console.WriteLine(payload);
        }
    }
}
