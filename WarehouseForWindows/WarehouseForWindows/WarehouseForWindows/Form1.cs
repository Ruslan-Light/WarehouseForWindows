using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Form1 : Form
    {
        static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Руслан\Documents\С#\WarehouseForWindows\WarehouseForWindows\WarehouseForWindows\Database.mdf;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);
        string selectDbCategory = "SELECT * FROM Products";
        string selectDbPostings = "SELECT * FROM Postings";
        string selectDbWriteOff = "SELECT * FROM WriteOff";
        

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await connection.OpenAsync();

            if (tabControl1.SelectedTab == tabPage1)
            {
                Remains remains = new Remains();
                remains.getAllProducts(connection, selectDbCategory, dataGridView1);
            }
            if (tabControl1.SelectedTab == tabPage2)
            {
                Postings postings = new Postings();
                postings.getAllDocuments(connection, selectDbPostings, dataGridView2);
            }
            if (tabControl1.SelectedTab == tabPage3)
            {
                Postings writeOff = new Postings();
                writeOff.getAllDocuments(connection, selectDbWriteOff, dataGridView3);
            }


            /*Remains remains = new Remains();
            remains.getAllProducts(connection, selectDbCategory, dataGridView1);

            Postings postings = new Postings();
            postings.getAllDocuments(connection, selectDbPostings, dataGridView2);

            Postings writeOff = new Postings();
            writeOff.getAllDocuments(connection, selectDbWriteOff, dataGridView3);*/

        }

        


        // --------------  Остатки
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Remains remains = new Remains();
            if (comboBox1.Text == "Все товары" && comboBox2.Text == "Все")
            {
                selectDbCategory = "SELECT * FROM Products";
                remains.getAllProducts(connection, selectDbCategory, dataGridView1);
                return;
            }

            if (comboBox1.Text == "Все товары" && comboBox2.Text == "В наличии")
            {
                selectDbCategory = "SELECT * FROM Products WHERE (remains > 0)";
                remains.getAllProducts(connection, selectDbCategory, dataGridView1);
                return;
            }

            if (comboBox1.Text == "Все товары" && comboBox2.Text == "Нет на складе")
            {
                selectDbCategory = "SELECT * FROM Products WHERE (remains = 0)";
                remains.getAllProducts(connection, selectDbCategory, dataGridView1);
                return;
            }

            if(comboBox1.Text == "Все товары" && comboBox2.Text == "Меньше мин. остатка")
            {
                selectDbCategory = "SELECT * FROM Products WHERE (remains < min_remains)";
                remains.getAllProducts(connection, selectDbCategory, dataGridView1);
                return;
            }

            if (comboBox2.Text == "Все")
            {
                selectDbCategory = "SELECT * FROM Products WHERE category = N'" + comboBox1.Text + "'";
            }

            if(comboBox2.Text == "В наличии")
            {
                selectDbCategory = "SELECT * FROM Products WHERE category = N'" + comboBox1.Text + "' AND (remains > 0)";
            }

            if (comboBox2.Text == "Нет на складе")
            {
                selectDbCategory = "SELECT * FROM Products WHERE category = N'" + comboBox1.Text + "' AND (remains = 0)";
            }

            if(comboBox2.Text == "Меньше мин. остатка")
            {
                selectDbCategory = "SELECT * FROM Products WHERE category = N'" + comboBox1.Text + "' AND (remains < min_remains)";
            }

            remains.getAllProducts(connection, selectDbCategory, dataGridView1);
        }

        // Поиск товаров по названию
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            selectDbCategory = "SELECT * FROM Products WHERE name LIKE N'%" + textBox1.Text + "%'";

            Remains remains = new Remains();
            remains.getAllProducts(connection, selectDbCategory, dataGridView1);

        }



        // --------------  Остатки end



        // -------------- Оприходования

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Postings postings = new Postings();

            if(comboBox3.Text == "За всё время")
            {
                selectDbPostings = "SELECT * FROM Postings";
            }

            if(comboBox3.Text == "За сегодня")
            {
                selectDbPostings = "SELECT * FROM Postings WHERE posting_date = CONVERT(date, GETDATE())";
            }

            if(comboBox3.Text == "За последние 3 дня")
            {
                selectDbPostings = "SELECT * FROM Postings WHERE posting_date >= DATEADD(day, -3, GETDATE())";
            }

            if (comboBox3.Text == "За последнюю неделю")
            {
                selectDbPostings = "SELECT * FROM Postings WHERE posting_date >= DATEADD(day, -7, GETDATE())";
            }

            if (comboBox3.Text == "За последний месяц")
            {
                selectDbPostings = "SELECT * FROM Postings WHERE posting_date >= DATEADD(day, -30, GETDATE())";
            }

            if (comboBox3.Text == "За последние 6 месяцев")
            {
                selectDbPostings = "SELECT * FROM Postings WHERE posting_date >= DATEADD(day, -180, GETDATE())";
            }

            if (comboBox3.Text == "За последний год")
            {
                selectDbPostings = "SELECT * FROM Postings WHERE posting_date >= DATEADD(day, -365, GETDATE())";
            }

            postings.getAllDocuments(connection, selectDbPostings, dataGridView2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostingForm postingForm = new PostingForm();
            postingForm.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Postings postings = new Postings();
            postings.getAllDocuments(connection, selectDbPostings, dataGridView2);
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            selectDbPostings = "SELECT * FROM Postings WHERE invoice_number LIKE N'%" + textBox2.Text + "%'";

            Postings postings = new Postings();
            postings.getAllDocuments(connection, selectDbPostings, dataGridView2);
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            Data.IdPosting = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();

            PostingShow postingShow = new PostingShow();
            postingShow.Show();


        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if(tabControl1.SelectedTab == tabPage1)
            {
                Remains remains = new Remains();
                remains.getAllProducts(connection, selectDbCategory, dataGridView1);
            }
            if (tabControl1.SelectedTab == tabPage2)
            {
                Postings postings = new Postings();
                postings.getAllDocuments(connection, selectDbPostings, dataGridView2);
            }
            if (tabControl1.SelectedTab == tabPage3)
            {
                Postings writeOff = new Postings();
                writeOff.getAllDocuments(connection, selectDbWriteOff, dataGridView3);
            }
            
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Postings writeOff = new Postings();

            if (comboBox3.Text == "За всё время")
            {
                selectDbPostings = "SELECT * FROM WriteOff";
            }

            if (comboBox3.Text == "За сегодня")
            {
                selectDbPostings = "SELECT * FROM WriteOff WHERE posting_date = CONVERT(date, GETDATE())";
            }

            if (comboBox3.Text == "За последние 3 дня")
            {
                selectDbPostings = "SELECT * FROM WriteOff WHERE posting_date >= DATEADD(day, -3, GETDATE())";
            }

            if (comboBox3.Text == "За последнюю неделю")
            {
                selectDbPostings = "SELECT * FROM WriteOff WHERE posting_date >= DATEADD(day, -7, GETDATE())";
            }

            if (comboBox3.Text == "За последний месяц")
            {
                selectDbPostings = "SELECT * FROM WriteOff WHERE posting_date >= DATEADD(day, -30, GETDATE())";
            }

            if (comboBox3.Text == "За последние 6 месяцев")
            {
                selectDbPostings = "SELECT * FROM WriteOff WHERE posting_date >= DATEADD(day, -180, GETDATE())";
            }

            if (comboBox3.Text == "За последний год")
            {
                selectDbPostings = "SELECT * FROM WriteOff WHERE posting_date >= DATEADD(day, -365, GETDATE())";
            }

            writeOff.getAllDocuments(connection, selectDbWriteOff, dataGridView3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Postings writeOff = new Postings();
            writeOff.getAllDocuments(connection, selectDbWriteOff, dataGridView3);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            selectDbWriteOff = "SELECT * FROM WriteOff WHERE invoice_number LIKE N'%" + textBox3.Text + "%'";

            Postings writeOff = new Postings();
            writeOff.getAllDocuments(connection, selectDbWriteOff, dataGridView3);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WriteOffForm writeOffForm = new WriteOffForm();
            writeOffForm.Show();
        }

        private void dataGridView3_DoubleClick(object sender, EventArgs e)
        {
            Data.IdWriteOff = dataGridView3.SelectedRows[0].Cells[0].Value.ToString();

            WriteOffShow writeOffShow = new WriteOffShow();
            writeOffShow.Show();
        }

        // Добавить новый товар
        private void button5_Click(object sender, EventArgs e)
        {
            AddNewProduct addNewProduct = new AddNewProduct();
            addNewProduct.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ChangeMinRemains changeMinRemains = new ChangeMinRemains();
            changeMinRemains.Show();
        }
    }

    static class Data
    {
        public static List<int> someValue = new List<int>();
        //public static List<int> writeOffValue = new List<int>();
        public static int delValue;

        public static string IdPosting;
        public static string IdWriteOff;
    }

}
