using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BT03_DataGridView_1
{
    public partial class form1 : Form
    {
        public form1()
        {
            InitializeComponent();
        }

        private void dgvMonHoc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void form1_Load(object sender, EventArgs e)
        {
            Them_Cot_Vao_Luoi();
            Them_Dong_Du_Lieu_Vao_Luoi();
            Dinh_Dang_Luoi();
        }

        private void Dinh_Dang_Luoi()
        {
            dgvMonHoc.SelectionMode= DataGridViewSelectionMode.FullRowSelect;
            dgvMonHoc.MultiSelect= false;
            dgvMonHoc.AllowUserToAddRows= false;
            dgvMonHoc.Columns[0].Width = 100;
            dgvMonHoc.Columns[1].Width = 250;
            dgvMonHoc.Columns[2].Width = 100;
            dgvMonHoc.Columns[3].Width = 100;

        }

        private void Them_Dong_Du_Lieu_Vao_Luoi()
        {
            dgvMonHoc.Rows.Add("01", "Cơ sở dữ liệu", 90, "Bắt buộc");
            dgvMonHoc.Rows.Add("02", "Tin học", 75, "Tự chọn");
            dgvMonHoc.Rows.Add("03", "Lập trình Window", 90, "Bắt buộc");
            dgvMonHoc.Rows.Add("04", "Quản trị CSDL với SQL", 90, "Tự chọn");
            dgvMonHoc.Rows.Add("05", "Lập trình cơ sở dữ liệu cơ bản", 75, "Bắt buộc");
            dgvMonHoc.Rows.Add("06", "Phân tích thiết kế hệ thống", 105, "Tự chọn");
        }

        private void Them_Cot_Vao_Luoi()
        {
            dgvMonHoc.Columns.Add("colMaMH", "Mã Môn");
            dgvMonHoc.Columns.Add("colTenMH", "Tên Môn");
            dgvMonHoc.Columns.Add("colSoTiet", "Số tiết");
            dgvMonHoc.Columns.Add("colLoaiMH", "Loại Môn");

        }
    }
}
