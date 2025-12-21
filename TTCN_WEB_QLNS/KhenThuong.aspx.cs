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
                LoadThang();
                LoadNam();

                ddlThang.SelectedValue = DateTime.Now.Month.ToString();
                ddlNam.SelectedValue = DateTime.Now.Year.ToString();

            }
        }
        void LoadThang()
        {
            ddlThang.Items.Clear();
            for (int i = 1; i <= 12; i++)
                ddlThang.Items.Add(i.ToString());
        }
        void LoadNam()
        {
            ddlNam.Items.Clear();
            for (int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year + 1; i++)
                ddlNam.Items.Add(i.ToString());
        }
        void LoadKhenThuong()
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);
            string keyword = txtSearch.Text.Trim();

            string sql = @"
        SELECT 
            kt.MaNV,
            nv.HoTen,
            kt.Ngay,
            kt.NoiDung,
            kt.SoKTKL,
            kt.Loai
        FROM KhenThuong_KyLuat kt
        JOIN Nhan_vien nv ON nv.MaNV = kt.MaNV
        WHERE MONTH(kt.Ngay) = @Thang
          AND YEAR(kt.Ngay) = @Nam
          AND (nv.HoTen LIKE @kw OR kt.MaNV LIKE @kw)
        ORDER BY kt.Ngay DESC";

            using (SqlConnection conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);
                cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvKhenThuong.DataSource = dt;
                gvKhenThuong.DataBind();
            }
        }
        //load tháng năm
        protected void FilterChanged(object sender, EventArgs e)
        {
            LoadKhenThuong();
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
        // xuất excel 
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);

            // 1️⃣ Lấy dữ liệu giống hệt GridView
            DataTable dt = GetKhenThuongTheoThang(thang, nam);

            // 2️⃣ Xuất Excel
            ExportToExcel(dt, $"KhenThuong_{thang}_{nam}.xls");
        }
        private DataTable GetKhenThuongTheoThang(int thang, int nam)
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            string sql = @"
        SELECT 
            kt.MaNV,
            nv.HoTen,
            kt.Ngay,
            kt.NoiDung,
            kt.SoKTKL,
            CASE 
                WHEN kt.Loai = 1 THEN N'Khen thưởng'
                ELSE N'Kỷ luật'
            END AS Loai
        FROM KhenThuong_KyLuat kt
        JOIN Nhan_vien nv ON nv.MaNV = kt.MaNV
        WHERE MONTH(kt.Ngay) = @Thang
          AND YEAR(kt.Ngay) = @Nam
        ORDER BY kt.Ngay";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        private void ExportToExcel(DataTable dt, string fileName)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            // Tạo GridView tạm để render Excel
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            gv.RenderControl(hw);

            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/>");
            Response.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

    }

}
