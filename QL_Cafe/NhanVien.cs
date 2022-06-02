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
    public partial class NhanVien : Form
    {
        private DataTable sanPham = new DataTable();
        private int n = 0;
        public NhanVien()
        {
            InitializeComponent();
            CreateSPtable();
            dataGridView1.DataSource = sanPham;
        }

        public void CreateSPtable()
        {
            if (System.IO.File.Exists("SanPhams.json"))
            {
                System.IO.StreamReader reader = new System.IO.StreamReader("SanPhams.json");
                string jsonStr = reader.ReadToEnd();
                reader.Close();

                if (jsonStr != "")
                {
                    sanPham = JsonConvert.DeserializeObject<DataTable>(jsonStr);
                }
            }
            if (sanPham.Columns.Count == 0)
            {
                sanPham.Columns.Add("Name", typeof(string));
                sanPham.Columns.Add("Price", typeof(string));
                sanPham.Columns.Add("Quantity", typeof(int));
            }
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if(dataGridView1.Rows.Count != n + 1)
                {
                    dataGridView2.Rows.Add();
                    dataGridView2.Rows[n].Cells[0].Value = row.Cells[1].Value.ToString();
                    dataGridView2.Rows[n].Cells[2].Value = row.Cells[2].Value.ToString();
                }
                n++;
            }
        }
    }
}
