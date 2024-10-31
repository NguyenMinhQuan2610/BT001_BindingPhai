using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT04_DataGridView3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Them_Du_Lieu_Vao_Luoi();
            DataGridViewRow r = dgvMonHoc.Rows[0];
            r.Selected = true;
            Gan_Du_Lieu(r);
        }

        private void Gan_Du_Lieu(DataGridViewRow r)
        {
            txtMaMH.Text = r.Cells[0].Value.ToString();
            txtTenMH.Text = r.Cells[1].Value.ToString();
            txtsotiet.Text = r.Cells[2].Value.ToString();
        }

        private void Them_Du_Lieu_Vao_Luoi()
        {
            dgvMonHoc.Rows.Add("01", "Cơ sở dữ liệu", 90);
            dgvMonHoc.Rows.Add("02", "Tin học", 75);
            dgvMonHoc.Rows.Add("03", "Lập trình Window", 90);
            dgvMonHoc.Rows.Add("04", "Quản trị CSDL với SQL", 90);
            dgvMonHoc.Rows.Add("05", "Lập trình cơ sở dữ liệu cơ bản", 75);
            dgvMonHoc.Rows.Add("06", "Phân tích thiết kế hệ thống", 105);
        }

        private void dgvMonHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Sự kiện này xảy ra khi có một ô trên dòng nào đó được click
            //Để lấy thông tin của dòng chứa ô được click => e.RowIndex
            DataGridViewRow r=dgvMonHoc.Rows[e.RowIndex];
            Gan_Du_Lieu(r);
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            txtMaMH.ReadOnly = false;
            foreach (Control ctl in this.Controls) 
                if(ctl is TextBox) { 
                    (ctl as TextBox).Clear();
             }
                txtMaMH.Focus();
        }

        private void btnkhong_Click(object sender, EventArgs e)
        {
            Gan_Du_Lieu(dgvMonHoc.SelectedRows[0]);
            txtMaMH.ReadOnly=true;
        }

        private void btnghi_Click(object sender, EventArgs e)
        {
            if (txtMaMH.ReadOnly == true)
            {
                //Lấy thông tin dòng cần sửa => Dòng đang được chọn trên lưới
                DataGridViewRow sua = dgvMonHoc.SelectedRows[0];
                sua.Cells[1].Value = txtTenMH.Text;
                sua.Cells[2].Value = txtsotiet.Text;
            }
            else { 
                int stt=dgvMonHoc.Rows.Add(txtMaMH.Text,txtTenMH.Text,txtsotiet.Text);
                dgvMonHoc.Rows[stt].Selected = true;
                txtMaMH.ReadOnly = true;

            }
        }

        private void btnhuy_Click(object sender, EventArgs e)
        {
            DialogResult tl;
            tl = MessageBox.Show("Bạn có muốn xóa môn có: Mã môn học: " +  txtMaMH.Text + 
                ", Tên môn học: "+ txtTenMH.Text + ", Số tiết: "+txtsotiet.Text +" này không (Y/N) ? ","Hỏi đáp xóa môn",
                MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (tl == DialogResult.Yes) {
                DataGridViewRow rHuy = dgvMonHoc.SelectedRows[0];
                dgvMonHoc.Rows.Remove(rHuy);
                dgvMonHoc.Rows[0].Selected = true;
                Gan_Du_Lieu(dgvMonHoc.Rows[0]);
            }
        }
    }
}
