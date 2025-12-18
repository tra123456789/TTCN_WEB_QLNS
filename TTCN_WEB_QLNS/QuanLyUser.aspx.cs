using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

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
            //    Response.Redirect("QuanLyUser.aspx");
            //    return;
            //}

            // Hiển thị tên
            //lblWelcome.Text = "Xin chào: " + Session["UserName"].ToString();


            //// Phân quyền
            //string role = Session["IDROLE"].ToString();

            //if (role == "User")
            //{
            //    menuTongQuan.Visible = false;
            //    menuNhanVien.Visible = false;
            //    menuPhongBan.Visible = false;
            //    menuHopDong.Visible = false;
            //    menuLuong.Visible = false;
            //    menuKhenThuong.Visible = false;
            //}

            if (!IsPostBack)
            {
                LoadDataQuanLyUser(false);
                //string role = Session["IDROLE"].ToString();
                //if (role == "1")
                //{
                //    //menuThongTinNV.Visible = false;
                //}
            }
        }
        //Hàm xử lý Hiển thị lên bảng
       


        private void LoadDataQuanLyUser(bool hienNghi = false)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = @"
        SELECT 
    nv.MaNV,
    nv.HoTen,
    CASE 
        WHEN nv.GioiTinh = 1 THEN N'Nam'
        WHEN nv.GioiTinh = 0 THEN N'Nữ'
        ELSE N'Chưa xác định'
    END AS GioiTinhText,
    nv.NgaySinh,
    nv.SDT,
    nv.CCCD,
    nv.DiaChi,
    nv.HinhAnh,
    nv.TrangThai,
    bp.IDBP,
    bp.TenBP,
    pb.IDPB,
    pb.TenPB
