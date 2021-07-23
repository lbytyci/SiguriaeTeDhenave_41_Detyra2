using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace ServerSide
{

    class Program
    {
        private const int listenPort = 8080;
        private static void StartListener()

        {
            LinkedList<string> Name = new LinkedList<string>();
            LinkedList<string> Password = new LinkedList<string>();
            LinkedList<string> Super = new LinkedList<string>();

            string connStr = "server=localhost;user=root;database=udp;password=1243244338";

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {

                conn.Open();

                string sql = "select * from userinfo;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    Super.AddLast(rdr[0] + "," + rdr[1] + "," + rdr[2] + "," + rdr[3] + "," + rdr[4] + "," + rdr[5]);
                    Name.AddLast(rdr[0].ToString());
                    Password.AddLast(Class1.hashPassword(rdr[1].ToString()));

                }
                rdr.Close();

            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }

            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

            foreach (string s in Name)
            {
                Console.WriteLine(s);
            }

            foreach (string s in Password)
            {
                Console.WriteLine(s);
            }

            try
            {

                while (true)
                {
                    Console.WriteLine("Server started and Waiting for Clients!");
                    byte[] bytes = listener.Receive(ref groupEP);

                    Console.WriteLine("Recieved data........");

                    string a = Encoding.Default.GetString(bytes);
                    Console.WriteLine(a);
                    string[] loginValues = a.Split(',');
                    string user = "";
                    string pass = "";
                    if (loginValues.Length == 2)
                    {
                        for (int i = 0; i < loginValues.Length; i++)
                        {
                            if (i == 0)
                                user = loginValues[i];
                            if (i == 1)
                                pass = loginValues[i];
                        }
                        string decryptedUser = HarshCode.RSADecrypt(user);
                        string decryptedPassNothas = HarshCode.RSADecrypt(pass);
                        string decrypredPass = Class1.hashPassword(HarshCode.RSADecrypt(pass));
                        Console.WriteLine("Pas-" + decrypredPass);
                        Console.WriteLine("Username->" + decryptedUser);
                        Console.WriteLine("Password->" + decrypredPass);
                        if (Password.Contains(decrypredPass) && Name.Contains(decryptedUser))
                        {
                            Console.WriteLine("Sign a digital document");
                            HarshSign sender = new HarshSign();
                            string BigB = Extraction(decryptedUser, decryptedPassNothas);
                            DigitalSignatureResult res = sender.BuildSignedMessage(BigB);
                            Console.WriteLine("cipher" + res.CipherText);
                            Console.WriteLine("cSigner" + res.SignatureText);
                            byte[] ciphervalues = Encoding.UTF8.GetBytes(1 + "," + res.CipherText + "," + res.SignatureText);
                            listener.Send(ciphervalues, ciphervalues.Length, groupEP);



                        }
                        else
                        {
                            byte[] login = Encoding.UTF8.GetBytes("0,0,0");
                            listener.Send(login, login.Length, groupEP);
                        }
                    }
                    else
                    {
                        string regis1 = "", regis2 = "", regis3 = "", regis4 = "";
                        for (int i = 0; i < loginValues.Length; i++)
                        {
                            if (i == 0)
                                user = loginValues[i];
                            if (i == 1)
                                pass = loginValues[i];
                            if (i == 2)
                                regis1 = loginValues[i];
                            if (i == 3)
                                regis2 = loginValues[i];
                            if (i == 4)
                                regis3 = loginValues[i];
                            if (i == 5)
                                regis4 = loginValues[i];

                        }
                        RegisterUser(user, pass, regis1, regis2, regis3, regis4);

                    }
                }
            }

            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
        }

        public static string Extraction(string name, string pass)
        {
            string value = "";
            string connStr = "server=localhost;user=root;database=udp;password=1243244338";

            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();

            string sql = "select * from userinfo where (Name='" + name + "'and Password='" + pass + "');";
            Console.WriteLine(name + " " + pass);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            string a1 = "", a2 = "", a3 = "", a4 = "";

            while (rdr.Read())
            {

                a1 = rdr[2].ToString();
                a2 = rdr[3].ToString();
                a3 = rdr[4].ToString();
                a4 = rdr[5].ToString();

            }
            rdr.Close();
            


            value = a1 + "," + a2 + "," + a3 + "," + a4;
            return value;
            //John | Hello world
        }
        public static void RegisterUser(string name, string pass, string month, string year, string euro, string Invoice)
        {
            string value = "";
            //server=localhost;user id=root;persistsecurityinfo=True;database=udp
            string connStr = "server=localhost;user=root;database=udp;password=1243244338";

            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();
           

            string sql = "Insert into userinfo  values(" + "'" + name + "','" + pass + "','" + month + "','" + year + "','" + euro + "','" + Invoice + "');";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            conn.Close();
            Console.WriteLine("Successfully registered...........");
        }
        static void Main(string[] args)
        {
            StartListener();


        }
    }

}
