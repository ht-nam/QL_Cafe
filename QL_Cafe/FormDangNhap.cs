using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_Cafe
{
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
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
            } else if (textBox1.Text == "nv01" && textBox2.Text == "1234")
            {
                Form FormNhanVien = new NhanVien();
                FormNhanVien.Show();
                this.Hide();
                FormNhanVien.Closed += (s, args) => this.Close();
                FormNhanVien.Show();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không hợp lệ","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
    }
}
