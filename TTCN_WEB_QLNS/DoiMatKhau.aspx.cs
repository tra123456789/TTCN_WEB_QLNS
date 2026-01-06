using System;
using System.Configuration; 
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI;

namespace TTCN_WEB_QLNS
{
    public partial class DoiMatKhau : System.Web.UI.Page
    {
        // Lấy chuỗi kết nối từ file Web.config
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiểm tra đăng nhập
            if (Session["UserName"] == null)
            {
                Response.Redirect("DangNhap.aspx");
            }
        }

        protected void btnChangePass_Click(object sender, EventArgs e)
        {
            string username = Session["UserName"]?.ToString();

            // Kiểm tra rỗng các ô nhập
            if (string.IsNullOrEmpty(txtOldPass.Text) || string.IsNullOrEmpty(txtNewPass.Text))
            {
                lblMsg.Text = "Vui lòng nhập đầy đủ thông tin!";
                lblMsg.ForeColor = Color.Red;
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // 1. Kiểm tra mật khẩu cũ
                string checkSql = "SELECT COUNT(*) FROM [User] WHERE Username=@user AND Password=@old";
                SqlCommand cmdCheck = new SqlCommand(checkSql, conn);
                cmdCheck.Parameters.AddWithValue("@user", username);
                cmdCheck.Parameters.AddWithValue("@old", txtOldPass.Text);

                try
                {
                    conn.Open();
                    int exists = (int)cmdCheck.ExecuteScalar();

                    if (exists > 0)
                    {
                        if (txtNewPass.Text == txtConfirmPass.Text)
                        {
                            // 2. Cập nhật mật khẩu mới
                            string updateSql = "UPDATE [User] SET Password=@new WHERE Username=@user";
                            SqlCommand cmdUpdate = new SqlCommand(updateSql, conn);
                            cmdUpdate.Parameters.AddWithValue("@new", txtNewPass.Text);
                            cmdUpdate.Parameters.AddWithValue("@user", username);
                            cmdUpdate.ExecuteNonQuery();

                            lblMsg.Text = "Đổi mật khẩu thành công!";
                            lblMsg.ForeColor = Color.Green;

                            // Xóa sạch các ô nhập sau khi thành công
                            txtOldPass.Text = txtNewPass.Text = txtConfirmPass.Text = "";
                        }
                        else
                        {
                            lblMsg.Text = "Xác nhận mật khẩu mới không khớp!";
                            lblMsg.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Mật khẩu cũ không chính xác!";
                        lblMsg.ForeColor = Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Lỗi: " + ex.Message;
                }
            }
        }
    }
}