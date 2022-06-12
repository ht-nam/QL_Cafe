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
        private DataTable thanhVien = new DataTable();
        //private DataTable hoaDon
        private string ID = "";
        public NhanVien()
        {
            InitializeComponent();
            CreateSPtable();
            CreateTVtable();
            dataGridView1.DataSource = sanPham;
            dataGridView1.Columns[0].Visible = false;
            dataGridView2.DataSource = selectedSP;
            dataGridView3.DataSource = thanhVien;

            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns[0].Visible = false;
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

        public void CreateTVtable()
        {

            if (System.IO.File.Exists("ThanhViens.json"))
            {
                System.IO.StreamReader reader = new System.IO.StreamReader("ThanhViens.json");
                string jsonStr = reader.ReadToEnd();
                reader.Close();

                if (jsonStr != "")
                {
                    thanhVien = JsonConvert.DeserializeObject<DataTable>(jsonStr);
                }
            }
            if (thanhVien.Columns.Count == 0)
            {
                thanhVien.Columns.Add("IDcard", typeof(string));
                thanhVien.Columns.Add("Name", typeof(string));
                thanhVien.Columns.Add("ID", typeof(string));
                thanhVien.Columns.Add("Phone", typeof(string));
                thanhVien.Columns.Add("BirthDay", typeof(string));
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
                bool idCheck = false;

                foreach(DataRow row in thanhVien.Rows)
                {
                    if (row["IDcard"].ToString() == textBoxID.Text && textBoxID.Text != "")
                    {
                        idCheck = true;
                        break;
                    }
                }

                if (idCheck == true || textBoxID.Text == "")
                {
                    DialogResult dialogResult = MessageBox.Show("Bạn có đồng ý xuất hóa đơn", "Thông báo", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        XuatHoaDon xhd = new XuatHoaDon(dataGridView2, selectedSP, textBoxID.Text);
                        xhd.ShowDialog();
                        selectedSP.Rows.Clear();
                        string jsonStr = JsonConvert.SerializeObject(sanPham);
                        System.IO.File.WriteAllText("SanPhams.json", jsonStr);
                    }
                } else if(idCheck == false)
                {
                    MessageBox.Show("Mã thẻ thành viên không hợp lệ");
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

        private void emptyText()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            dateTimePicker1.Value = new DateTime(2000,1,1);
        }

        private void saveTVJson()
        {
            string jsonStr = JsonConvert.SerializeObject(thanhVien);
            System.IO.File.WriteAllText("ThanhViens.json", jsonStr);
        }


        //Them the thanh vien
        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Hãy nhập thông tin hợp lệ");
            }
            else
            {
                bool check = false;
                foreach (DataRow dataRow in thanhVien.Rows)
                {
                    if (dataRow["IDcard"].ToString() == textBox4.Text)
                    {
                        MessageBox.Show("Mã thẻ đã bị trùng");
                        check = true;
                    }
                }
                if (check == false)
                {
                    try
                    {
                        thanhVien.Rows.Add(textBox4.Text, textBox1.Text, textBox3.Text, textBox2.Text, dateTimePicker1.Value.ToString("dd/MM/yyyy"));
                        MessageBox.Show("Thêm thông tin thẻ thành công");
                        emptyText();
                        saveTVJson();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Thêm thông tin thẻ không thành công", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        //Sua the thanh vien
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Hãy nhập thông tin hợp lệ");
            }
            else
            {
                bool check = false;
                foreach (DataRow dataRow in thanhVien.Rows)
                {
                    if (dataRow["IDcard"].ToString() == textBox4.Text)
                    {
                        try
                        {
                            DialogResult dialogResult = MessageBox.Show("Bạn có đồng ý sửa thẻ này", "Thông báo", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                dataRow["Name"] = textBox1.Text;
                                dataRow["ID"] = textBox3.Text;
                                dataRow["Phone"] = textBox2.Text;
                                dataRow["BirthDay"] = dateTimePicker1.Value.ToString("dd/MM/yyyy");
                                MessageBox.Show("Sửa thông tin thẻ thành công");
                                check = true;
                                saveTVJson();
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Sửa thẻ không thành công", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                if (check == false)
                {
                    MessageBox.Show("Không tìm thẻ id mà bạn nhập");
                }
            }
        }


        //Xoa the thanh vien
        private void button3_Click(object sender, EventArgs e)
        {
            bool check = false;
            for (int i = 0; i < thanhVien.Rows.Count; i++)
            {
                if (thanhVien.Rows[i]["IDcard"].ToString() == textBox4.Text)
                {
                    try
                    {
                        check = true;
                        DialogResult dialogResult = MessageBox.Show("Bạn có đồng ý xóa thẻ này", "Thông báo", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            thanhVien.Rows.RemoveAt(i);
                            MessageBox.Show("Xóa thẻ thành công");
                            saveTVJson();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Xóa thẻ không thành công", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (check == false)
            {
                MessageBox.Show("Không tìm thấy thẻ với id mà bạn nhập");
            }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox4.Text = dataGridView3.SelectedCells[0].OwningRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView3.SelectedCells[0].OwningRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView3.SelectedCells[0].OwningRow.Cells[2].Value.ToString();
            textBox2.Text = dataGridView3.SelectedCells[0].OwningRow.Cells[3].Value.ToString();

            string[] dL = dataGridView3.SelectedCells[0].OwningRow.Cells[4].Value.ToString().Split('/');
            dateTimePicker1.Value = new DateTime(Convert.ToInt32(dL[2]), Convert.ToInt32(dL[1]), Convert.ToInt32(dL[0]));
            
            
        }
    }
}
