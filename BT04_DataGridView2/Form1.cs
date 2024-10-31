using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT04_DataGridView2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvMonHoc.Rows.Add("01", "Cơ sở dữ liệu", 90, "Bắt buộc");
            dgvMonHoc.Rows.Add("02", "Tin học", 75, "Tự chọn");
            dgvMonHoc.Rows.Add("03", "Lập trình Window", 90, "Bắt buộc");
            dgvMonHoc.Rows.Add("04", "Quản trị CSDL với SQL", 90, "Tự chọn");
            dgvMonHoc.Rows.Add("05", "Lập trình cơ sở dữ liệu cơ bản", 75, "Bắt buộc");
            dgvMonHoc.Rows.Add("06", "Phân tích thiết kế hệ thống", 105, "Tự chọn");
        }

        private void dgvsinhvien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
