using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
                if (Session["UserName"] == null || Session["IDROLE"] == null)
                {
                    Response.Redirect("DangNhap.aspx");
                    return;
                }

                lblWelcome.Text = "Xin chào: " + Session["UserName"];

                string role = Session["IDROLE"].ToString();

                if (role == "1") // ADMIN
                {
                    // Admin KHÔNG xem thông tin cá nhân
                    menuThongTinNV.Visible = false;
                }
                else if (role == "10") // USER
                {
                    // User CHỈ xem: Thông tin cá nhân, Lương, Chấm công, BHXH
                    menuTongQuan.Visible = false;
                    menuNhanVien.Visible = false;
                    menuPhongBan.Visible = false;
                    menuHopDong.Visible = false;
                    menuKhenThuong.Visible = false;
               }
            

        }
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }

    }
}