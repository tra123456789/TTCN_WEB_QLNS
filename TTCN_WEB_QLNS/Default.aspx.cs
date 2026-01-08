using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.Script.Services;

namespace TTCN_WEB_QLNS
{
    public partial class Default : System.Web.UI.Page
    {
        // Chuỗi kết nối lấy từ Web.config
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                plhGuest.Visible = false;
                plhUser.Visible = true;

                // Gán lại ảnh và tên (đề phòng bị mất khi load trang)
                imgAvatar.ImageUrl = ResolveUrl(Session["Avatar"]?.ToString() ?? "~/Images/default-avatar.png");
                litFullName.Text = Session["HoTen"]?.ToString() ?? Session["UserName"].ToString();

                // Gán link quản trị
                lnkAdminDashboard.HRef = GetRedirectUrlByRole();
            }
            else
            {
                plhGuest.Visible = true;
                plhUser.Visible = false;
            }
            if (!IsPostBack)
            {
                SetUserInterface();
            }
        }

        private void SetUserInterface()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["UserName"] != null)
            {
                plhGuest.Visible = false;
                plhUser.Visible = true;
                btnStart.Text = "Vào trang quản trị";

                // Xử lý ảnh đại diện
                string avatarPath = Session["Avatar"] != null ? Session["Avatar"].ToString() : "~/Images/default-avatar.png";
                imgAvatar.ImageUrl = ResolveUrl(avatarPath);

                // Hiển thị họ tên
                litFullName.Text = Session["HoTen"] != null ? Session["HoTen"].ToString() : Session["UserName"].ToString();

                // Xác định link điều hướng cho nút "Tài khoản"
                lnkAdminDashboard.HRef = GetRedirectUrlByRole();
            }
            else
            {
                btnStart.Text = "Trải nghiệm ngay";
                plhGuest.Visible = true;
                plhUser.Visible = false;
            }
        }

        private string GetRedirectUrlByRole()
        {
            string role = Session["IDROLE"] != null ? Session["IDROLE"].ToString() : "";
            switch (role)
            {
                case "1": return "TongQuan.aspx";     // Admin
                case "11": return "HrHome.aspx";      // HR
                case "12": return "KeToanHome.aspx";  // Kế toán
                case "10": return "UserHome.aspx";    // Nhân viên
                default: return "DangNhap.aspx";
            }
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                Response.Redirect(GetRedirectUrlByRole());
            }
            else
            {
                Response.Redirect("DangNhap.aspx");
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Default.aspx");
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            // Bảo mật: Mã hóa HTML để tránh XSS
            string hoTen = HttpUtility.HtmlEncode(txtName.Text.Trim());
            string email = HttpUtility.HtmlEncode(txtEmail.Text.Trim());
            string message = HttpUtility.HtmlEncode(txtMessage.Text.Trim());

            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(email))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Vui lòng nhập tên và email!');", true);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {

                string sql = "INSERT INTO LienHe (HoTen, Email, NoiDung, TrangThai) VALUES (@ten, @email, @nd, N'Chưa xử lý')";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", hoTen);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@nd", message);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Gửi thành công! Chúng tôi sẽ liên hệ sớm.');", true);

                    // Reset Form
                    txtName.Text = txtEmail.Text = txtMessage.Text = "";
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Lỗi: " + ex.Message + "');", true);
                }
            }
        }

      
    }
}