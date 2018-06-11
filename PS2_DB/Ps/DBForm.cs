using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Ps
{
    public partial class DBForm : Form
    {
        int ID_DB = 0;
        static public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Vasile-Adrian TORJA\Documents\AutomatLogs.mdf;Integrated Security=True;Connect Timeout=30");
        void calculateRows(){
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT COUNT(*) FROM Logs";
            ID_DB = (int)cmd.ExecuteScalar();
            con.Close();
        }
        void deleteRow(){
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Vasile-Adrian TORJA\Documents\AutomatLogs.mdf;Integrated Security=True;Connect Timeout=30");
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "DELETE FROM Logs WHERE Id_Log=(SELECT MAX(Id_Log) FROM Logs)";
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public DBForm(){
            InitializeComponent();
            calculateRows();
        }

        private void DBForm_Load(object sender, EventArgs e){
            this.logsTableAdapter.Fill(this.automatLogsDataSet.Logs);
        }
        private void button1_Click(object sender, EventArgs e){
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e){
            deleteRow();
            this.logsTableAdapter.Fill(this.automatLogsDataSet.Logs);
        }
        private void button3_Click(object sender, EventArgs e){
            while (ID_DB != 0){
                deleteRow();
                ID_DB--;
            }
            this.logsTableAdapter.Fill(this.automatLogsDataSet.Logs);
        }
    }
}
