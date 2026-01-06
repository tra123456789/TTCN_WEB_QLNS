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
    public partial class ThemHopDong : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // THÊM TỪ TRANG NHÂN VIÊN
                if (Request.QueryString["MaNV"] != null)
                {
                    txtMaNV.Text = Request.QueryString["MaNV"];
                    txtMaNV.ReadOnly = true;
                    txtMaNV.Visible = true;
                    ddlNhanVien.Visible = false;

                    txtNgayKi.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    // THÊM TRỰC TIẾP → CHỌN NHÂN VIÊN
                    LoadNhanVien();
                    ddlNhanVien.Visible = true;
                    txtMaNV.Visible = false;
                }

                // SỬA HỢP ĐỒNG
                if (Request.QueryString["SoHD"] != null)
                {
                    string soHD = Request.QueryString["SoHD"];
                    LoadDataBySoHD(soHD);

                    ddlNhanVien.Visible = false;
                    txtMaNV.Visible = true;
                    txtMaNV.ReadOnly = true;
                }
            }
        }

        void LoadNhanVien()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT MaNV, HoTen FROM Nhan_vien";
                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                ddlNhanVien.DataSource = cmd.ExecuteReader();
                ddlNhanVien.DataTextField = "HoTen";
                ddlNhanVien.DataValueField = "MaNV";
                ddlNhanVien.DataBind();
            }

            ddlNhanVien.Items.Insert(0, new ListItem("-- Chọn nhân viên --", ""));
        }

        void LoadDataBySoHD(string soHD)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Hop_dong WHERE SoHD = @soHD";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@soHD", soHD);

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    txtMaNV.Text = r["MaNV"].ToString();
                    txtMaNV.ReadOnly = true;

                    txtNgayBatDau.Text = Convert.ToDateTime(r["NgayBatDau"]).ToString("yyyy-MM-dd");
                    txtNgayKetThuc.Text = Convert.ToDateTime(r["NgayKetThuc"]).ToString("yyyy-MM-dd");
                    txtNgayKi.Text = Convert.ToDateTime(r["NgayKi"]).ToString("yyyy-MM-dd");

                    txtNoiDung.Text = r["NoiDung"].ToString();
                    txtLanKy.Text = r["LanKy"].ToString();
                    txtThoiHan.Text = r["ThoiHan"].ToString();
                    txtLuongCoBan.Text = r["LuongCoBan"].ToString();

                    ViewState["EditingSoHD"] = soHD;
                }
            }
        }

        protected void btnAddHD_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql;

                if (ViewState["EditingSoHD"] == null)
                {
                    // THÊM MỚI
                    sql = @"
INSERT INTO Hop_dong
(MaNV, NgayBatDau, NgayKetThuc, NgayKi, NoiDung, LanKy, ThoiHan, LuongCoBan)
VALUES
(@manv,@ngaybatdau,@ngayketthuc,@ngayki,@noidung,@lanky,@thoihan,@luong)";
                }
                else
                {
                    // CẬP NHẬT
                    sql = @"
UPDATE Hop_dong SET
NgayBatDau=@ngaybatdau,
NgayKetThuc=@ngayketthuc,
NgayKi=@ngayki,
NoiDung=@noidung,
LanKy=@lanky,
ThoiHan=@thoihan,
LuongCoBan=@luong
WHERE SoHD=@soHD";
                }

                SqlCommand cmd = new SqlCommand(sql, conn);

                string maNV = txtMaNV.Visible
     ? txtMaNV.Text
     : ddlNhanVien.SelectedValue;

                cmd.Parameters.AddWithValue("@manv", maNV);

                cmd.Parameters.AddWithValue("@ngaybatdau", txtNgayBatDau.Text);
                cmd.Parameters.AddWithValue("@ngayketthuc", txtNgayKetThuc.Text);
                cmd.Parameters.AddWithValue("@ngayki", txtNgayKi.Text);
                cmd.Parameters.AddWithValue("@noidung", txtNoiDung.Text);
                cmd.Parameters.AddWithValue("@lanky", txtLanKy.Text);
                cmd.Parameters.AddWithValue("@thoihan", txtThoiHan.Text);
                cmd.Parameters.AddWithValue("@luong", txtLuongCoBan.Text);

                if (ViewState["EditingSoHD"] != null)
                    cmd.Parameters.AddWithValue("@soHD", ViewState["EditingSoHD"]);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("QuanLyHopDong.aspx");
        }


    }
}