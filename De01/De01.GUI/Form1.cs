using De01.BUS;
using De01.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De01.GUI
{
    public partial class frmStudent : Form
    {
        public frmStudent()
        {
            InitializeComponent();
        }

        private readonly SinhVienService sinhVienService = new SinhVienService();
        private readonly LopService lopService = new LopService();

        private void frmStudent_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvStudent);
                var listSinhVien = sinhVienService.GetAll();
                var listLop = lopService.GetAll();
                FillClassCombobox(listLop);
                BindGrid(listSinhVien);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillClassCombobox(List<Lop> listLop)
        {
            listLop.Insert(0, new Lop());
            this.cmbLop.DataSource = listLop;
            this.cmbLop.DisplayMember = "TenLop";
            this.cmbLop.ValueMember = "MaLop";
        }

        private void BindGrid(List<Sinhvien> listSinhvien)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listSinhvien)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.MaSV;
                dgvStudent.Rows[index].Cells[1].Value = item.HotenSV;
                dgvStudent.Rows[index].Cells[2].Value = item.Ngaysinh;
                if (item.Lop != null)
                {
                    dgvStudent.Rows[index].Cells[3].Value = item.Lop.TenLop;
                }
            }
        }

        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn muốn thoát chương trình ?", "Thoát",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(rs == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvStudent.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvStudent.Rows)
                {
                    if (row.Selected)
                    {
                        txtMSSV.Text = dgvStudent.Rows[row.Index].Cells[0].Value.ToString();
                        txtHoTen.Text = dgvStudent.Rows[row.Index].Cells[1].Value.ToString();
                        dtNgaySinh.Text = dgvStudent.Rows[row.Index].Cells[2].Value.ToString();
                        cmbLop.Text = dgvStudent.Rows[row.Index].Cells[3].Value.ToString();
                    }
                }
            }
        }

        private void LoadList()
        {
            StudentModel context = new StudentModel();
            List<Sinhvien> listSinhvien = context.Sinhviens.ToList();
            BindGrid(listSinhvien);
        }

        private void Refresh()
        {
            txtMSSV.Clear();
            txtHoTen.Clear();
            txtMSSV.Focus();
            cmbLop.SelectedValue = 1;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMSSV.Text) || string.IsNullOrEmpty(txtHoTen.Text))
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin !");
                }
                if (txtMSSV.Text.Length != 6)
                {
                    throw new Exception("Mã sinh viên bao gồm 6 số !");
                }
                foreach (DataGridViewRow i in dgvStudent.Rows)
                {
                    if (txtMSSV.Text == dgvStudent.Rows[i.Index].Cells[0].Value.ToString())
                    {
                        throw new Exception("Mã số sinh viên đã tồn tại trong danh sách !");
                    }
                }
                var item = new Sinhvien()
                {
                    MaSV = txtMSSV.Text,
                    HotenSV = txtHoTen.Text,
                    Ngaysinh = DateTime.Parse(dtNgaySinh.Text),
                    MaLop = cmbLop.SelectedValue.ToString()
                };
                sinhVienService.InsertNew(item);
                LoadList();
                MessageBox.Show("Thêm mới sinh viên thành công !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            StudentModel context = new StudentModel();
            Sinhvien deleteStudent = context.Sinhviens.FirstOrDefault(p => p.MaSV == txtMSSV.Text);
            try
            {
                if (deleteStudent != null)
                {
                    DialogResult rs = MessageBox.Show($"Bạn có chắc muốn xóa sinh viên {deleteStudent.HotenSV} này không ?",
                        "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (rs == DialogResult.Yes)
                    {
                        dgvStudent.Rows.RemoveAt(dgvStudent.CurrentRow.Index);
                        context.Sinhviens.Remove(deleteStudent);
                        context.SaveChanges();
                        LoadList();
                        MessageBox.Show($"Xóa sinh viên {deleteStudent.HotenSV} thành công !", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Refresh();
                    }
                }
                else
                    throw new Exception("Không tồn tại mã số sinh viên này trong danh sách !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            StudentModel context = new StudentModel();
            Sinhvien updateStudentList = sinhVienService.FindById(txtMSSV.Text);
            try
            {
                if (updateStudentList != null)
                {
                    updateStudentList.HotenSV = txtHoTen.Text;
                    updateStudentList.Ngaysinh = DateTime.Parse(dtNgaySinh.Text);
                    sinhVienService.InsertUpdate(updateStudentList);
                    LoadList();
                    MessageBox.Show($"Cập nhật thông tin sinh viên {updateStudentList.HotenSV} thành công !",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Refresh();
                }
                else
                    throw new Exception("Mã số sinh viên chưa có trong danh sách ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMSSV.Focus();
            }
        }

        private string RemoveDiacritics(string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string findName = txtFind.Text;
            findName = RemoveDiacritics(findName);
            for (int i = 0; i < dgvStudent.Rows.Count; i++)
            {
                string name = dgvStudent.Rows[i].Cells[1].Value.ToString();

                name = RemoveDiacritics(name);

                bool contains = name.IndexOf(findName, StringComparison.OrdinalIgnoreCase) >= 0;
                if (contains)
                {
                    dgvStudent.Rows[i].Visible = true;
                }
                else
                {
                    dgvStudent.Rows[i].Visible = false;
                }
            }
        }
    }
}
