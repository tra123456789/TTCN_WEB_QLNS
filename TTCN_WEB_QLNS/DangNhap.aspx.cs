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
            string Username = txtUsername.Text;
            string Password = txtPassword.Text;

            if (Username == "" || Password == "")
            {
                lblMessage.Text = "Vui lòng nhập đầy đủ thông tin ";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);
            {
                conn.Open();

            }
            string sql = " Select count(*) From User where Username = @u And Password = @p ";
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add(new SqlParameter("@u", Username));
            cmd.Parameters.Add(new SqlParameter("@p", Password));


            //int result = (int)cmd.ExecuteScalar();

            conn.Close();                                // đóng kết nối

            //if (result > 0)                               // nếu result = 1 tức là đăng nhập đúng
            //{
            Session["UserName"] = Username;           // lưu username vào session

            Response.Redirect("KhenThuong.aspx");           // chuyển sang trang chủ

            //else
            //{
            lblMessage.Text = "Sai tài khoản hoặc mật khẩu.";  // báo lỗi


           // Server.Transfer("QuanLyNhanVien.aspx");
        }

        protected void btnDangKy_Click(object sender, EventArgs e)
        {
            Response.Redirect("DangKy.aspx");
        }
    }
}