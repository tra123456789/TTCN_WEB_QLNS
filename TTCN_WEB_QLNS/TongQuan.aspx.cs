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
    public partial class TongQuan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["IDROLE"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }

            // Nếu ROLE khác User 
            if (Session["IDROLE"].ToString() != "1")
            {
               
                Response.Redirect("UserHome.aspx");
                return;
            }

            // Nếu ROLE đúng là User → cho vào trang
            lblWelcome.Text = "Xin chào: " + Session["UserName"].ToString();


            if (!IsPostBack)
            {
                LoadDashboard();

                string role = Session["IDROLE"].ToString();
                if (role == "1")
                {
                    menuThongTinNV.Visible = false;
                }
            }
            //if (Session["UserName"] != null)
            //{
            //    lblWelcome.Text = "Xin chào, " + Session["UserName"].ToString();
            //}
            //else
            //{
            //    Response.Redirect("DangNhap.aspx"); // nếu chưa đăng nhập → quay lại login
            //}
        }
        void LoadDashboard()
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Tổng nhân viên
                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Nhan_vien", conn);
                lblEmp.Text = cmd1.ExecuteScalar().ToString();

                // Tổng phòng ban
                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Phong_ban", conn);
                lblDept.Text = cmd2.ExecuteScalar().ToString();

                // Tổng hệ số/bảng lương
                SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM Bang_luong", conn);
                lblSalary.Text = cmd3.ExecuteScalar().ToString();

                // Tổng khen thưởng
                SqlCommand cmd4 = new SqlCommand("SELECT COUNT(*) FROM Khenthuong_Kyluat", conn);
                lblReward.Text = cmd4.ExecuteScalar().ToString();
                // Hợp Đồng
                SqlCommand cmd5 = new SqlCommand("SELECT COUNT(*) FROM Hop_dong", conn);
                lblhd.Text = cmd5.ExecuteScalar().ToString();
                // Bảo hiểm
                SqlCommand cmd6 = new SqlCommand("SELECT COUNT(*) FROM Bao_hiem", conn);
                lblbh.Text = cmd6.ExecuteScalar().ToString();
            }
        }
        protected void btntsnv_Click(object sender, EventArgs e)
        {
            Response.Redirect("QuanLyUser.aspx");
        }

        protected void btnpban_Click(object sender, EventArgs e)
        {
            Response.Redirect("PhongBan.aspx");
        }

        protected void btnbluong_Click(object sender, EventArgs e)
        {
            Response.Redirect("QuanLyLuong.aspx");
        }
        protected void btnkthuong_Click(object sender, EventArgs e)
        {
            Response.Redirect("KhenThuong.aspx");
        }
        protected void btnhd_Click(object sender, EventArgs e)
        {
            Response.Redirect("QuanLyHopDong.aspx");
        }
        protected void btnbh_Click(object sender, EventArgs e)
        {
            Response.Redirect("BaoHiem.aspx");
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }
    }
}