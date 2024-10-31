using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT05_DataGridView04_SQL
{
    public partial class Form1 : Form
    {
        string strcon = @"Server=May04; Database=QLSV; Integrated Security = True";
        DataSet ds = new DataSet();
        SqlDataAdapter adpMonHoc, adpKetqua;
        SqlCommandBuilder cmbMonHoc;
        BindingSource bs = new BindingSource();
        int stt = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Khoi_Tao_Doi_Tuong();
            Doc_Du_Lieu();
            Moc_Noi_Quan_He();
            bs.DataSource = ds;
            bs.DataMember = "MONHOC";
            dgvMonHoc.DataSource = bs;
            bdnmonhoc.BindingSource= bs;
            Lien_Ket_Dieu_Khien();
        }

        private void Lien_Ket_Dieu_Khien()
        {
            txtMaMH.DataBindings.Add("text", bs, "MaMH", true);
            txtTenMH.DataBindings.Add("text", bs, "TenMH", true);
            txtsotiet.DataBindings.Add("text", bs, "Sotiet", true);
        }

        private void Moc_Noi_Quan_He()
        {
            ds.Relations.Add("FK_MH_KQ", ds.Tables["MONHOC"].Columns["MaMH"], ds.Tables["KETQUA"].Columns["MaMH"]);
            ds.Relations["FK_MH_KQ"].ChildKeyConstraint.DeleteRule = Rule.None;

        }

        private void Doc_Du_Lieu()
        {
            adpMonHoc.FillSchema(ds, SchemaType.Source, "MONHOC");
            adpMonHoc.Fill(ds, "MONHOC");

            adpKetqua.FillSchema(ds, SchemaType.Source, "KETQUA");
            adpKetqua.Fill(ds, "KETQUA");
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            txtMaMH.ReadOnly = false;
            stt = bs.Position;
            bs.AddNew();
            txtMaMH.Focus();
        }

        private void btnkhong_Click(object sender, EventArgs e)
        {
            bs.CancelEdit();
            bs.Position = stt; 
        }

        private void btnghi_Click(object sender, EventArgs e)
        {
            //Ghi vào DataSet/Datatable
            bs.EndEdit();
            //Ghi vào CSDL trên SQL server
            int n = adpMonHoc.Update(ds, "MONHOC");
            if (n > 0)
                MessageBox.Show("Cập nhật Thêm/sửa thành công!", "Thông báo cập nhật thành công",MessageBoxButtons.OK,MessageBoxIcon.Information);
            else
                MessageBox.Show("Không thể  Cập nhật Thêm/sửa thành công!", "Thông báo cập nhật Không thành công",MessageBoxButtons.RetryCancel,MessageBoxIcon.Warning);
            txtMaMH.ReadOnly=false;
        }
        
        private void btnhuy_Click(object sender, EventArgs e)
        {
            //Lấy ra dòng cần hủy
            DataRow rHuy=(bs.Current as DataRowView).Row;
            if (rHuy.GetChildRows("FK_MH_KQ").Length > 0)
            {
                MessageBox.Show("Môn học này đã có SV thi,Không hủy được ", "Thông báo hủy môn học");
                return;
            }
            DialogResult tl;
            tl = MessageBox.Show("Có muốn hủy môn học này không ?", "Hỏi đáp môn học", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (tl == DialogResult.Yes) { 
                rHuy.Delete();
                int n = adpMonHoc.Update(ds, "MONHOC");
                if (n > 0)
                    MessageBox.Show("Hủy môn học thành công ", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Khoi_Tao_Doi_Tuong()
        {
            adpMonHoc = new SqlDataAdapter("Select* from MonHoc", strcon);
            adpKetqua = new SqlDataAdapter("Select* from Ketqua ", strcon);
            cmbMonHoc = new SqlCommandBuilder(adpMonHoc);

        }
    }
}
