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
    public partial class QuanLy : Form
    {
        private DataTable NhanViens = new DataTable();
        private DataTable SanPhams = new DataTable();

        public QuanLy()
        {
            InitializeComponent();
            CreateNVtable();
            CreateSPtable();
            dataGridView1.DataSource = NhanViens;
            dataGridView2.DataSource = SanPhams;
        }

        public void CreateNVtable()
        {
            if (System.IO.File.Exists("NhanViens.json"))
            {
                System.IO.StreamReader reader = new System.IO.StreamReader("NhanViens.json");
                string jsonStr = reader.ReadToEnd();
                reader.Close();

                NhanViens = JsonConvert.DeserializeObject<DataTable>(jsonStr);
            }
            if (NhanViens.Columns.Count == 0){
                NhanViens.Columns.Add("Username", typeof(string));
                NhanViens.Columns.Add("Password", typeof(string));
                NhanViens.Columns.Add("Name", typeof(string));
                NhanViens.Columns.Add("ID", typeof(string));
                NhanViens.Columns.Add("PhoneNumber", typeof(string));
                NhanViens.Columns.Add("YearOfBirth", typeof(int));
            }
        }

        public void CreateSPtable()
        {
            if (System.IO.File.Exists("SanPhams.json"))
            {
                System.IO.StreamReader reader = new System.IO.StreamReader("SanPhams.json");
                string jsonStr = reader.ReadToEnd();
                reader.Close();

                SanPhams = JsonConvert.DeserializeObject<DataTable>(jsonStr);
            }
            if (SanPhams.Columns.Count == 0)
            {
                SanPhams.Columns.Add("ID", typeof(string));
                SanPhams.Columns.Add("Name", typeof(string));
                SanPhams.Columns.Add("Price", typeof(string));
                SanPhams.Columns.Add("Quantity", typeof(int));
            }
        }

        private void saveNVJson()
        {
            string jsonStr = JsonConvert.SerializeObject(NhanViens);
            System.IO.File.WriteAllText("NhanViens.json", jsonStr);
        }

        private void saveSPJson()
        {
            string jsonStr = JsonConvert.SerializeObject(SanPhams);
            System.IO.File.WriteAllText("SanPhams.json", jsonStr);
        }

        //Thêm nhân viên
        private void button1_Click(object sender, EventArgs e)
        {
            bool check = false;
            foreach (DataRow dataRow in NhanViens.Rows)
            {
                if (dataRow["Username"].ToString() == textBox5.Text)
                {
                    MessageBox.Show("Username nhân viên đã bị trùng");
                    check = true;
                }
            }
            if (check == false)
            {
                NhanViens.Rows.Add(textBox5.Text, textBox6.Text, textBox1.Text, textBox4.Text, textBox2.Text,textBox3.Text);
                MessageBox.Show("Thêm thông tin nhân viên thành công");
                saveNVJson();
            }
        }

        //Sửa thông tin nhân viên
        private void button2_Click(object sender, EventArgs e)
        {
            bool check = false;
            foreach (DataRow dataRow in NhanViens.Rows)
            {
                if (dataRow["Username"].ToString() == textBox5.Text)
                {
                    dataRow["Password"] =  textBox6.Text;
                    dataRow["Name"] = textBox1.Text;
                    dataRow["ID"] = textBox4.Text;
                    dataRow["PhoneNumber"] =  textBox2.Text;
                    dataRow["YearOfBirth"] = textBox3.Text;
                    MessageBox.Show("Sửa thông tin nhân viên thành công");
                    check = true;
                    saveNVJson();
                }
            }
            if (check == false)
            {
                MessageBox.Show("Không tìm thấy nhân viên với Username mà bạn nhập");
            }
        }

        //Xóa thông tin nhân viên
        private void button3_Click(object sender, EventArgs e)
        {
            bool check = false;
            for (int i = 0; i < NhanViens.Rows.Count; i++)
            {
                if (NhanViens.Rows[i]["Username"].ToString() == textBox5.Text)
                {
                    check = true;
                    NhanViens.Rows.RemoveAt(i);
                    MessageBox.Show("Xóa thông tin nhân viên thành công");
                    saveNVJson();
                }
            }
            if (check == false)
            {
                MessageBox.Show("Không tìm thấy nhân viên với Username mà bạn nhập");
            }
        }

        //Thêm sản phẩm
        private void button6_Click(object sender, EventArgs e)
        {
            bool check = false;
            foreach (DataRow dataRow in SanPhams.Rows)
            {
                if (dataRow["ID"].ToString() == textBox7.Text)
                {
                    MessageBox.Show("ID sản phẩm đã bị trùng");
                    check = true;
                }
            }
            if (check == false)
            {
                SanPhams.Rows.Add(textBox7.Text, textBox8.Text, textBox9.Text, textBox10.Text);
                MessageBox.Show("Thêm sản phẩm thành công");
                saveSPJson();
            }
        }

        //Sửa sản phẩm
        private void button5_Click(object sender, EventArgs e)
        {
            bool check = false;
            foreach(DataRow dataRow in SanPhams.Rows)
            {
                if (dataRow["ID"].ToString() == textBox7.Text)
                {
                    dataRow["Name"] =  textBox8.Text;
                    dataRow["Price"] = textBox9.Text;
                    dataRow["Quantity"] = textBox10.Text;
                    MessageBox.Show("Sửa thông tin sản phẩm thành công");
                    check = true;
                    saveSPJson();
                }
            }
            if (check == false)
            {
                MessageBox.Show("Không tìm thấy sản phẩm với ID mà bạn nhập");
            }
        }

        //Xóa sản phẩm
        private void button4_Click(object sender, EventArgs e)
        {
            bool check = false;
            for(int i = 0; i < SanPhams.Rows.Count; i++)
            {
                if (SanPhams.Rows[i]["ID"].ToString() == textBox7.Text)
                {
                    check = true;
                    SanPhams.Rows.RemoveAt(i);
                    MessageBox.Show("Xóa thông tin sản phẩm thành công");
                    saveSPJson();
                }
            }
            if (check == false)
            {
                MessageBox.Show("Không tìm thấy sản phẩm với ID mà bạn nhập");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox5.Text = dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value.ToString();
            textBox6.Text = dataGridView1.SelectedCells[0].OwningRow.Cells[1].Value.ToString();
            textBox1.Text = dataGridView1.SelectedCells[0].OwningRow.Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.SelectedCells[0].OwningRow.Cells[3].Value.ToString();
            textBox2.Text = dataGridView1.SelectedCells[0].OwningRow.Cells[4].Value.ToString();
            textBox3.Text = dataGridView1.SelectedCells[0].OwningRow.Cells[5].Value.ToString();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox7.Text = dataGridView2.SelectedCells[0].OwningRow.Cells[0].Value.ToString();
            textBox8.Text = dataGridView2.SelectedCells[0].OwningRow.Cells[1].Value.ToString();
            textBox9.Text = dataGridView2.SelectedCells[0].OwningRow.Cells[2].Value.ToString();
            textBox10.Text = dataGridView2.SelectedCells[0].OwningRow.Cells[3].Value.ToString();
        }

        
    }
}
