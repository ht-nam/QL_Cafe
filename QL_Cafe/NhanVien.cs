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
        public static bool button1WasClicked = false;
        private DataTable sanPham = new DataTable();
        private DataTable selectedSP = new DataTable();
        //private DataTable hoaDon
        private string ID = "";
        public NhanVien()
        {
            InitializeComponent();
            CreateSPtable();
            dataGridView1.DataSource = sanPham;
            dataGridView1.Columns[0].Visible = false;
            dataGridView2.DataSource = selectedSP;

            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns[0].Visible = false;

            //Thêm đoạn code này sau khi xuất hóa đơn để lưu dữ liệu
            //string jsonStr = JsonConvert.SerializeObject(sanPham);
            //System.IO.File.WriteAllText("SanPhams.json", jsonStr);
        }

        public void CreateSPtable()
        {
            selectedSP.Columns.Add("ID");
            selectedSP.Columns.Add("Name");
            selectedSP.Columns.Add("Price");
            selectedSP.Columns.Add("QTT");

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
                sanPham.Columns.Add("ID",typeof(string));
                sanPham.Columns.Add("Name", typeof(string));
                sanPham.Columns.Add("Price", typeof(string));
                sanPham.Columns.Add("Quantity", typeof(int));
            }
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value.ToString() == "0" || ID == "")
            {
                MessageBox.Show("Bạn chưa chọn sản phẩm hoặc số lượng");
            } else {
                bool check = false;
                foreach (DataRow row in sanPham.Rows)
                {
                    if (ID != "" && ID == row["ID"].ToString())
                    {
                        if (Convert.ToInt32(numericUpDown1.Value.ToString()) > Convert.ToInt32(row["Quantity"].ToString()))
                        {
                            MessageBox.Show("Vượt quá số lượng tồn kho");
                        }
                        else
                        {
                            if (selectedSP.Rows.Count == 0)
                            {
                                selectedSP.Rows.Add(row["ID"], row["Name"], row["Price"], numericUpDown1.Value.ToString());
                                row["Quantity"] = Convert.ToInt32(row["Quantity"].ToString()) - Convert.ToInt32(numericUpDown1.Value.ToString());                             
                            } else
                            {
                                bool ch = false;
                                foreach (DataRow r1 in selectedSP.Rows)
                                {
                                    if (r1["ID"].ToString() == ID)
                                    {
                                        row["Quantity"] = Convert.ToInt32(row["Quantity"].ToString()) - Convert.ToInt32(numericUpDown1.Value.ToString());
                                        r1["QTT"] = Convert.ToInt32(r1["QTT"].ToString()) + Convert.ToInt32(numericUpDown1.Value.ToString());
                                        ch = true;
                                    }
                                }
                                if (ch == false)
                                {
                                    selectedSP.Rows.Add(row["ID"], row["Name"], row["Price"], numericUpDown1.Value.ToString());
                                    row["Quantity"] = Convert.ToInt32(row["Quantity"].ToString()) - Convert.ToInt32(numericUpDown1.Value.ToString());
                                }
                            }

                            dataGridView1.DataSource = sanPham;
                            dataGridView2.DataSource = selectedSP;
                            numericUpDown1.Value = 0;
                            check = true;
                            ID = "";
                        }
                    }
                }
                if (check == false)
                {
                    MessageBox.Show("Chọn sản phẩm không thành công");
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ID = dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value.ToString();
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {
            DataRow dlRow = null;
            if (ID == "")
            {
                MessageBox.Show("Bạn chưa chọn sản phẩm");
            } else
            {
                bool check = false;
                foreach (DataRow row in selectedSP.Rows)
                {
                    if (ID != "" && ID == row["ID"].ToString())
                    {

                        foreach (DataRow r in sanPham.Rows)
                        {
                            if (ID != "" && ID == r["ID"].ToString())
                            {
                                r["Quantity"] = Convert.ToInt32(row["QTT"].ToString()) + Convert.ToInt32(r["Quantity"].ToString());
                            }
                        }

                        dlRow = row;
                        dataGridView1.DataSource = sanPham;
                        check = true;
                        ID = "";
                    }
                }
                if (check == false)
                {
                    MessageBox.Show("Xóa sản phẩm không thành công");
                } else
                {
                    if (dlRow != null)
                        selectedSP.Rows.Remove(dlRow);
                }
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ID = dataGridView2.SelectedCells[0].OwningRow.Cells[0].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1WasClicked = true;
            if (dataGridView2.Rows.Count == 0)
            {
                MessageBox.Show("Chưa có sản phẩm nào trong hóa đơn");
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có đồng ý xuất hóa đơn", "Thông báo", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    XuatHoaDon xhd = new XuatHoaDon(dataGridView2, selectedSP);
                    xhd.ShowDialog();
                    selectedSP.Rows.Clear();
                    string jsonStr = JsonConvert.SerializeObject(sanPham);
                    System.IO.File.WriteAllText("SanPhams.json", jsonStr);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormDangNhap formDangNhap = new FormDangNhap();
            formDangNhap.Closed += (s, args) => this.Close();
            formDangNhap.Show();
        }
    }
}