FROM Nhan_vien nv
LEFT JOIN Bo_phan bp ON nv.IDBP = bp.IDBP
LEFT JOIN Phong_ban pb ON bp.IDPB = pb.IDPB
WHERE nv.TrangThai = @trangthai  ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@trangthai", SqlDbType.Int).Value = hienNghi ? 0 : 1;


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvQuanLyUser.DataSource = dt;
                gvQuanLyUser.DataBind();
            }
        }




        protected void gvQuanLyUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvQuanLyUser.PageIndex = e.NewPageIndex;
            LoadDataQuanLyUser();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvQuanLyUser.PageSize = int.Parse(ddlPageSize.SelectedValue);
            LoadDataQuanLyUser();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
         

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Nhan_vien WHERE MaNV LIKE @search OR HoTen LIKE @search",
                    conn);

                da.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvQuanLyUser.DataSource = dt;
                gvQuanLyUser.DataBind();
            }
        }


        protected void gvQuanLyUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void txtNgaySinh_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtSDT_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtCCCD_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtDiaChi_TextChanged(object sender, EventArgs e)
        {

        }

       

        protected void btnSave_Click(object sender, EventArgs e)
        {
           
        }

        protected void txtHoTen_TextChanged(object sender, EventArgs e)
        {

        }

        protected void gvQuanLyUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Restore")
            {
                string manv = e.CommandArgument.ToString();

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = "UPDATE Nhan_vien SET TrangThai = 1 WHERE MaNV = @id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", manv);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadDataQuanLyUser(chkNhanVienNghi.Checked);

                ScriptManager.RegisterStartupScript(this, GetType(), "ok",
                    "alert('✔ Đã khôi phục nhân viên!');", true);
            }
            else if (e.CommandName == "Detail")
            {
                string maNV = e.CommandArgument.ToString();

                // Chuyển sang trang chi tiết
                Response.Redirect("ThongTinCaNhan.aspx?MaNV=" + maNV);
            }
        }


        protected void gvQuanLyUser_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvQuanLyUser.EditIndex= e.NewEditIndex;
            LoadDataQuanLyUser();
        }



        protected void gvQuanLyUser_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string manv = gvQuanLyUser.DataKeys[e.RowIndex].Value.ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "UPDATE Nhan_vien SET TrangThai = 0 WHERE MaNV = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", manv);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadDataQuanLyUser();

            ScriptManager.RegisterStartupScript(this, GetType(), "ok",
                "alert('✔ Nhân viên đã được cho nghỉ việc (xóa mềm)!');", true);
        }

        protected void gvQuanLyUser_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string manv = gvQuanLyUser.DataKeys[e.RowIndex].Value.ToString();
            GridViewRow row = gvQuanLyUser.Rows[e.RowIndex];

            string hoten = ((TextBox)row.Cells[1].Controls[0]).Text;
            DateTime ngaysinh = DateTime.Parse(((TextBox)row.Cells[2].Controls[0]).Text);
            string sdt = ((TextBox)row.Cells[3].Controls[0]).Text;
            string cccd = ((TextBox)row.Cells[4].Controls[0]).Text;
            string diachi = ((TextBox)row.Cells[5].Controls[0]).Text;

            // ============================
            // BỘ PHẬN
            // ============================
            DropDownList ddlBP = row.FindControl("ddlBP_Grid") as DropDownList;
            string idbp = ddlBP != null ? ddlBP.SelectedValue : null;

            // ============================
            // ẢNH
            // ============================
            string imgPath = null;
            FileUpload fu = row.FindControl("fuEditAvatar") as FileUpload;

            if (fu != null && fu.HasFile)
            {
                string folder = Server.MapPath("~/Images/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Path.GetFileName(fu.FileName);
                imgPath = "~/Images/" + fileName;
                fu.SaveAs(Path.Combine(folder, fileName));
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = @"
UPDATE Nhan_vien
SET HoTen=@ten,
    NgaySinh=@ngay,
    SDT=@sdt,
    CCCD=@cccd,
    DiaChi=@diachi,
    IDBP=@idbp"
                + (imgPath != null ? ", HinhAnh=@img" : "") +
                @" WHERE MaNV=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", hoten);
                cmd.Parameters.AddWithValue("@ngay", ngaysinh);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.Parameters.AddWithValue("@cccd", cccd);
                cmd.Parameters.AddWithValue("@diachi", diachi);
                cmd.Parameters.AddWithValue("@idbp", (object)idbp ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id", manv);

                if (imgPath != null)
                    cmd.Parameters.AddWithValue("@img", imgPath);

                cmd.ExecuteNonQuery();
            }

            gvQuanLyUser.EditIndex = -1;
            LoadDataQuanLyUser(chkNhanVienNghi.Checked);
        }



        protected void gvQuanLyUser_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvQuanLyUser.EditIndex = -1;
            LoadDataQuanLyUser();
        }
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }
        protected void chkNhanVienNghi_CheckedChanged(object sender, EventArgs e)
        {
            LoadDataQuanLyUser(chkNhanVienNghi.Checked);
        }

        protected void gvQuanLyUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int trangThai = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TrangThai"));
                bool dangXemNghi = chkNhanVienNghi.Checked;

                var btnDetail = e.Row.FindControl("LinkButtonDetail") as LinkButton;
                var btnDelete = e.Row.FindControl("LinkButtonDelete") as LinkButton;
                var btnRestore = e.Row.FindControl("LinkButtonRestore") as LinkButton;

                if (dangXemNghi && trangThai == 0)
                {
                    if (btnDelete != null) btnDelete.Visible = false;
                    if (btnDetail != null) btnDetail.Visible = false; // ⬅ tuỳ chọn
                    if (btnRestore != null) btnRestore.Visible = true;

                    e.Row.ForeColor = System.Drawing.Color.Gray;
                }
                else
                {
                    if (btnRestore != null) btnRestore.Visible = false;
                }

                // =========================
                // BIND DROPDOWN PHÒNG BAN KHI EDIT
                // =========================
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlBP = e.Row.FindControl("ddlBP_Grid") as DropDownList;
                    if (ddlBP != null)
                    {
                        using (SqlConnection conn = new SqlConnection(connStr))
                        {
                            SqlDataAdapter da = new SqlDataAdapter(
                                "SELECT IDBP, TenBP FROM Bo_phan", conn);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            ddlBP.DataSource = dt;
                            ddlBP.DataTextField = "TenBP";
                            ddlBP.DataValueField = "IDBP";
                            ddlBP.DataBind();
                        }

                        ddlBP.SelectedValue = DataBinder.Eval(e.Row.DataItem, "IDBP").ToString();
                    }

                }
            }
        }




    }
}
