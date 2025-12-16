using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class KhenThuong : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserName"] != null)
            //{
            //    lblWelcome.Text = "Xin chào: " + Session["UserName"].ToString();
            //}
            //else
            //{
            //    Response.Redirect("DangNhap.aspx"); // nếu chưa đăng nhập → quay lại login
            //}
            //if (Session["UserName"] == null || Session["IDROLE"] == null)
            //{
            //    Response.Redirect("KhenThuong.aspx");
            //    return;
            //}

            // Hiển thị tên
            //lblWelcome.Text = "Xin chào: " + Session["UserName"].ToString();

            //// Phân quyền
            //string role = Session["IDROLE"].ToString();

            //if (role == "User")
            //{

            //    menuNhanVien.Visible = false;
            //    menuPhongBan.Visible = false;
            //    menuHopDong.Visible = false;
            //    menuLuong.Visible = false;
            //    menuKhenThuong.Visible = false;
            //}
            if (!IsPostBack)
            {
                LoadDataKhenThuong();
                //string role = Session["IDROLE"].ToString();
                //if (role == "1")
                //{
                //    menuThongTinNV.Visible = false;
                //}
            }
        }

        private void LoadDataKhenThuong()
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(@"SELECT 
    kt.ID,
    kt.MaNV,
    nv.HoTen,
    kt.Loai,
    kt.SoKTKL,
    kt.NoiDung,
    kt.Ngay
FROM KhenThuong_KyLuat kt
JOIN Nhan_vien nv ON kt.MaNV = nv.MaNV

", conn);


                DataTable dt = new DataTable();
                da.Fill(dt);

                gvKhenThuong.DataSource = dt;
                gvKhenThuong.DataBind();
            }
        }

        protected void gvKhenThuong_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvKhenThuong.PageIndex = e.NewPageIndex;
            LoadDataKhenThuong();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvKhenThuong.PageSize = int.Parse(ddlPageSize.SelectedValue);
            LoadDataKhenThuong();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM KhenThuong_KyLuat WHERE MA_NV LIKE @search OR LyDo LIKE @search",
                    conn);

                da.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvKhenThuong.DataSource = dt;
                gvKhenThuong.DataBind();
            }
        }

        protected void gvKhenThuong_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }
    }
}
