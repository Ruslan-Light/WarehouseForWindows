using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarehouseForWindows
{
    public partial class WriteOffShow : Form
    {
        static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Руслан\Documents\С#\WarehouseForWindows\WarehouseForWindows\WarehouseForWindows\Database.mdf;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);
        string selectDbWriteOff = "SELECT * FROM WriteOff WHERE id = " + Data.IdWriteOff;

        public WriteOffShow()
        {
            InitializeComponent();
        }

        private async void WriteOffShow_Load(object sender, EventArgs e)
        {

            SqlCommand sqlCommand = new SqlCommand();

            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(selectDbWriteOff, connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    label4.Text = reader["writeOff_date"].ToString();
                    label5.Text = reader["invoice_number"].ToString();
                    textBox1.Text = reader["products_list"].ToString();

                }

            }
            reader.Close();

        }

        private void WriteOffShow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();
        }
    }
}
