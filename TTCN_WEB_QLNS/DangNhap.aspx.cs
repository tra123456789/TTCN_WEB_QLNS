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

                            string sql = @"
            SELECT u.MaNV, u.IDROLE
            FROM [User] u
            LEFT JOIN Nhan_vien nv ON u.MaNV = nv.MaNV
            WHERE u.Username = @u
              AND u.Password = @p
              AND u.IsActive = 1
              AND (
                    u.IDROLE IN (1, 12)
                    OR (u.IDROLE = 10 AND nv.TrangThai = 1)
                  )";


                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", Username);
                cmd.Parameters.AddWithValue("@p", Password);

                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    Session["UserName"] = Username;
                    Session["IDROLE"] = rd["IDROLE"].ToString();
                    Session["MaNV"] = rd["MaNV"].ToString();

                    string role = rd["IDROLE"].ToString();

                    if (role == "1" || role == "12")
                        Response.Redirect("TongQuan.aspx");
                    else if (role == "10")
                        Response.Redirect("UserHome.aspx");
                }
                else
                {
                    lblMessage.Text = "Tài khoản không tồn tại hoặc đã bị khóa.";
                }
            }
        


    }
    protected void btnDangKy_Click(object sender, EventArgs e)
        {
            Response.Redirect("DangKy.aspx");
        }
    }
}