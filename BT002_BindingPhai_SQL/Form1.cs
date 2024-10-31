using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT002_BindingPhai_SQL
{
    public partial class Form1 : Form
    {
        string strcon = @"Server=May04; Database=QLSV; Integrated Security = True";
        DataSet ds = new DataSet();
        SqlDataAdapter adpSinhVien, adpKhoa, adpKetqua;
        SqlCommandBuilder cmbSinhVien;
        BindingSource bs = new BindingSource();
        int stt = 0;

        public Form1()
        {
            InitializeComponent();
            bs.CurrentChanged += Bs_CurrentChanged;
            bdnSinhVien.BindingSource = bs;
        }

        private void Bs_CurrentChanged(object sender, EventArgs e)
        {
            lblSTT.Text = (bs.Position + 1) + "/" + bs.Count;
            txtTongDiem.Text = Tong_Diem(txtMaSV.Text).ToString();
        }

        private object Tong_Diem(string MSV)
        {
            double kq = 0;
            object td = ds.Tables["KETQUA"].Compute("sum(Diem)", "MaSV='" + MSV + "'");
            if (td == DBNull.Value)
                kq = 0;
            else
                kq = Convert.ToDouble(td);
            return kq;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Khoi_Tao_Doi_Tuong();
            Doc_Du_Lieu();
            Moc_Noi_Quan_He();
            Khoi_Tao_BindingSource();
            Khoi_Tao_ComboBox();
            Lien_Ket_Dieu_Khien();
            txtTongDiem.Text = Tong_Diem(txtMaSV.Text).ToString();
        }

        private void Lien_Ket_Dieu_Khien()
        {
            foreach (Control ctl in this.Controls)
                if (ctl is TextBox && ctl.Name != "txtPhai" && ctl.Name != "txtTongDiem")
                    ctl.DataBindings.Add("text", bs, ctl.Name.Substring(3), true);
                else if (ctl is ComboBox)
                    ctl.DataBindings.Add("SelectedValue", bs, ctl.Name.Substring(3), true);
                else if (ctl is DateTimePicker)
                    ctl.DataBindings.Add("Value", bs, ctl.Name.Substring(3), true);
            Binding bdPhai = new Binding("text", bs, "Phai", true);
            bdPhai.Parse += BdPhai_Parse;
            bdPhai.Format += BdPhai_Format;
            txtPhai.DataBindings.Add(bdPhai);
        }

        private void BdPhai_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value == DBNull.Value || e.Value == null) return;
            e.Value = (Boolean)e.Value ? "NAM" : "NỮ";
        }

        private void BdPhai_Parse(object sender, ConvertEventArgs e)
        {
            if (e.Value == null) return;
            e.Value = e.Value.ToString().ToUpper() == "NAM" ? true : false;

        }

        private void Khoi_Tao_ComboBox()
        {
            cboMaKH.DisplayMember = "TenKH";
            cboMaKH.ValueMember = "MaKH";
            cboMaKH.DataSource = ds.Tables["KHOA"];
        }

        private void Khoi_Tao_BindingSource()
        {
            bs.DataSource = ds;
            bs.DataMember = "SINHVIEN";
        }

        private void Moc_Noi_Quan_He()
        {
            ds.Relations.Add("FK_KH_SV", ds.Tables["KHOA"].Columns["MaKH"], ds.Tables["SINHVIEN"].Columns["MaKH"]);
            ds.Relations.Add("FK_SV_KQ", ds.Tables["SINHVIEN"].Columns["MaSV"], ds.Tables["KETQUA"].Columns["MaSV"]);

            ds.Relations["FK_KH_SV"].ChildKeyConstraint.DeleteRule = Rule.None;
            ds.Relations["FK_SV_KQ"].ChildKeyConstraint.DeleteRule = Rule.None;
        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            bs.MovePrevious();
        }

        private void btnSau_Click(object sender, EventArgs e)
        {
            bs.MoveNext();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaSV.ReadOnly = false;
            stt = bs.Position;
            bs.AddNew();
            cboMaKH.SelectedIndex = 0;
            txtMaSV.Focus();
        }

        private void btnKhong_Click(object sender, EventArgs e)
        {
            bs.CancelEdit();
            txtMaSV.ReadOnly = true;
            bs.Position = stt;

        }

        private void lblSTT_Click(object sender, EventArgs e)
        {

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DataRow rsv = (bs.Current as DataRowView).Row;
            DataRow[] Mang_Dong_lien_Quan = rsv.GetChildRows("FK_SV_KQ");
            if (Mang_Dong_lien_Quan.Length > 0)
            {
                MessageBox.Show("khong xoa duoc sinh vien nay, vi da thi", "Xoa sinh vien", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else
            {
                DialogResult tl;
                tl = MessageBox.Show("Ban co muon xoa sinh vien nay khong(y/n)?", "Can than", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (tl == DialogResult.Yes)
                {
                    bs.RemoveCurrent();
                    int n = adpSinhVien.Update(ds, "SINHVIEN");
                    if (n > 0)
                        MessageBox.Show("Da xoa sinh vien thanh cong");
                }
            }

        }

        private void btnGhi_Click(object sender, EventArgs e)
        {
            if (txtMaSV.ReadOnly == false)
            {
                DataRow rsv = ds.Tables["SINHVIEN"].Rows.Find(txtMaSV.Text);
                if (rsv != null)
                {
                    MessageBox.Show("Ma sv bi trung, nhap lai MaSV khac", "thong bao loi");
                    txtMaSV.Focus();
                    return;
                }
            }
            txtMaSV.ReadOnly = true;
            bs.EndEdit();
            int n = adpSinhVien.Update(ds, "SINHVIEN");
            if (n > 0)
                MessageBox.Show("Cap nhat ghi sinh vien thanh cong");

        }

        private void Doc_Du_Lieu()
        {
            adpKhoa.FillSchema(ds, SchemaType.Source, "KHOA");
            adpKhoa.Fill(ds, "KHOA");

            adpSinhVien.FillSchema(ds, SchemaType.Source, "SINHVIEN");
            adpSinhVien.Fill(ds, "SINHVIEN");

            adpKetqua.FillSchema(ds, SchemaType.Source, "KETQUA");
            adpKetqua.Fill(ds, "KETQUA");
        }

        private void Khoi_Tao_Doi_Tuong()
        {
            adpKhoa = new SqlDataAdapter("Select* from khoa ", strcon);
            adpSinhVien = new SqlDataAdapter("Select* from SinhVien ", strcon);
            adpKetqua = new SqlDataAdapter("Select* from Ketqua ", strcon);
            cmbSinhVien = new SqlCommandBuilder(adpSinhVien);
        }
    }
}
