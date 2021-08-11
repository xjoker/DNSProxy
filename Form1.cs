using System;
using System.Windows.Forms;
using DNSProxy.Utils;

namespace DNSProxy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dns = new DnsServer();
            dns.Initialize();
            dns.Start();
        }
    }
}