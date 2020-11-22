using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WarehouseForWindows
{
    class Postings
    {
        public void getAllDocuments(SqlConnection connection, string selectDbCategory, DataGridView dataGridView)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(selectDbCategory, connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            dataGridView.DataSource = dataSet.Tables[0];
            dataGridView.Columns[0].HeaderText = "ID";
            dataGridView.Columns[1].HeaderText = "Дата";
            dataGridView.Columns[2].HeaderText = "Накладная №";
            dataGridView.Columns[3].HeaderText = "Товары";

            dataGridView.Columns[0].Width = 60;
            dataGridView.Columns[1].Width = 140;
            dataGridView.Columns[2].Width = 120;
            dataGridView.Columns[3].Width = 300;
        }
    }
}
