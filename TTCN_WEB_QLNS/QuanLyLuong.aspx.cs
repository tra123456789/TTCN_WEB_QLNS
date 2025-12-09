using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class QuanLyLuong : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string role = Session["IDROLE"].ToString();

            if (role == "User")
            {
                menuTongQuan.Visible = false;
                menuNhanVien.Visible = false;
                menuPhongBan.Visible = false;
                menuHopDong.Visible = false;
                menuLuong.Visible = true;
                menuKhenThuong.Visible = false;
            }

            if (Session["UserName"] != null)
            {
                lblWelcome.Text = "Xin chào, " + Session["UserName"].ToString();
            }
            else
            {
                Response.Redirect("DangNhap.aspx"); // nếu chưa đăng nhập → quay lại login
            }
            if (!IsPostBack)
            {
                LoadDataLuong();
            }
            //if (Session["UserName"] == null || Session["IDROLE"] == null)
            //{
            //    Response.Redirect("QuanLyLuong.aspx");
            //    return;
            //}

            //// Hiển thị tên
            //lblWelcome.Text = "Xin chào: " + Session["UserName"].ToString();

            // Phân quyền
          
        }

        private void LoadDataLuong()
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Bang_luong", conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvLuong.DataSource = dt;
                gvLuong.DataBind();
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            // Tải lại dữ liệu để xuất (nếu cần có filter thì dùng filter)
            LoadDataLuong(); 

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DanhSachLuong.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "utf-8";

            // Thêm BOM UTF-8 cho Excel nhận đúng font
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            // Tắt paging để xuất toàn bộ dữ liệu
            gvLuong.AllowPaging = false;
            LoadDataLuong();

            gvLuong.RenderControl(hw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

       
        public override void VerifyRenderingInServerForm(Control control)
        {
            // bắt buộc để tránh lỗi "Control must be placed inside form"
        }


        protected void gvLuong_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvLuong_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string ID = gvLuong.DataKeys[e.RowIndex].Value.ToString();
            GridViewRow row = gvLuong.Rows[e.RowIndex];

            string ngayCong = ((TextBox)row.FindControl("txtNgayCong")).Text;
            string khongPhep = ((TextBox)row.FindControl("txtKhongPhep")).Text;
            string ngayLe = ((TextBox)row.FindControl("txtNgayLe")).Text;
            string ngaycn = ((TextBox)row.FindControl("txtNgayCN")).Text;
            string ngayThuong = ((TextBox)row.FindControl("txtNgayThuong")).Text;
            string thucLanh = ((TextBox)row.FindControl("txtThucLanh")).Text;

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"UPDATE Bang_luong SET
                        TongNgayCong = @tongNgayCong,
                        KhongPhep = @khongPhep,
                        NgayLe = @ngayLe,
                        NgayCN = @ngaycn,
                        NgayThuong = @ngayThuong,
                        ThucLanh = @thucLanh,
                        Update_date = GETDATE()
                       WHERE ID = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@tongNgayCong", ngayCong);
                cmd.Parameters.AddWithValue("@khongPhep", khongPhep);
                cmd.Parameters.AddWithValue("@ngayLe", ngayLe);
                cmd.Parameters.AddWithValue("@ngayCN", ngaycn);
                cmd.Parameters.AddWithValue("@ngayThuong", ngayThuong);
                cmd.Parameters.AddWithValue("@thucLanh", thucLanh);
                cmd.Parameters.AddWithValue("@id", ID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvLuong.EditIndex = -1;
            LoadDataLuong(); 
        }


        protected void gvLuong_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvLuong.EditIndex = e.NewEditIndex;
            LoadDataLuong();
        }

        protected void gvLuong_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string ID = gvLuong.DataKeys[e.RowIndex].Value.ToString();

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "DELETE FROM Bang_luong WHERE ID=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", ID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadDataLuong();
        }

        protected void gvLuong_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLuong.PageIndex = e.NewPageIndex;
            LoadDataLuong();
        }

        protected void gvLuong_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            gvLuong.EditIndex = -1;
            LoadDataLuong();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {

            gvLuong.PageSize = int.Parse(ddlPageSize.SelectedValue);
            LoadDataLuong();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Bang_luong WHERE MANV LIKE @search OR HoTen LIKE @search",
                    conn);

                da.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvLuong.DataSource = dt;
                gvLuong.DataBind();
            }
        }
        protected void btnDS_Click(object sender, EventArgs e)
        {
            // Tải lại dữ liệu để xuất (nếu cần có filter thì dùng filter)
            LoadDataLuong();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DanhSachLuong.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            // Tắt paging để xuất toàn bộ dữ liệu
            gvLuong.AllowPaging = false;
            LoadDataLuong();

            gvLuong.RenderControl(hw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        protected void btncaculator_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString))
            {
                conn.Open();

                string sql = @"
                             UPDATE Bang_luong
                SET ThucLanh =
                    (ISNULL(TongNgayCong,0) * 300000)
                  + (ISNULL(NgayLe,0) * 300000 * 3)
                  + (ISNULL(NgayPhep,0) * 300000)
                  - (ISNULL(KhongPhep,0) * 300000)              ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }

            LoadDataLuong(); // Load lại grid
        }
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }
    }
}