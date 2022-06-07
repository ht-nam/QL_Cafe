using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
namespace QL_Cafe
{
    public partial class XuatHoaDon : Form
    {
        private DataTable dt = new DataTable();
        private DataTable NhanViens = new DataTable();
        FormDangNhap fdn = new FormDangNhap();
        public XuatHoaDon(DataGridView dataGridView, DataTable dataTable)
        {
            InitializeComponent();
            dataGridView1.DataSource = dataGridView.DataSource;
            dt = dataTable;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.DataSource = dt;
            label3.Text = label3.Text + totalAmountOfMoney();
        }

        public string totalAmountOfMoney()
        {
            int money = 0;
            foreach (DataRow row in dt.Rows)
            {
                money += Convert.ToInt32(row["QTT"].ToString()) * Convert.ToInt32(row["Price"].ToString());
            }

            return money.ToString();
        }

        private void XuatHoaDon_Load(object sender, EventArgs e)
        {
            tenNV.Text = FormDangNhap.passingText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
