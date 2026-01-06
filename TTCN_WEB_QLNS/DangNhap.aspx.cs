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
    public partial class DangNhap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "Vui lòng nhập đầy đủ tài khoản và mật khẩu.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // THÊM: nv.HoTen vào câu lệnh SELECT
                string sql = @"
            SELECT u.MaNV, u.IDROLE, nv.HoTen
            FROM [User] u
            LEFT JOIN Nhan_vien nv ON u.MaNV = nv.MaNV
            WHERE u.Username = @u
              AND u.Password = @p
              AND u.IsActive = 1
              AND (
                    u.IDROLE IN (1, 11, 12)
                    OR (u.IDROLE = 10 AND nv.TrangThai = 1)
                  )";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);

                conn.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    Session["UserName"] = username;
                    Session["MaNV"] = rd["MaNV"].ToString();
                    Session["IDROLE"] = rd["IDROLE"].ToString();

                    // LƯU HỌ TÊN VÀO SESSION: Nếu HoTen null (như Admin) thì gán chuỗi rỗng
                    Session["HoTen"] = rd["HoTen"] != DBNull.Value ? rd["HoTen"].ToString() : "";

                    RedirectByRole(rd["IDROLE"].ToString());
                }
                else
                {
                    lblMessage.Text = "Sai tài khoản, mật khẩu hoặc tài khoản bị khóa.";
                }
            }
        }

        void RedirectByRole(string role)
        {
            switch (role)
            {
                case "1": // Admin
                    Response.Redirect("TongQuan.aspx");
                    break;

                case "11": // HR
                    Response.Redirect("HrHome.aspx");
                    break;

                case "12": // Kế toán
                    Response.Redirect("KeToanHome.aspx");
                    break;

                case "10": // Nhân viên
                    Response.Redirect("UserHome.aspx");
                    break;

                default:
                    Response.Redirect("DangNhap.aspx");
                    break;
            }
        }
    
    protected void btnDangKy_Click(object sender, EventArgs e)
        {
            Response.Redirect("DangKy.aspx");
        }
    }
}