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
    public partial class PostingShow : Form
    {
        static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Руслан\Documents\С#\WarehouseForWindows\WarehouseForWindows\WarehouseForWindows\Database.mdf;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);
        string selectDbPosting = "SELECT * FROM Postings WHERE id = " + Data.IdPosting;

        public PostingShow()
        {
            InitializeComponent();
        }

        private async void PostingShow_Load(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand();

            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(selectDbPosting, connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while(await reader.ReadAsync())
                {
                    label4.Text = reader["posting_date"].ToString();
                    label5.Text = reader["invoice_number"].ToString();
                    textBox1.Text = reader["products_list"].ToString();
                    
                }

            }
            reader.Close();
        }

        private void PostingShow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();
        }
    }
}
