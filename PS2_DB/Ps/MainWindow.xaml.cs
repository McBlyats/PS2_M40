using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
namespace Ps
{
    public partial class MainWindow : Window
    {
        static public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Vasile-Adrian TORJA\Documents\AutomatLogs.mdf;Integrated Security=True;Connect Timeout=30");
        public static System.Timers.Timer timy = new System.Timers.Timer(1800); 
        static byte[] buffers=new byte[10];
        static public int ID_DB = 0;
        static void calculateRows()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT COUNT(*) FROM Logs";
            ID_DB = (int)cmd.ExecuteScalar();
            con.Close();
        }
        static byte[] received=new byte[10];
        //static object state = 0;
        static Byte[] bytes = new Byte[16];
        private static TcpClient client;
        static IAsyncResult result;
        List<Rectangle> rectangles = new List<Rectangle>();
        int  nivMaxim = 5;
        static string simulat = "Da";
        bool sameState = false;
        static bool startProgram = true;
        static bool simulatButton = true;
        static void CurrentDomain_ProcessExit(object sender, EventArgs e){
            try{

                client.Close();
            }
            catch(Exception e1){}
        }
        static void calculateSimulation()
        {
            if (simulatButton == true){
                simulat = "Nu";
                simulatButton = false;
                MessageBox.Show("Simulat : Nu");
            }
            else{
                simulat = "Da";
                simulatButton = true;
                MessageBox.Show("Simulat : Da");
            }
        }
        static void writeFile(){
            string text="Nivel nedefinit.";
            switch (nivel){
                case 0:{
                        text = "Silozul este gol.";
                        break;}
                case 1:{
                        text = "Nivel 1";
                        break;}
                case 2:{
                        text = "Nivel 2";
                        break;}
                case 3:{
                        text = "Nivel 3";
                        break;}
                case 4:{
                        text = "Nivel 4";
                        break;}
                case 5:{
                        text = "Nivel 5";
                        break;}
            }
            System.IO.File.WriteAllText(@"C:\Users\Vasile-Adrian TORJA\Desktop\ASP_AUTOMAT\WebApplication2\data.txt", text);
            calculateRows();
            if (startProgram == false){
                ID_DB++;
                var time = DateTime.Now;
                string Ora = time.ToString("hh:mm");
                string Data = time.ToString("dd:MM:yyyy");
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "insert into Logs values('" + ID_DB.ToString() + "','" + text + "','" + Ora + "','" + Data + "','" + simulat + "')";
                cmd.ExecuteNonQuery();
                con.Close();
           }
        }
        public void  TcpThread(){
            Thread listenerThread = new Thread(tcp_listener);
            listenerThread.Start();
            Thread.Sleep(5000);
            //TcpClient client = new TcpClient(new IPEndPoint(IPAddress.Loopback, 1234));
        }
        public  static void tcp_listener(){
            try{
                Int32 port = 2000;
                IPAddress localAddr = IPAddress.Parse("192.168.0.101");
                TcpListener server = new TcpListener(localAddr, port);
                server.Start();
                String data = null;
                Boolean inaltime = false;
                while (true)
                {
                    Console.Write("Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    timy.Enabled = true;
                    data = null;
                    NetworkStream stream = client.GetStream();
                    //Byte[] bytes2 = new Byte[] {2,0,0,0,0,0,0,0,0,0};  //se umple
                    //MessageBox.Show(bytes2[0].ToString());
                    //  Byte[] bytes2 = new Byte[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // se goleste
                    // nivelul 1 : Byte[] bytes2 = new Byte[] {64,0,0,0,0,0,0,0,0,0}; 
                    // nivelul 2 : Byte[] bytes2 = new Byte[] {192,0,0,0,0,0,0,0,0,0}; 
                    // nivelul 3 : Byte[] bytes2 = new Byte[] {192,0,0,0,0,0,0,0,1,0}; 
                    // nivelul 4 : Byte[] bytes2 = new Byte[] {192,0,0,0,0,0,0,0,3,0}; 
                    // nivelul 5 : Byte[] bytes2 = new Byte[] {192,0,0,0,0,0,0,0,7,0}; 
                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0){
                    Restart:
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);    
                        //Console.WriteLine((String.Format("Received: {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",bytes[0],
                        //bytes[1],bytes[2],bytes[3],bytes[4],bytes[5],bytes[6],bytes[7],bytes[8],bytes[9])));
                        //byte[] msg = new byte[16];
                        //msg[0] = bytes[0];
                        //msg[1] = bytes[8];
                    if (bytes[0] == 64){
                        l1 = true;
                        nivel = 1;
                    }   
                    else if (bytes[0] == 192 && bytes[8] == 0){
                        l1 = true;
                        l2 = true;
                        nivel = 2;
                    }
                    else if (bytes[0] == 192 && bytes[8] == 1){
                        l1 = true;
                        l2 = true;
                        l3 = true;
                        nivel = 3;
                    }
                    else if (bytes[0] == 192 && bytes[8] == 3){
                        l1 = true;
                        l2 = true;
                        l3 = true;
                        l4 = true;
                        nivel = 4;
                    }
                    if (bytes[0] == 192 && bytes[8] == 7){
                        l1 = true;
                        l2 = true;
                        l3 = true;
                        l4 = true;
                        l5 = true;
                        nivel = 5;
                    }
                    //msg[0] = 0;
                    //msg[8] = 0;
                    //stream.Write(bytes2, 0, bytes2.Length);
                    stream.Write(received, 0, received.Length);
                    //stream.Read(buffers, 0, buffers.Length);
                    }
                    writeFile();
                    client.Close();
                }
            }
            catch (SocketException e){
                Console.WriteLine("SocketException: {0}", e);
            }          
            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        } 
        public static int  Opreste_Banda (){
            return 0;
        }
        private static int nivel=0;
        private static bool l1=false, l2=false, l3=false, l4=false, l5=false;
        private void BtnFill_Click(object sender, RoutedEventArgs e){
            nivel = 5;
            Byte[] bytes2 = new Byte[] {2,0,0,0,0,0,0,0,0,0};  //se umple
            // send 192 00 00 00 7 0
            Thread t1 = new Thread(new ThreadStart(
            delegate (){
                foreach (Rectangle rect in rectangles){
                    Dispatcher.BeginInvoke((Action)(() =>{
                            // foreach (Rectangle rect in rectangles)
                            if (rectangles.IndexOf(rect) < nivMaxim){
                                rect.Fill = new SolidColorBrush(Color.FromRgb(77, 165, 255));
                              //  LampFilling.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                            }
                            if (rect == rectangles.ElementAt(4)){
                                LampFull.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            }
                            //if(nivel==nivMaxim)
                            //    LampFilling.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                        }));
                    Thread.Sleep(4000);
                }
            }));
            t1.Start();
            l5 = true;
            l4 = true;
            l3 = true;
            l2 = true;
            l1 = true;
            for(int i=0;i<10;i++)
               received[i]=bytes2[i];
               TcpThread();
               writeFile();
        }
        private void BtnEmpty_Click(object sender, RoutedEventArgs e){   
            Byte[] bytes2 = new Byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; 
            for (int i = 0; i < 10; i++)
                received[i] = bytes2[i];
            TcpThread();
            Goleste();
            writeFile();
        }
        public void Goleste(int niv){
                while (nivel > niv){
                    Descarca();
                }  
             if(nivel<nivMaxim)
                LampFull.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            writeFile();
        }
        public void Descarca(){
            if (nivel == 5){
                Lev5.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                l5 = false;
                nivel--;
               Byte[] bytes2= new Byte[] {192,0,0,0,0,0,0,0,3,0};
            }
            else if (nivel == 4){
                Lev4.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                l4 = false;
                nivel--;
            }
            else if (nivel == 3){
                Lev3.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                l3 = false;
                nivel--;
            }
            else if (nivel == 2){
                Lev2.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                l2 = false;
                nivel--;
            }
            else if (nivel == 1){
                Lev1.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                l1 = false;
                nivel--;
            }
            else{
                MessageBox.Show("Silozul este gol!");
                sameState = true;
            }
            if (nivel < nivMaxim){
                LampFull.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            if (sameState == false){
                writeFile();
            }
            sameState = false;
        }
        public void Goleste(){
            nivel = 0;
            List<Rectangle> mylist = new List<Rectangle>();
            // send 64 00 00 00 0 0 cred
            Thread t2 = new Thread(new ThreadStart(
            delegate (){
                 foreach (Rectangle rect in rectangles.Reverse<Rectangle>()){
                     Dispatcher.BeginInvoke((Action)(() =>{
                         // foreach (Rectangle rect in rectangles)
                          rect.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));

                     }));
                     Thread.Sleep(2000);
                 }
             }));
            t2.Start();
            l5 = false;
            l4 = false;
            l3 = false;
            l2 = false;
            l1 = false;
            LampFull.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            //writeFile();
        }
        private void BtnUnload_Click(object sender, RoutedEventArgs e)
        {
            Descarca();
        }

        private void Lev4Btn_Checked(object sender, RoutedEventArgs e)
        {
            nivMaxim = 4;
            if (nivel > nivMaxim)
                Goleste(nivMaxim);
            if(nivel==nivMaxim)
                LampFull.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            else
                LampFull.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        }

        private void Lev2Btn_Checked(object sender, RoutedEventArgs e)
        {
            nivMaxim = 2;
            if (nivel > nivMaxim)
                Goleste(nivMaxim);
            if (nivel == nivMaxim)
                LampFull.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            else
                LampFull.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        }

        private void Lev3Btn_Checked(object sender, RoutedEventArgs e)
        {
            nivMaxim = 3;
            if (nivel > nivMaxim)
                Goleste(nivMaxim);
            if (nivel == nivMaxim)
                LampFull.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                else
                LampFull.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        }

        private void Lev1Btn_Checked(object sender, RoutedEventArgs e)
        {
            nivMaxim = 1;
            if (nivel > nivMaxim)
                Goleste(nivMaxim);
            if (nivel == nivMaxim)
                LampFull.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            else
                LampFull.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        }

        private void Lev5Btn_Checked(object sender, RoutedEventArgs e)
        {
            nivMaxim = 5;
            if (nivel == nivMaxim)
                LampFull.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            else
                LampFull.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            int count = 0;
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT COUNT(*) FROM Logs";
            count = (int)cmd.ExecuteScalar();
            con.Close();
            MessageBox.Show(count.ToString());
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DBForm f = new DBForm();
            f.Show();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            calculateSimulation();
            if (simulatButton == true)
            {
                BtnLoad.IsEnabled = true;
                BtnUnload.IsEnabled = true;
                BtnFill.IsEnabled = true;
                BtnEmpty.IsEnabled = true;

            }
            else
            {
                BtnLoad.IsEnabled = false;
                BtnUnload.IsEnabled = false;
                BtnFill.IsEnabled = false;
                BtnEmpty.IsEnabled = false;
            }
        }

        public MainWindow()
        {
            calculateRows();
            InitializeComponent();
            InitializeList();
            LampFull.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
           // tcp_listener();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            startProgram = true;
            writeFile();
            startProgram = false;
            //Draw_Border();
        }
        public void InitializeList()
        {
            rectangles.Add(Lev1);
            rectangles.Add(Lev2);
            rectangles.Add(Lev3);
            rectangles.Add(Lev4);
            rectangles.Add(Lev5);

        }
        public void Draw_Border()
        {
        }
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (!l1 && nivMaxim >= 1)
            {

                Lev1.Fill = new SolidColorBrush(Color.FromRgb(77, 165, 255));
                l1 = true;
                nivel++;
            }
            else if (l1 && !l2 && nivMaxim >= 2)
            {
                Lev2.Fill = new SolidColorBrush(Color.FromRgb(77, 165, 255));
                l2 = true;
                nivel++;
            }
            else if (l1 && l2 && !l3 && nivMaxim >= 3)
            {
                Lev3.Fill = new SolidColorBrush(Color.FromRgb(77, 165, 255));
                l3 = true;
                nivel++;
            }
            else if (l1 && l2 && l3 && !l4 && nivMaxim >= 4)
            {
                Lev4.Fill = new SolidColorBrush(Color.FromRgb(77, 165, 255));
                l4 = true;
                nivel++;
            }
            else if (l1 && l2 && l3 && l4 && !l5 && nivMaxim == 5)
            {
                Lev5.Fill = new SolidColorBrush(Color.FromRgb(77, 165, 255));
                l5 = true;
                LampFull.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                nivel++;
            }
            else
            {
                MessageBox.Show("Silozul este plin");
                sameState = true;

            }
            if (sameState == false)
            {
                writeFile();
                
            }
            sameState = false;
        }
       
    }
}
