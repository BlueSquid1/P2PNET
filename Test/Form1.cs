using P2PNET.ApplicationLayer;
using System;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        private ObjectManager objectManager;
        public Form1()
        {
            objectManager = new ObjectManager();
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await objectManager.StartAsync();
        }

        private void SendObj_Click(object sender, EventArgs e)
        {
            Person person = new Test.Person("Phillip", "King", 20);
            objectManager.SendObjBroadcastUDP(person);
        }
    }
}
