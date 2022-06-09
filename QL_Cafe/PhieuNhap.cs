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
    public partial class PhieuNhap : Form
    {
        private DataTable phieuNhaps = new DataTable();
        public PhieuNhap()
        {
            InitializeComponent();
            CreatePNtable();
            dataGridView1.DataSource = phieuNhaps;
        }
        public void CreatePNtable()
        {
            if (System.IO.File.Exists("PhieuNhaps.json"))
            {
                System.IO.StreamReader reader = new System.IO.StreamReader("PhieuNhaps.json");
                string jsonStr = reader.ReadToEnd();
                reader.Close();

                if (jsonStr != "")
                {
                    phieuNhaps = JsonConvert.DeserializeObject<DataTable>(jsonStr);
                }
            }
            if (phieuNhaps.Columns.Count == 0)
            {
                phieuNhaps.Columns.Add("ID", typeof(string));
                phieuNhaps.Columns.Add("Name", typeof(string));
                phieuNhaps.Columns.Add("Price", typeof(string));
                phieuNhaps.Columns.Add("Quantity", typeof(int));
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox8.Text == "" || textBox7.Text == "" || textBox9.Text == "" || textBox10.Text == "" || textBox1.Text  == "") {
                MessageBox.Show("Hãy nhập thông tin hợp lệ");
            } else {
                bool check = false;
                foreach (DataRow dataRow in phieuNhaps.Rows)
                {
                    if (dataRow["ID"].ToString() == textBox7.Text)
                    {
                        MessageBox.Show("ID sản phẩm đã bị trùng");
                        check = true;
                    }
                }
                if (check == false)
                {
                    phieuNhaps.Rows.Add(textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text);
                    MessageBox.Show("Thêm sản phẩm thành công");
                } }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (phieuNhaps.Rows.Count > 0)
            {
                string jsonStr = JsonConvert.SerializeObject(phieuNhaps);
                System.IO.File.WriteAllText("PhieuNhaps.json", jsonStr);
                MessageBox.Show("Lưu phiếu nhập thành công");
            }
            else
            {
                MessageBox.Show("Lưu phiếu không thành công");
            }
        }
    }
}
