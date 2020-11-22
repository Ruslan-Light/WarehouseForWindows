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
    class Remains
    {
        
        public void getAllProducts(SqlConnection connection, string selectDbCategory, DataGridView dataGridView)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(selectDbCategory, connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            dataGridView.DataSource = dataSet.Tables[0];
            dataGridView.Columns[0].HeaderText = "ID";
            dataGridView.Columns[1].HeaderText = "Артикул";
            dataGridView.Columns[2].HeaderText = "Название";
            dataGridView.Columns[3].HeaderText = "Категория";
            dataGridView.Columns[4].HeaderText = "Остатки";
            dataGridView.Columns[5].HeaderText = "Минимальный остаток";

            for (int i = 0; i < dataGridView.RowCount; i++)
            {
                if (Convert.ToInt32(dataGridView.Rows[i].Cells[4].Value) < Convert.ToInt32(dataGridView.Rows[i].Cells[5].Value))
                {
                    dataGridView.Rows[i].Cells[4].Style.BackColor = System.Drawing.Color.Red;
                }
            }

        }

        
    }
}
