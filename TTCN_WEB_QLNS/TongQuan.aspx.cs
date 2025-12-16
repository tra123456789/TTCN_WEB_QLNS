using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class TongQuan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["IDROLE"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }

            // Nếu ROLE khác User 
            if (Session["IDROLE"].ToString() != "1")
            {
               
                Response.Redirect("UserHome.aspx");
                return;
            }

            // Nếu ROLE đúng là User → cho vào trang
            lblWelcome.Text = "Xin chào: " + Session["UserName"].ToString();


            if (!IsPostBack)
            {
                LoadDashboard();

                string role = Session["IDROLE"].ToString();
                if (role == "1")
                {
                    //menuThongTinNV.Visible = false;
                }
            }
            //if (Session["UserName"] != null)
            //{
            //    lblWelcome.Text = "Xin chào, " + Session["UserName"].ToString();
            //}
            //else
            //{
            //    Response.Redirect("DangNhap.aspx"); // nếu chưa đăng nhập → quay lại login
            //}
        }
        void LoadDashboard()
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Tổng nhân viên
                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Nhan_vien", conn);
                lblEmp.Text = cmd1.ExecuteScalar().ToString();

                // Tổng phòng ban
                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Phong_ban", conn);
                lblDept.Text = cmd2.ExecuteScalar().ToString();

                // Tổng hệ số/bảng lương
                SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM Bang_luong", conn);
                lblSalary.Text = cmd3.ExecuteScalar().ToString();

                // Tổng khen thưởng
                SqlCommand cmd4 = new SqlCommand("SELECT COUNT(*) FROM Khenthuong_Kyluat", conn);
                lblReward.Text = cmd4.ExecuteScalar().ToString();
                // Hợp Đồng
                SqlCommand cmd5 = new SqlCommand("SELECT COUNT(*) FROM Hop_dong", conn);
                lblhd.Text = cmd5.ExecuteScalar().ToString();
                // Bảo hiểm
                SqlCommand cmd6 = new SqlCommand("SELECT COUNT(*) FROM Bao_hiem", conn);
                lblbh.Text = cmd6.ExecuteScalar().ToString();
            }
        }
        protected void btntsnv_Click(object sender, EventArgs e)
        {
            Response.Redirect("QuanLyUser.aspx");
        }

        protected void btnpban_Click(object sender, EventArgs e)
        {
            Response.Redirect("PhongBan.aspx");
        }

        protected void btnbluong_Click(object sender, EventArgs e)
        {
            Response.Redirect("QuanLyLuong.aspx");
        }
        protected void btnkthuong_Click(object sender, EventArgs e)
        {
            Response.Redirect("KhenThuong.aspx");
        }
        protected void btnhd_Click(object sender, EventArgs e)
        {
            Response.Redirect("QuanLyHopDong.aspx");
        }
        protected void btnbh_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaoHiem.aspx");
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }
        protected void btnLuuCauHinh_Click(object sender, EventArgs e)
        {
            // 1️⃣ Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(txtMoTu.Text) ||
                string.IsNullOrWhiteSpace(txtDongDen.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "err", "alert('❌ Vui lòng nhập đầy đủ thời gian mở và đóng');", true);
                return;
            }

            DateTime mo, dong;

            // 2️⃣ Kiểm tra đúng định dạng ngày
            if (!DateTime.TryParse(txtMoTu.Text, out mo) ||
                !DateTime.TryParse(txtDongDen.Text, out dong))
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "err2", "alert('❌ Thời gian không hợp lệ');", true);
                return;
            }

            // 3️⃣ Kiểm tra logic thời gian
            if (mo >= dong)
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "err3", "alert('❌ Thời gian mở phải nhỏ hơn thời gian đóng');", true);
                return;
            }

            // 4️⃣ Lưu DB
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString))
            {
                string sql = @"
        IF EXISTS (SELECT 1 FROM CauHinhSuaThongTin)
            UPDATE CauHinhSuaThongTin
            SET ThoiGianMo = @mo, ThoiGianDong = @dong
        ELSE
            INSERT INTO CauHinhSuaThongTin (ThoiGianMo, ThoiGianDong)
            VALUES (@mo, @dong)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@mo", SqlDbType.DateTime).Value = mo;
                cmd.Parameters.Add("@dong", SqlDbType.DateTime).Value = dong;

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ScriptManager.RegisterStartupScript(this, GetType(),
                "ok", "alert('✔ Đã cập nhật thời gian cho phép chỉnh sửa');", true);
        }



    }
}