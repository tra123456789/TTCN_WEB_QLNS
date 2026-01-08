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
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["IDROLE"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }

            string role = Session["IDROLE"].ToString();

            // Thay vì dùng role != 1, hãy kiểm tra và chuyển hướng đúng trang chủ của từng Role
            if (role == "11") // HR
            {
                Response.Redirect("HrHome.aspx");
                return;
            }
            else if (role == "12") // Kế toán
            {
                Response.Redirect("KeToanHome.aspx");
                return;
            }
            else if (role == "10") // User
            {
                Response.Redirect("UserHome.aspx");
                return;
            }

            // Nếu là Admin (Role 1) thì mới chạy tiếp các lệnh dưới đây
            lblWelcome.Text = "Xin chào : " + Session["UserName"];

            if (!IsPostBack)
            {
                LoadDashboard();
                LoadLienHe();
            }
        }

        void LoadDashboard()
        {
      
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
              
              
                // Hợp Đồng
                SqlCommand cmd5 = new SqlCommand("SELECT COUNT(*) FROM Hop_dong", conn);
                lblhd.Text = cmd5.ExecuteScalar().ToString();
                // Bảo hiểm
              
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

    
        protected void btnhd_Click(object sender, EventArgs e)
        {
            Response.Redirect("QuanLyHopDong.aspx");
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
            using (SqlConnection conn = new SqlConnection(connStr))
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

        private void LoadLienHe()
        {
                     using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Lấy danh sách, ưu tiên yêu cầu mới chưa xử lý lên trước
                string sql = "SELECT * FROM LienHe ORDER BY TrangThai DESC, NgayGui DESC";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvLienHe.DataSource = dt;
                gvLienHe.DataBind();
            }
        }
        protected void btnConfirm_Click(object sender, CommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
     
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Cập nhật trạng thái thành 'Đã xử lý'
                string sql = "UPDATE LienHe SET TrangThai = N'Đã xử lý' WHERE ID = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Load lại GridView để cập nhật giao diện
            LoadLienHe();
        }

    }
}