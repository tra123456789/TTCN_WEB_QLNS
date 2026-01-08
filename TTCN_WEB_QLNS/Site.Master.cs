using System;
using System.Web;
using System.Web.UI;

namespace TTCN_WEB_QLNS
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. Kiểm tra đăng nhập
            if (Session["UserName"] == null || Session["IDROLE"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }

            string role = Session["IDROLE"].ToString();
            string userName = Session["UserName"].ToString();
            string hoTen = Session["HoTen"] != null ? Session["HoTen"].ToString() : "";

            // logic hiển thị tên thông minh
            if (role == "1")
            {
                // Nếu là Admin, hiển thị "Quản trị viên" hoặc "Admin (SĐT)"
               // lblWelcome.Text = "Xin chào: Quản trị viên (" + userName + ")";
            }
            else
            {
                // Nếu là HR, Kế toán hoặc User: Ưu tiên hiện Họ Tên, nếu trắng thì hiện SĐT
                if (!string.IsNullOrEmpty(hoTen))
                {
                    lblWelcome.Text = "Xin chào: " + hoTen;
                }
                else
                {
                    lblWelcome.Text = "Xin chào: " + userName;
                }
            }
          
            // 2. Phân quyền hiển thị Menu
            // Bước 1: Ẩn tất cả menu (để tránh sai sót)
            SetAllMenuVisible(false);

            // Bước 2: Hiển thị lại dựa trên Role
            if (role == "1") // ADMIN
            {
                SetAllMenuVisible(true);
                menuThongTinNV.Visible = false; // Admin thường không xem trang cá nhân
            }
            else if (role == "11") // HR (NHÂN SỰ)
            {
                menuTongQuan.Visible = true;
                menuNhanVien.Visible = true;
                menuPhongBan.Visible = true;
                menuHopDong.Visible = true;
                menuKhenThuong.Visible = true;
                menuThongTinNV.Visible = false;
            }
            else if (role == "12") // KẾ TOÁN
            {
                menuTongQuan.Visible = true;
                menuChamCong.Visible = true;
             
                menuLuong.Visible = true;
                menuThongTinNV.Visible = true;
                menuDoiMatKhau.Visible = true;
            }
            else if (role == "10") // USER (NHÂN VIÊN)
            {
                menuThongTinNV.Visible = true;
                menuChamCong.Visible = true; 
                menuLuong.Visible = true;    
               
                menuDoiMatKhau.Visible = true;
            }
        }

        // Hàm phụ để ẩn/hiện nhanh tất cả menu
        private void SetAllMenuVisible(bool visible)
        {
            menuTongQuan.Visible = visible;
            menuThongTinNV.Visible = visible;
            menuNhanVien.Visible = visible;
            menuPhongBan.Visible = visible;
            menuChamCong.Visible = visible;
            menuHopDong.Visible = visible;
          
            menuLuong.Visible = visible;
            menuKhenThuong.Visible = visible;
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Default.aspx");
        }
    }
}