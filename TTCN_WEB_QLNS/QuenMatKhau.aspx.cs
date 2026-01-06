using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class QuenMatKhau : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;
       
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string cccd = txtCCCD.Text.Trim();
            string newPass = txtNewPass.Text.Trim();
            string confirmPass = txtConfirmPass.Text.Trim();

            // 1. Kiểm tra để trống dữ liệu
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(cccd) || string.IsNullOrEmpty(newPass))
            {
                lblMessage.Text = "⚠️ Vui lòng điền đầy đủ thông tin!";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            // 2. Kiểm tra mật khẩu khớp nhau
            if (newPass != confirmPass)
            {
                lblMessage.Text = "❌ Xác nhận mật khẩu không khớp!";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Câu lệnh JOIN để xác minh SĐT này có đúng là của nhân viên có số CCCD này không
                string sqlCheck = @"SELECT COUNT(*) 
                                   FROM [User] u 
                                   INNER JOIN Nhan_vien nv ON u.MaNV = nv.MaNV 
                                   WHERE u.Username = @user AND nv.CCCD = @cccd";

                SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn);
                cmdCheck.Parameters.AddWithValue("@user", user);
                cmdCheck.Parameters.AddWithValue("@cccd", cccd);

                try
                {
                    conn.Open();
                    int count = (int)cmdCheck.ExecuteScalar();

                    if (count > 0)
                    {
                        // 3. Thông tin khớp -> Tiến hành cập nhật mật khẩu mới luôn
                        string sqlUpdate = "UPDATE [User] SET Password = @pass WHERE Username = @user";
                        SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn);
                        cmdUpdate.Parameters.AddWithValue("@pass", newPass);
                        cmdUpdate.Parameters.AddWithValue("@user", user);
                        cmdUpdate.ExecuteNonQuery();

                        lblMessage.Text = "✔️ Đổi mật khẩu thành công! Bạn có thể đăng nhập ngay.";
                        lblMessage.ForeColor = Color.Green;

                        // Làm sạch form sau khi thành công
                        txtUsername.Text = txtCCCD.Text = "";
                    }
                    else
                    {
                        lblMessage.Text = "❌ SĐT hoặc số CCCD không chính xác!";
                        lblMessage.ForeColor = Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Lỗi hệ thống: " + ex.Message;
                    lblMessage.ForeColor = Color.Red;
                }
            }
        }
    }
}
