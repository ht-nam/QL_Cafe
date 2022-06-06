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
    public partial class FormDangNhap : Form
    {
        public static string passingText;
        private DataTable NhanViens = new DataTable();
        private bool haveNV = false;
        public FormDangNhap()
        {
            InitializeComponent();
            CreateNVtable();
        }

        public void CreateNVtable()
        {
            if (System.IO.File.Exists("NhanViens.json"))
            {
                System.IO.StreamReader reader = new System.IO.StreamReader("NhanViens.json");
                string jsonStr = reader.ReadToEnd();
                reader.Close();

                if (jsonStr != "")
                {
                    NhanViens = JsonConvert.DeserializeObject<DataTable>(jsonStr);
                    haveNV = true;
                }
            }
            if (NhanViens.Columns.Count == 0)
            {
                NhanViens.Columns.Add("Username", typeof(string));
                NhanViens.Columns.Add("Password", typeof(string));
                NhanViens.Columns.Add("Name", typeof(string));
                NhanViens.Columns.Add("ID", typeof(string));
                NhanViens.Columns.Add("PhoneNumber", typeof(string));
                NhanViens.Columns.Add("Gender", typeof(string));
                NhanViens.Columns.Add("YearOfBirth", typeof(int));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin" && textBox2.Text == "admin")
            {
                Form FormQuanLy = new QuanLy();
                FormQuanLy.Show();
                this.Hide();
                FormQuanLy.Closed += (s, args) => this.Close();
                FormQuanLy.Show();
            } else if (haveNV == true)
            {
                passingText = textBox1.Text;
                bool check = false;
                foreach (DataRow row in NhanViens.Rows)
                {
                    if (row["Username"].ToString() == textBox1.Text && row["Password"].ToString() == textBox2.Text)
                    {
                        check = true;
                        Form FormNhanVien = new NhanVien();
                        FormNhanVien.Show();
                        this.Hide();
                        FormNhanVien.Closed += (s, args) => this.Close();
                        FormNhanVien.Show();
                    }
                }
                if (check == false)
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không hợp lệ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không hợp lệ","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
    }
}
