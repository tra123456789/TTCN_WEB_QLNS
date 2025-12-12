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
            if (!IsPostBack)
            {
                if (Session["UserName"] == null || Session["IDROLE"] == null)
                {
                    Response.Redirect("DangNhap.aspx");
                    return;
                }

                if (Session["IDROLE"].ToString() != "10")
                {
                    Response.Redirect("TongQuan.aspx");
                    return;
                }

                lblWelcome.Text = "Xin chào: " + Server.HtmlEncode(Session["UserName"].ToString());

            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }
    }
}