using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class ThongTinCaNhan : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["MaNV"] == null)
                {
                    Response.Redirect("DangNhap.aspx");
                    return;
                }

                string maNV = Session["MaNV"].ToString();
                lblWelcome.Text = "Xin chào, " + Session["UserName"].ToString();
                LoadThongTinNhanVien(maNV);

                // ⭐ Kiểm tra quyền sửa
                btnEdit.Visible = DuocPhepChinhSua();
            }

        }

        void LoadThongTinNhanVien(string maNV)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
        SELECT 
            nv.MaNV,
            nv.HoTen,
            nv.NgaySinh,
            nv.GioiTinh,
            nv.DiaChi,
            cv.TenCV,
            pb.TenPB,
            bh.SoBH
        FROM Nhan_vien nv
        LEFT JOIN Chuc_vu cv ON nv.IDCV = cv.IDCV
        LEFT JOIN Phong_ban pb ON nv.IDPB = pb.IDPB
        LEFT JOIN Bao_hiem bh ON nv.MaNV = bh.MaNV
        WHERE nv.MaNV = @MaNV";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblMaNV.Text = dr["MaNV"].ToString();
                    lblHoTen.Text = dr["HoTen"].ToString();

                    // 👉 Ngày sinh
                    if (dr["NgaySinh"] != DBNull.Value)
                    {
                        DateTime ns = Convert.ToDateTime(dr["NgaySinh"]);
                        lblNgaySinh.Text = ns.ToString("dd/MM/yyyy");
                    }

                    // 👉 Giới tính
                    if (dr["GioiTinh"] != DBNull.Value)
                    {
                        bool gioiTinh = Convert.ToBoolean(dr["GioiTinh"]);
                        lblGioiTinh.Text = gioiTinh ? "Nam" : "Nữ";
                    }

                    lblDiaChi.Text = dr["DiaChi"].ToString();
                    lblChucVu.Text = dr["TenCV"].ToString();
                    lblPhongBan.Text = dr["TenPB"].ToString();
                    lblSoBH.Text = dr["SoBH"] == DBNull.Value ? "Chưa có" : dr["SoBH"].ToString();
                }
            }
        }

        bool DuocPhepChinhSua()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT ThoiGianMo, ThoiGianDong FROM CauHinhSuaThongTin";
                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    DateTime mo = Convert.ToDateTime(dr["ThoiGianMo"]);
                    DateTime dong = Convert.ToDateTime(dr["ThoiGianDong"]);
                    DateTime now = DateTime.Now;

                    return now >= mo && now <= dong;
                }
            }
            return false;
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            pnlEdit.Visible = true;

            txtEditHoTen.Text = lblHoTen.Text;
            txtEditDiaChi.Text = lblDiaChi.Text;

            // ngày sinh
            DateTime ns;
            if (DateTime.TryParseExact(lblNgaySinh.Text, "dd/MM/yyyy",
                null, System.Globalization.DateTimeStyles.None, out ns))
            {
                txtEditNgaySinh.Text = ns.ToString("yyyy-MM-dd");
            }

            ddlEditGioiTinh.SelectedValue =
                (lblGioiTinh.Text == "Nam") ? "true" : "false";
        }

        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            string maNV = Session["MaNV"].ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
        UPDATE Nhan_vien
        SET HoTen = @ten,
            NgaySinh = @ngaysinh,
            GioiTinh = @gioitinh,
            DiaChi = @diachi
        WHERE MaNV = @ma";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", txtEditHoTen.Text.Trim());
                cmd.Parameters.Add("@ngaysinh", System.Data.SqlDbType.Date) 
                    .Value = DateTime.Parse(txtEditNgaySinh.Text);
                cmd.Parameters.AddWithValue("@gioitinh", ddlEditGioiTinh.SelectedValue);
                cmd.Parameters.AddWithValue("@diachi", txtEditDiaChi.Text.Trim());
                cmd.Parameters.AddWithValue("@ma", maNV);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            pnlEdit.Visible = false;
            LoadThongTinNhanVien(maNV);
        }


    }
}