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
                string sql = @"SELECT u.MaNV, u.IDROLE, nv.HoTen, nv.HinhAnh 
    FROM [User] u
    LEFT JOIN Nhan_vien nv ON u.MaNV = nv.MaNV
    WHERE u.Username = @u AND u.Password = @p AND u.IsActive = 1";

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

                    // Nếu rỗng thì hiện tên đăng nhập hoặc "Quản trị viên" thay vì để trống
                    string hoTen = rd["HoTen"] != DBNull.Value ? rd["HoTen"].ToString() : "";
                    Session["HoTen"] = string.IsNullOrEmpty(hoTen) ? "Quản trị viên" : hoTen;

                    // Xử lý ảnh đại diện
                    string hinhAnh = rd["HinhAnh"] != DBNull.Value ? rd["HinhAnh"].ToString() : "";

                    if (!string.IsNullOrEmpty(hinhAnh))
                    {
                        // Đảm bảo đường dẫn luôn có dấu ~ để ResolveUrl ở trang Default không bị lỗi
                        Session["Avatar"] = hinhAnh.StartsWith("~") ? hinhAnh : "~/" + hinhAnh.TrimStart('/');
                    }
                    else
                    {
                        Session["Avatar"] = "~/Images/default-avatar.png";
                    }

                    Response.Redirect("Default.aspx");
                }
                else
                {
                    lblMessage.Text = "Sai tài khoản, mật khẩu hoặc tài khoản bị khóa.";
                }
            }
        }

       
    
    protected void btnDangKy_Click(object sender, EventArgs e)
        {
            Response.Redirect("DangKy.aspx");
        }
    }
}