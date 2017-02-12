using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using P2PNET.TransportLayer;

namespace SendObjects
{
    public partial class Form1 : Form
    {
        private TransportManager transMgr;
        private int portNum = 8080;

        public Form1()
        {
            transMgr = new TransportManager(portNum, true);
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            transMgr.MsgReceived += TransMgr_MsgReceived;
            await transMgr.StartAsync();
        }

        private void TransMgr_MsgReceived(object sender, P2PNET.TransportLayer.EventArgs.MsgReceivedEventArgs e)
        {
            string receivedMsg = Encoding.ASCII.GetString(e.Message);
            AppendMsg("From ip = " + e.RemoteIp);
            AppendMsg(receivedMsg);
            AppendMsg("");
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            await SendMessageImp();
        }

        private void AppendMsg(string message)
        {
            txtReceivedMessages.AppendText(message + Environment.NewLine);
        }

        private async Task SendMsg(string ipAddress, string msgString)
        {
            byte[] msgBits = Encoding.ASCII.GetBytes(msgString);
            await transMgr.SendBroadcastAsyncUDP(msgBits);
        }

        private async void txtSendMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                await SendMessageImp();
            }

        }

        private async Task SendMessageImp()
        {
            string ipAddress = txtAddress.Text;
            string message = txtSendMessage.Text;

            await SendMsg(ipAddress, message);
            //clear message
            txtSendMessage.Text = "";
        }
    }
}
