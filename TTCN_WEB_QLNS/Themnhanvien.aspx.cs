using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class Themnhanvien : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadChucVu();
                LoadTrinhDo();
                LoadBoPhan();

            }
        }

        void LoadChucVu()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT IDCV, TenCV FROM Chuc_vu", conn);
                conn.Open();
                ddlChucVu.DataSource = cmd.ExecuteReader();
                ddlChucVu.DataTextField = "TenCV";
                ddlChucVu.DataValueField = "IDCV";
                ddlChucVu.DataBind();
            }
        }

        void LoadTrinhDo()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT IDTD, TenTD FROM Trinh_do", conn);
                conn.Open();
                ddlTrinhDo.DataSource = cmd.ExecuteReader();
                ddlTrinhDo.DataTextField = "TenTD";
                ddlTrinhDo.DataValueField = "IDTD";
                ddlTrinhDo.DataBind();
            }
        }

        void LoadBoPhan()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT IDBP, TenBP FROM Bo_phan", conn);
                conn.Open();
                ddlBoPhan.DataSource = cmd.ExecuteReader();
                ddlBoPhan.DataTextField = "TenBP";
                ddlBoPhan.DataValueField = "IDBP";
                ddlBoPhan.DataBind();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // ============================
            // VALIDATE RỖNG
            // ============================
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtNgaySinh.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtCCCD.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "err",
                    "alert('❌ Vui lòng nhập đầy đủ thông tin nhân viên!');", true);
                return;
            }

            string hoten = txtHoTen.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string cccd = txtCCCD.Text.Trim();
            string diachi = txtDiaChi.Text.Trim();
            string imgPath = "";

            // ============================
            // VALIDATE SĐT
            // ============================
            if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, @"^\d{10}$"))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "sdt",
                    "alert('❌ Số điện thoại phải gồm đúng 10 chữ số!');", true);
                return;
            }

            // ============================
            // VALIDATE CCCD
            // ============================
            if (!System.Text.RegularExpressions.Regex.IsMatch(cccd, @"^\d{12}$"))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "cccd",
                    "alert('❌ CCCD phải gồm đúng 12 chữ số!');", true);
                return;
            }

            // ============================
            // VALIDATE TUỔI
            // ============================
            DateTime ngaySinh;
            if (!DateTime.TryParse(txtNgaySinh.Text, out ngaySinh))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ngay",
                    "alert('❌ Ngày sinh không hợp lệ!');", true);
                return;
            }

            int tuoi = DateTime.Now.Year - ngaySinh.Year;
            if (ngaySinh > DateTime.Now.AddYears(-tuoi)) tuoi--;

            if (tuoi < 18)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "tuoi",
                    "alert('❌ Nhân viên phải đủ 18 tuổi!');", true);
                return;
            }

            // ============================
            // UPLOAD ẢNH
            // ============================
            string folderPath = Server.MapPath("~/Images/");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            if (fileAvatar.HasFile)
            {
                string filename = Path.GetFileName(fileAvatar.FileName);
                imgPath = "~/Images/" + filename;
                fileAvatar.SaveAs(Path.Combine(folderPath, filename));
            }

            // ============================
            // INSERT DATABASE
            // ============================
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sqlNV = @"
INSERT INTO Nhan_vien
(HoTen, GioiTinh, NgaySinh, SDT, CCCD, DiaChi, HinhAnh,
 IDCV, IDTD, IDBP, TrangThai)
VALUES
(@ten, @gt, @ngay, @sdt, @cccd, @diachi, @img,
 @cv, @td, @bp, 1);
SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(sqlNV, conn);
                cmd.Parameters.AddWithValue("@ten", hoten);
                cmd.Parameters.AddWithValue("@gt", rblGioiTinh.SelectedValue);
                cmd.Parameters.AddWithValue("@ngay", ngaySinh);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.Parameters.AddWithValue("@cccd", cccd);
                cmd.Parameters.AddWithValue("@diachi", diachi);
                cmd.Parameters.AddWithValue("@img", imgPath);
                cmd.Parameters.AddWithValue("@cv", ddlChucVu.SelectedValue);
                cmd.Parameters.AddWithValue("@td", ddlTrinhDo.SelectedValue);
                cmd.Parameters.AddWithValue("@bp", ddlBoPhan.SelectedValue);

                int maNV = Convert.ToInt32(cmd.ExecuteScalar());

                // ============================
                // TẠO USER
                // ============================
                string sqlUser = @"INSERT INTO [User]
(Username, Password, MaNV, IDROLE, IsActive)
VALUES (@u, @p, @manv, 10, 1)";

                SqlCommand cmdUser = new SqlCommand(sqlUser, conn);
                cmdUser.Parameters.AddWithValue("@u", sdt);
                cmdUser.Parameters.AddWithValue("@p", "123");
                cmdUser.Parameters.AddWithValue("@manv", maNV);

                cmdUser.ExecuteNonQuery();
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "ok",
                "alert('✔ Thêm nhân viên thành công!'); window.location='QuanLyUser.aspx';", true);
        }

        protected void txtHoTen_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtNgaySinh_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtSDT_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtCCCD_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtDiaChi_TextChanged(object sender, EventArgs e)
        {

        }

        protected void rblGioiTinh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlTrinhDo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlBoPhan_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}