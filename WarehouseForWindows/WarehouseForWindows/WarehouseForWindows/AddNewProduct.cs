using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarehouseForWindows
{
    public partial class AddNewProduct : Form
    {
        static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Руслан\Documents\С#\WarehouseForWindows\WarehouseForWindows\WarehouseForWindows\Database.mdf;Integrated Security=True";
        public static SqlConnection connection = new SqlConnection(connectionString);

        public AddNewProduct()
        {
            InitializeComponent();
        }

        private async void AddNewProduct_Load(object sender, EventArgs e)
        {
            await connection.OpenAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Добавить валидацию полей


            SqlCommand sqlCommand = new SqlCommand();

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                sqlCommand.CommandText = "INSERT INTO Products (article_number, name, category, remains, min_remains) VALUES (N'" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "', N'" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "', N'" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "', N'" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "', N'" + dataGridView1.Rows[i].Cells[4].Value.ToString() + "')";
                sqlCommand.Connection = connection;
                sqlCommand.ExecuteNonQuery();
            }


            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();

            this.Close();
        }

        
    }
}
