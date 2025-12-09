using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class UserHome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Nếu chưa đăng nhập → quay về login
            if (Session["UserName"] == null || Session["IDROLE"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }

            // Nếu ROLE khác User 
            if (Session["IDROLE"].ToString() != "10")
            {
                Response.Redirect("TongQuan.aspx");
                return;
            }

            // Nếu ROLE đúng là User → cho vào trang
            lblWelcome.Text = "Xin chào: " + Session["UserName"].ToString();

        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }
    }
}