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
            string Username = txtUsername.Text.Trim();
            string Password = txtPassword.Text.Trim();

            if (Username == "" || Password == "")
            {
                lblMessage.Text = "Vui lòng nhập đầy đủ thông tin.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = "SELECT IDROLE FROM [User] WHERE Username = @u AND Password = @p";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", Username);
                cmd.Parameters.AddWithValue("@p", Password);

                object role = cmd.ExecuteScalar();   // lấy quyền hoặc null

                if (role != null)
                {
                    // Lưu thông tin đăng nhập
                    Session["UserName"] = Username;
                    Session["IDROLE"] = role.ToString();

                    // Điều hướng theo IDROLE
                    if (role.ToString() == "1" || role.ToString() == "12")
                        Response.Redirect("TongQuan.aspx");
                    else if (role.ToString() == "10")
                        Response.Redirect("UserHome.aspx");
                    else
                        lblMessage.Text = "Role không hợp lệ!";
                }
                else
                {
                    lblMessage.Text = "Sai tài khoản hoặc mật khẩu.";
                }
            }



        }

        protected void btnDangKy_Click(object sender, EventArgs e)
        {
            Response.Redirect("DangKy.aspx");
        }
    }
}