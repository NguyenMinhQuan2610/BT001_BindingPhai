using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT06_MonHoc_DataSet_CoDinhKieu
{
    public partial class Form1 : Form
    {
        //Khai báo dataset có định kiểu
        dsQLSV ds= new dsQLSV();
        //Khai báo các đối tượng
        dsQLSVTableAdapters.MONHOCTableAdapter adpMonHoc=new dsQLSVTableAdapters.MONHOCTableAdapter();
        dsQLSVTableAdapters.KETQUATableAdapter adpKetQua = new dsQLSVTableAdapters.KETQUATableAdapter();
        BindingSource bs=new BindingSource();
        int stt = -1;
        public Form1()
        {
            InitializeComponent();
            bs.CurrentChanged += Bs_CurrentChanged;
        }

        private void Bs_CurrentChanged(object sender, EventArgs e)
        {
            lblSTT.Text = (bs.Position + 1) + "/" + bs.Count;
            txtTssv.Text = Tong_SV(txtMaMH.Text).ToString();
            txtdiemmax.Text = TinhDiemMx(txtMaMH.Text).ToString();
            if (bs.Position == 0)
            {
                btnDau.Enabled = false;
                btnTruoc.Enabled = false;
                btnSau.Enabled = true;
                btnCuoi.Enabled = true;
            }
            else if (bs.Position == bs.Count - 1)
            {
                btnDau.Enabled = true;
                btnTruoc.Enabled = true;
                btnSau.Enabled = false;
                btnCuoi.Enabled = false;
            }
            else
            {
                btnDau.Enabled = true;
                btnTruoc.Enabled = true;
                btnSau.Enabled = true;
                btnCuoi.Enabled = true;
            }

        }

        private object TinhDiemMx(string MMH)
        {
            double kq = 0;
            object td = ds.Tables["KETQUA"].Compute("max(diem)", "MaMH='" + MMH + "'");
            if (td == DBNull.Value)
                kq = 0;
            else
                kq = Convert.ToDouble(td);
            return kq;

        }

        private object Tong_SV(string MMH)
        {
            double kq = 0;
            object td = ds.Tables["KETQUA"].Compute("count(masv)", "MaMH='" + MMH + "'");
            if (td == DBNull.Value)
                kq = 0;
            else
                kq = Convert.ToDouble(td);
            return kq;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Doc_Du_Lieu();
            Khoi_Tao_BindingSource();
            Lien_Ket_Dieu_Khien();
            bs.MoveFirst();
        }

        private void Lien_Ket_Dieu_Khien()
        {

            foreach (Control ctl in this.Controls)
                if (ctl is TextBox && ctl.Name != "txtdiemmax" && ctl.Name != "txtTssv" && ctl.Name != "txtLoaimon")
                    ctl.DataBindings.Add("text", bs, ctl.Name.Substring(3), true);
            Binding bdLoaiMh = new Binding("text", bs, "LoaiMH", true);
            bdLoaiMh.Parse += BdLoaiMh_Parse;
            bdLoaiMh.Format += BdLoaiMh_Format;
            txtLoaimon.DataBindings.Add(bdLoaiMh);

        }

        private void BdLoaiMh_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value == DBNull.Value || e.Value == null) return;
            e.Value = (Boolean)e.Value ? "Bắt Buộc" : "Tùy Chọn";
        }

        private void BdLoaiMh_Parse(object sender, ConvertEventArgs e)
        {
            if (e.Value == null) return;
            e.Value = e.Value.ToString().ToUpper() == "Bắt Buộc" ? true : false;
        }

        private void Khoi_Tao_BindingSource()
        {
            bs.DataSource = ds;
            bs.DataMember = ds.MONHOC.TableName;
            bdnMonHoc.BindingSource = bs;
        }

        private void Doc_Du_Lieu()
        {
            adpMonHoc.Fill(ds.MONHOC);
            adpKetQua.Fill(ds.KETQUA);
        }

        private void btnDau_Click(object sender, EventArgs e)
        {
            bs.MoveFirst();
         
        }

        private void btnCuoi_Click(object sender, EventArgs e)
        {
 
            bs.MoveLast();
        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            bs.MovePrevious();
        }

        private void btnSau_Click(object sender, EventArgs e)
        {
            bs.MoveNext();
        }

        private void btnkhong_Click(object sender, EventArgs e)
        {
            bs.CancelEdit();
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            DialogResult tl;
            tl = MessageBox.Show("Ban co muon thoat  khong(y/n)?", "Thong bao", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (tl == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            txtMaMH.ReadOnly= false;
            bs.Position = stt;
            bs.AddNew( );
            txtMaMH.Focus();
        }

        private void btnghi_Click(object sender, EventArgs e)
        {
            if (txtMaMH.ReadOnly == false)
            {
                dsQLSV.MONHOCRow rmh=ds.MONHOC.FindByMaMH(txtMaMH.Text);
                if (rmh != null)
                {
                    MessageBox.Show("Mã môn vừa nhập: " + txtMaMH.Text + " đã có. Mời nhập lại","Bị trùng mã môn học",MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtMaMH.Clear();
                    txtMaMH.Focus();
                    return;
                }
            }
            txtMaMH.ReadOnly = true;
            bs.EndEdit();
            int n = adpMonHoc.Update(ds.MONHOC);
            if (n > 0)
            {
               // MessageBox.Show("Cập nhật môn học thành công !","Thông báo cập nhật",MessageBoxButtons.OK,MessageBoxIcon.Information);
                MessageBox.Show("Cập nhật môn học: " + "\r\n" +
                    " + Mã môn: " + txtMaMH.Text + "\r\n" +
                    " + Tên Môn: " + txtTenMH.Text + "\r\n" +
                    " + Số tiết: " + txtSotiet.Text + "\r\n" +
                    "Thành công !", "Thông báo đã cập nhật thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            dsQLSV.MONHOCRow rmh=(bs.Current as DataRowView).Row as dsQLSV.MONHOCRow;
            if (rmh.GetKETQUARows().Length > 0) { 
                MessageBox.Show("Môn học: "+ txtMaMH.Text + " - " + txtTenMH.Text.ToUpper() + "\r\n" +
                    "Đã có sinh viên dự thi. Không hủy được","Thông báo hủy môn",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
            DialogResult tl;
            tl=MessageBox.Show("Bạn có muốn hủy môn học: " + "\r\n" +
                    " + Mã môn: " + txtMaMH.Text + "\r\n" +
                    " + Tên Môn: " + txtTenMH.Text + "\r\n" +
                    " + Số tiết: " + txtSotiet.Text + "\r\n" +
                    "này không ?", "Hủy môn học", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (tl == DialogResult.Yes) {
                bs.RemoveCurrent();
                int n = adpMonHoc.Update(ds.MONHOC);
                if (n > 0) {
                    MessageBox.Show("Xóa môn học: " + "\r\n" +
                    " + Mã môn: " + txtMaMH.Text + "\r\n" +
                    " + Tên Môn: " + txtTenMH.Text + "\r\n" +
                    " + Số tiết: " + txtSotiet.Text + "\r\n" +
                    "Thành công !", "Thông báo đã Xóa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
