using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class QuanLyLuong : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserName"] == null || Session["IDROLE"] == null || Session["MaNV"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }


            string role = Session["IDROLE"].ToString();
            if (role == "10" && Session["MaNV"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }
            // Hiển thị tên

            if (!IsPostBack)
            {
                //LoadNhanVien();
                LoadDataLuong();
               
                if (role == "10")
                {
                    
                    btnDS.Visible = false;
                    btncaculator.Visible = false;
                }
                if (Session["IDROLE"].ToString() == "10") 
                {
                    gvLuong.Columns[gvLuong.Columns.Count - 1].Visible = false;
                }
            }
         
        }

        private void LoadDataLuong()
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string role = Session["IDROLE"]?.ToString();
                string sql = "";

                // ADMIN
                if (role == "1" || role == "12")
                {
                    sql = @"
                SELECT 
                    bl.ID,
                    bl.MaNV,
                    nv.HoTen,
                    bl.TongNgayCong,
                    bl.KhongPhep,
                    bl.NgayLe,
                    bl.NgayCN,
                    bl.ThucLanh
                FROM Bang_luong bl
                JOIN Nhan_vien nv ON bl.MaNV = nv.MaNV
            ";
                }
                else // USER
                {
                    sql = @"
                SELECT 
                    bl.ID,
                    bl.MaNV,
                    nv.HoTen,
                    bl.TongNgayCong,
                    bl.KhongPhep,
                    bl.NgayLe,
                    bl.NgayCN,
                    bl.ThucLanh
                FROM Bang_luong bl
                JOIN Nhan_vien nv ON bl.MaNV = nv.MaNV
                WHERE bl.MaNV = @MaNV
            ";
                }

                SqlCommand cmd = new SqlCommand(sql, conn);

                if (role != "1" && role != "12")
                {
                    cmd.Parameters.AddWithValue("@MaNV", Session["MaNV"].ToString());
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvLuong.DataSource = dt;
                gvLuong.DataBind();
            }
        }

        //private void LoadNhanVien()
        //{
        //    string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        string sql = "SELECT MaNV, HoTen FROM NhanVien WHERE TrangThai = 1";
        //        SqlDataAdapter da = new SqlDataAdapter(sql, conn);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        ddlNhanVien.DataSource = dt;
        //        ddlNhanVien.DataTextField = "HoTen";   // hiển thị tên
        //        ddlNhanVien.DataValueField = "MaNV";   // giá trị thật
        //        ddlNhanVien.DataBind();

        //        ddlNhanVien.Items.Insert(0, new ListItem("-- Chọn nhân viên --", ""));
        //    }
        //}


        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string role = Session["IDROLE"].ToString();

            if (role == "10")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "noPermission",
              "alert('Bạn không có quyền xuất danh sách lương!');", true);
                return;
            }

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

        protected void btnAddLuong_Click(object sender, EventArgs e)
        {
            // Chỉ Admin được thêm
            if (Session["IDROLE"].ToString() == "10")
            {
                ScriptManager.RegisterStartupScript(this, GetType(),
                    "no", "alert('Bạn không có quyền thêm lương!');", true);
                return;
            }
            //string maNV = ddlNhanVien.SelectedValue;
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
            INSERT INTO Bang_luong
            (MaNV, TongNgayCong, KhongPhep, NgayLe, NgayCN, ThucLanh, Create_date)
            VALUES
            (@MaNV, @TongNgayCong, @KhongPhep, @NgayLe, @NgayCN, 0, GETDATE())
        ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
                cmd.Parameters.AddWithValue("@TongNgayCong", txtNgayCong.Text);
                cmd.Parameters.AddWithValue("@KhongPhep", txtKhongPhep.Text);
                cmd.Parameters.AddWithValue("@NgayLe", txtNgayLe.Text);
                cmd.Parameters.AddWithValue("@NgayCN", txtNgayCN.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadDataLuong();

            ScriptManager.RegisterStartupScript(this, GetType(),
                "ok", "alert('Thêm lương thành công!');", true);
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
            //string ngayThuong = ((TextBox)row.FindControl("txtNgayThuong")).Text;
            //string thucLanh = ((TextBox)row.FindControl("txtThucLanh")).Text;

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"UPDATE Bang_luong SET
                        TongNgayCong = @tongNgayCong,
                        KhongPhep = @khongPhep,
                        NgayLe = @ngayLe,
                        NgayCN = @ngaycn,
                      
                       
                        Update_date = GETDATE()
                       WHERE ID = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@tongNgayCong", ngayCong);
                cmd.Parameters.AddWithValue("@khongPhep", khongPhep);
                cmd.Parameters.AddWithValue("@ngayLe", ngayLe);
                cmd.Parameters.AddWithValue("@ngayCN", ngaycn);
                //cmd.Parameters.AddWithValue("@ngayThuong", ngayThuong);
                //cmd.Parameters.AddWithValue("@thucLanh", thucLanh);
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

                string role = Session["IDROLE"].ToString();
                SqlDataAdapter da;

                if (role == "1" || role == "12")   // ADMIN
                {
                    da = new SqlDataAdapter(
                        "SELECT * FROM Bang_luong WHERE MANV LIKE @search OR HoTen LIKE @search",
                        conn);
                }
                else   // USER → Chỉ search chính mình
                {
                    da = new SqlDataAdapter(
                        "SELECT * FROM Bang_luong WHERE MaNV = @MaNV AND (HoTen LIKE @search)",
                        conn);

                    da.SelectCommand.Parameters.AddWithValue("@MaNV", Session["MaNV"].ToString());
                }

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

            if (Session["IDROLE"].ToString() == "10")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "noPermission",
              "alert('Bạn không có quyền tính lương!');", true);

                return;
            }
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString))
            {
                conn.Open();

                string sql = @"
                            
        UPDATE Bang_luong
        SET ThucLanh =
            (ISNULL(TongNgayCong,0) * 300000)
          + (ISNULL(NgayLe,0) * 300000 * 3)
          + (ISNULL(NgayPhep,0) * 300000)
          - (ISNULL(KhongPhep,0) * 300000)
          + (
                SELECT ISNULL(SUM(SoKTKL),0)
                FROM KhenThuong_KyLuat kt
                WHERE kt.MaNV = Bang_luong.MaNV
                  AND kt.Loai = 1
            )
          - (
                SELECT ISNULL(SUM(SoKTKL),0)
                FROM KhenThuong_KyLuat kt
                WHERE kt.MaNV = Bang_luong.MaNV
                  AND kt.Loai = 0
            ),
            Update_date = GETDATE()           ";

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

        protected void gvLuong_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string role = Session["IDROLE"]?.ToString();

                // Lấy từng nút trong dòng
                Button btnExport = e.Row.FindControl("btnExportExcel") as Button;
                LinkButton btnEdit = e.Row.FindControl("BtnEdit") as LinkButton;
                LinkButton btnDelete = e.Row.FindControl("BtnDelete") as LinkButton;
                LinkButton btnUpdate = e.Row.FindControl("btnUpdate") as LinkButton;
                LinkButton btnCancel = e.Row.FindControl("BtnCancel") as LinkButton;

                if (role == "10")
                {
                    // User không được phép sửa / xóa / cập nhật / hủy / export
                    if (btnExport != null) btnExport.Visible = false;
                    if (btnEdit != null) btnEdit.Visible = false;
                    if (btnDelete != null) btnDelete.Visible = false;
                    if (btnUpdate != null) btnUpdate.Visible = false;
                    if (btnCancel != null) btnCancel.Visible = false;
                }
            }
        }

    }
}