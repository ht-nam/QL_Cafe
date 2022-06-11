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
        private int money = 0;
        public XuatHoaDon(DataGridView dataGridView, DataTable dataTable)
        {
            InitializeComponent();
            dataGridView1.DataSource = dataGridView.DataSource;
            dt = dataTable;
            //dataGridView1.Columns[0].Visible = false;
            dataGridView1.DataSource = dt;
            totalAmountOfMoney();
            label3.Text = label3.Text + money.ToString();
        }

        public void totalAmountOfMoney()
        {
            foreach (DataRow row in dt.Rows)
            {
                money += Convert.ToInt32(row["QTT"].ToString()) * Convert.ToInt32(row["Price"].ToString());
            }
        }

        private void XuatHoaDon_Load(object sender, EventArgs e)
        {
            label7.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            tenNV.Text = FormDangNhap.passingText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            DataTable dtHoaDon = new DataTable();
            CreateHDtable(dtHoaDon);
        }
        public void CreateHDtable(DataTable dtHoaDon)
        {
            if (System.IO.File.Exists("HoaDons.json"))
            {
                System.IO.StreamReader reader = new System.IO.StreamReader("HoaDons.json");
                string jsonStr = reader.ReadToEnd();
                reader.Close();

                if (jsonStr != "")
                {
                    dtHoaDon = JsonConvert.DeserializeObject<DataTable>(jsonStr);
                }
            }
            if (dtHoaDon.Columns.Count == 0)
            {
                dtHoaDon.Columns.Add("ID", typeof(string));
                dtHoaDon.Columns.Add("IDNV", typeof(string));
                dtHoaDon.Columns.Add("TotalPrice", typeof(int));
                dtHoaDon.Columns.Add("Date", typeof(string));
            }
            dtHoaDon.Rows.Add("HD" + (dtHoaDon.Rows.Count + 1).ToString(), FormDangNhap.passingText, money, DateTime.Now.ToString());
            string jsonStr1 = JsonConvert.SerializeObject(dtHoaDon);
            System.IO.File.WriteAllText("HoaDons.json", jsonStr1);
            
        }
    }
}
