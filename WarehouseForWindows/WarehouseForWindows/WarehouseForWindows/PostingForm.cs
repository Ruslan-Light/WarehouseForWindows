using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public partial class PostingForm : Form
    {

        static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Руслан\Documents\С#\WarehouseForWindows\WarehouseForWindows\WarehouseForWindows\Database.mdf;Integrated Security=True";
        public static SqlConnection connection = new SqlConnection(connectionString);
        string selectProduct;
        string selectDbCategory = "SELECT id, article_number, name, category FROM Products";

        public PostingForm()
        {
            InitializeComponent();
        }


        private async void PostingForm_Load(object sender, EventArgs e)
        {
            await connection.OpenAsync();

            SqlDataAdapter adapter = new SqlDataAdapter(selectDbCategory, connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Артикул";
            dataGridView1.Columns[2].HeaderText = "Название";
            dataGridView1.Columns[3].HeaderText = "Категория";

            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 60;
            dataGridView1.Columns[2].Width = 200;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Data.someValue.Add((Convert.ToInt32(dataGridView1.SelectedCells[0].Value)));
            selectProduct = "SELECT id, article_number, name, category FROM Products WHERE id IN (" + string.Join(",", Data.someValue) + ")";

            SqlDataAdapter adapter = new SqlDataAdapter(selectProduct, connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            dataGridView2.DataSource = dataSet.Tables[0];
            dataGridView2.Columns[0].HeaderText = "Кол-во";
            dataGridView2.Columns[1].HeaderText = "ID";
            dataGridView2.Columns[2].HeaderText = "Артикул";
            dataGridView2.Columns[3].HeaderText = "Название";

            dataGridView2.Columns[1].Width = 60;

            dataGridView2.Columns[0].DisplayIndex = 4;
            dataGridView2.Columns[4].HeaderText = "Категория";
            dataGridView2.Columns[0].Visible = true;

            dataGridView2.Columns[0].Width = 60;
            dataGridView2.Columns[1].Width = 40;
            dataGridView2.Columns[2].Width = 50;
            dataGridView2.Columns[3].Width = 200;

            dataGridView2.Columns[1].ReadOnly = true;
            dataGridView2.Columns[2].ReadOnly = true;
            dataGridView2.Columns[3].ReadOnly = true;
            dataGridView2.Columns[4].ReadOnly = true;

        }

        
        private void button4_Click(object sender, EventArgs e)
        {
            Data.delValue = ((Convert.ToInt32(dataGridView2.SelectedCells[0].Value)));
            Data.someValue.Remove(Data.delValue);


            if (Data.someValue.Count == 0)
            {
                MessageBox.Show("Таблица пуста!");
                this.Close();
                return;
            }

            if (Data.someValue.Count == 1)
            {
                selectProduct = "SELECT id, article_number, name, category FROM Products WHERE id = " + string.Join(",", Data.someValue);
            }
            if(Data.someValue.Count > 1)
            {
                selectProduct = "SELECT id, article_number, name, category FROM Products WHERE id IN (" + string.Join(",", Data.someValue) + ")";
            }
            SqlDataAdapter adapter = new SqlDataAdapter(selectProduct, connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            dataGridView2.DataSource = dataSet.Tables[0];
            dataGridView2.Columns[0].HeaderText = "Кол-во";
            dataGridView2.Columns[1].HeaderText = "ID";
            dataGridView2.Columns[2].HeaderText = "Артикул";
            dataGridView2.Columns[3].HeaderText = "Название";

            dataGridView2.Columns[1].Width = 60;

            dataGridView2.Columns[0].DisplayIndex = 4;
            dataGridView2.Columns[4].HeaderText = "Категория";
            dataGridView2.Columns[0].Visible = true;

            dataGridView2.Columns[0].Width = 60;
            dataGridView2.Columns[1].Width = 40;
            dataGridView2.Columns[2].Width = 50;
            dataGridView2.Columns[3].Width = 200;

            // Добавить обработку исключений. В данный момент удалять элемент из таблиц можно лишь выбрав ячейку в столбце ID
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            selectDbCategory = "SELECT id, article_number, name, category FROM Products WHERE name LIKE N'%" + textBox1.Text + "%'";

            SqlDataAdapter adapter = new SqlDataAdapter(selectDbCategory, connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Артикул";
            dataGridView1.Columns[2].HeaderText = "Название";
            dataGridView1.Columns[3].HeaderText = "Категория";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string productList = "";
            
            SqlCommand sqlCommand = new SqlCommand();
            
            //  Добавляем товар в базу данных. Изменяем количество в стобце "remains".
            
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                if(dataGridView2.Rows[i].Cells[0].Value == null)
                {
                    dataGridView2.Rows[i].Cells[0].Value = 0; // Добавить проверку! Что бы можно было вводить только числа.
                }

                if (dataGridView2.Rows[i] == dataGridView2.Rows[dataGridView2.Rows.Count - 2])
                {
                    productList += dataGridView2.Rows[i].Cells[3].Value.ToString() + " - " + dataGridView2.Rows[i].Cells[0].Value.ToString();
                }
                else
                {
                    productList += dataGridView2.Rows[i].Cells[3].Value.ToString() + " - " + dataGridView2.Rows[i].Cells[0].Value.ToString() + ", \r\n";
                }

                sqlCommand.CommandText = "UPDATE Products SET remains = (remains + " + dataGridView2.Rows[i].Cells[0].Value.ToString() + ") WHERE id = " + dataGridView2.Rows[i].Cells[1].Value.ToString();
                sqlCommand.Connection = connection;
                sqlCommand.ExecuteNonQuery();
                
                
            }
            // Добавляем товар в базу данных. Изменяем количество в стобце "remains".   -- end!


            // Добавляем новую запись в БД

            if (textBox2.Text == null)
            {
                sqlCommand.CommandText = "INSERT INTO Postings (posting_date, products_list) VALUES (GETDATE(), N'" + productList + "')";
            }
            if (textBox2.Text != null)
            {
                sqlCommand.CommandText = "INSERT INTO Postings (posting_date, invoice_number, products_list) VALUES (GETDATE(), N'" + textBox2.Text + "', N'" + productList + "')";
            }

            sqlCommand.Connection = connection;
            sqlCommand.ExecuteNonQuery();

            // Добавляем новую запись в БД  -- end!


            Data.someValue.Clear();
            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();
            this.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Data.someValue.Clear();
            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();
            this.Close();
        }

        private void PostingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Data.someValue.Clear();

            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();

        }
    }

}
