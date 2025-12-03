using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class DangKy : System.Web.UI.Page
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

        protected void txtPassword2_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {

        }

        protected void btnDangKy_Click(object sender, EventArgs e)
        {

            string Username = txtUsername.Text;
            string Password = txtPassword.Text;
            string rePassword = txtPassword2.Text;

            if (Username == "" || Password == "" || rePassword == "")
            {
                lblMessage.Text = "Vui lòng nhập đầy đủ.";
                return;
            }

            if (Password != rePassword)
            {
                lblMessage.Text = "Mật khẩu không khớp.";
                return;
            }

            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            //kiểm tra trùng username
            SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM [User] WHERE Username=@u", conn);
            check.Parameters.AddWithValue("@u", Username);
            int count = (int)check.ExecuteScalar();
            if (count > 0)
            {
                lblMessage.Text = "Tên đăng nhập đã tồn tại.";
                conn.Close();
                return;
            }

            // thêm mới
            SqlCommand cmd = new SqlCommand("INSERT INTO [User](Username, Password) VALUES(@u, @p)", conn);
            cmd.Parameters.AddWithValue("@u", Username);
            cmd.Parameters.AddWithValue("@p", Password);

            cmd.ExecuteNonQuery();

            conn.Close();

            Response.Redirect("DangNhap.aspx");


        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("DangNhap.aspx");
        }
    }
}