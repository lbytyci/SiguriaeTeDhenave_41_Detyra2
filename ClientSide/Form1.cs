using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientSide
{



    public partial class Form1 : Form
    {
        static string abc1 = "C:\\publicKey.xml";
        static string abc2 = "C:\\privateKey.xml";
        static string _publicKey = System.IO.File.ReadAllText(abc1);
        static string _privateKey = System.IO.File.ReadAllText(abc2);
        static string _myRsaKeys = _publicKey;
        private string _senderPublicKey = _privateKey;


        UdpClient client = new UdpClient();

        IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

        public Form1()
        {
            
            InitializeComponent();
           // how to manipulate with form's design 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));

            byte[] sendr = Encoding.UTF8.GetBytes(RSAEncrypt(User.Text) + "," + RSAEncrypt(pass.Text));
            client.Send(sendr, sendr.Length);
            Console.WriteLine("Done Message sent by client");

            byte[] bx = client.Receive(ref groupEP);
            string decider = Encoding.UTF8.GetString(bx);
            Console.WriteLine("Okay Server" + decider);
            string[] logintokens = decider.Split(',');
            string loginer1 = "", loginer2 = "", loginer3 = "";
            for (int i = 0; i < logintokens.Length; i++)
            {

                if (i == 0)
                    loginer1 = logintokens[i];
                if (i == 1)
                    loginer2 = logintokens[i];
                if (i == 2)
                    loginer3 = logintokens[i];

            }
            if (loginer1.Equals("1"))
            {

                String decryptedText = new HarshSign().ExtractMessage(loginer2, loginer3);
                Console.WriteLine(decryptedText);
                String []textChanger= decryptedText.Split(',');
                string a1 = "", a2 = "", a3 = "", a4 = "";
                for(int i = 0; i < textChanger.Length; i++)
                {
                    if (i == 0)
                        a1 = textChanger[i];
                    if (i == 1)
                        a2 = textChanger[i];
                    if (i == 2)
                        a3 = textChanger[i];
                    if (i == 3)
                        a4 = textChanger[i];
                }
                li.Show(); ly.Show(); lm.Show(); le.Show();
                I.Show(); m.Show(); y.Show(); eu.Show();
                m.Text = a1;
                y.Text = a2;
                eu.Text = a3;
                I.Text = a4;
                MessageBox.Show(decryptedText);

            }
            else if (loginer1.Equals("0"))
            {
                Console.WriteLine("Error logging you in");
                MessageBox.Show("Error:- Logging you in ");
            }
            else
            {

            }

        }
        
