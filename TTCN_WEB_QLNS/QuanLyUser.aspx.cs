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
        void LoadSingleUser(string maNV)
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Nhan_vien WHERE MaNV = @ma";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ma", maNV);

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    txtHoTen.Text = r["HoTen"].ToString();
                    txtNgaySinh.Text = Convert.ToDateTime(r["NgaySinh"]).ToString("yyyy-MM-dd");
                    txtSDT.Text = r["SDT"].ToString();
                    txtCCCD.Text = r["CCCD"].ToString();
                    txtDiaChi.Text = r["DiaChi"].ToString();

                    // lưu để biết đang sửa
                    ViewState["EditingMaNV"] = maNV;
                }
            }

            //btnSave.Text = "💾 Cập nhật";
        }



        private void LoadDataQuanLyUser(bool hienNghi = false)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = @"
            SELECT nv.MaNV, nv.HoTen, nv.NgaySinh, nv.SDT, nv.CCCD, nv.DiaChi,
                   nv.HinhAnh, nv.TrangThai,
                   nv.IDPB, pb.TenPB
            FROM Nhan_vien nv
            LEFT JOIN Phong_ban pb ON nv.IDPB = pb.IDPB
            WHERE nv.TrangThai = @trangthai";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@trangthai", hienNghi ? 0 : 1);

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

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            // ============================
            // VALIDATE RỖNG
            // ============================
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtNgaySinh.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtCCCD.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "err",
                    "alert('❌ Vui lòng nhập đầy đủ thông tin nhân viên!');", true);
                return;
            }

            string hoten = txtHoTen.Text.Trim();
            string ngaysinh = txtNgaySinh.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string cccd = txtCCCD.Text.Trim();
            string diachi = txtDiaChi.Text.Trim();
            string imgPath = "";

            // ============================
            // VALIDATE SĐT
            // ============================

            if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, @"^\d{10}$"))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "sdt",
                    "alert('❌ Số điện thoại phải gồm đúng 10 chữ số!');", true);
                return;
            }

            // ============================
            // VALIDATE CCCD
            // ============================
        
            if (!System.Text.RegularExpressions.Regex.IsMatch(cccd, @"^\d{12}$"))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "cccd",
                    "alert('❌ CCCD phải gồm đúng 12 chữ số!');", true);
                return;
            }

            // ============================
            // VALIDATE TUỔI >= 18
            // ============================
            DateTime ngaySinh;
            if (!DateTime.TryParse(txtNgaySinh.Text, out ngaySinh))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ngay",
                    "alert('❌ Ngày sinh không hợp lệ!');", true);
                return;
            }

            int tuoi = DateTime.Now.Year - ngaySinh.Year;
            if (ngaySinh > DateTime.Now.AddYears(-tuoi)) tuoi--;

            if (tuoi < 18)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "tuoi",
                    "alert('❌ Nhân viên phải đủ 18 tuổi trở lên!');", true);
                return;
            }

                string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

                // Tạo thư mục Images nếu chưa có
                string folderPath = Server.MapPath("~/Images/");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // Lưu ảnh nếu có
                if (fileAvatar.HasFile)
                {
                    string filename = Path.GetFileName(fileAvatar.FileName);
                    imgPath = "~/Images/" + filename;
                    fileAvatar.SaveAs(Path.Combine(folderPath, filename));
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // 1️⃣ INSERT nhân viên
                    string sqlNV = @"INSERT INTO Nhan_vien(HoTen, NgaySinh, SDT, CCCD, DiaChi, HinhAnh,TrangThai)
                         VALUES(@ten, @ngay, @sdt, @cccd, @diachi, @img,1);
                         SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdNV = new SqlCommand(sqlNV, conn);
                    cmdNV.Parameters.AddWithValue("@ten", hoten);
                    cmdNV.Parameters.AddWithValue("@ngay", ngaysinh);
                    cmdNV.Parameters.AddWithValue("@sdt", sdt);
                    cmdNV.Parameters.AddWithValue("@cccd", cccd);
                    cmdNV.Parameters.AddWithValue("@diachi", diachi);
                    cmdNV.Parameters.AddWithValue("@img", imgPath);

                    // 👉 Lấy mã nhân viên sau khi insert
                    int maNV = Convert.ToInt32(cmdNV.ExecuteScalar());

                    // 2️⃣ Tạo tài khoản User mặc định
                    string username = sdt;      // có thể dùng email hoặc CCCD
                    string password = "123";    // mật khẩu mặc định
                    int role = 10;               // User

                    string sqlUser = @"INSERT INTO [User](Username, Password, MaNV, IDROLE, IsActive)
                           VALUES(@u, @p, @manv, @role, 1)";

                    SqlCommand cmdUser = new SqlCommand(sqlUser, conn);
                    cmdUser.Parameters.AddWithValue("@u", username);
                    cmdUser.Parameters.AddWithValue("@p", password);
                    cmdUser.Parameters.AddWithValue("@manv", maNV);
                    cmdUser.Parameters.AddWithValue("@role", role);
                   

                cmdUser.ExecuteNonQuery();

                    conn.Close();
                }

                LoadDataQuanLyUser();

                ScriptManager.RegisterStartupScript(this, GetType(), "ok",
                    "alert('✔ Thêm nhân viên + tạo tài khoản User thành công!');", true);
            


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
            string ngaysinh = ((TextBox)row.Cells[2].Controls[0]).Text;
            string sdt = ((TextBox)row.Cells[3].Controls[0]).Text;
            string cccd = ((TextBox)row.Cells[4].Controls[0]).Text;
            string diachi = ((TextBox)row.Cells[5].Controls[0]).Text;

            // ============================
            // PHÒNG BAN
            // ============================
            string idpb = null;
            DropDownList ddlPB = row.FindControl("ddlPB_Grid") as DropDownList;
            if (ddlPB != null)
                idpb = ddlPB.SelectedValue;

            // ============================
            // XỬ LÝ ẢNH
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
                string sql = @"
        UPDATE Nhan_vien
        SET HoTen=@ten,
            NgaySinh=@ngay,
            SDT=@sdt,
            CCCD=@cccd,
            DiaChi=@diachi,
            IDPB=@idpb"
                    + (imgPath != null ? ", HinhAnh=@img" : "") +
                @" WHERE MaNV=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", hoten);
                cmd.Parameters.AddWithValue("@ngay", ngaysinh);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.Parameters.AddWithValue("@cccd", cccd);
                cmd.Parameters.AddWithValue("@diachi", diachi);
                cmd.Parameters.AddWithValue("@id", manv);
                cmd.Parameters.AddWithValue("@idpb", (object)idpb ?? DBNull.Value);

                if (imgPath != null)
                    cmd.Parameters.AddWithValue("@img", imgPath);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvQuanLyUser.EditIndex = -1;
            LoadDataQuanLyUser();
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
                // =========================
                // PHÂN QUYỀN + TRẠNG THÁI
                // =========================
                int trangThai = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TrangThai"));
                bool dangXemNghi = chkNhanVienNghi.Checked;

                var btnEdit = e.Row.FindControl("LinkButtonEdit") as LinkButton;
                var btnDelete = e.Row.FindControl("LinkButtonDelete") as LinkButton;
                var btnRestore = e.Row.FindControl("LinkButtonRestore") as LinkButton;

                if (dangXemNghi && trangThai == 0)
                {
                    if (btnEdit != null) btnEdit.Visible = false;
                    if (btnDelete != null) btnDelete.Visible = false;
                    if (btnRestore != null) btnRestore.Visible = true;
                    e.Row.ForeColor = System.Drawing.Color.Gray;
                }
                else
                {
                    if (btnRestore != null) btnRestore.Visible = false;
                }

                // =========================
                // 🔥 BIND DROPDOWN PHÒNG BAN
                // =========================
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlPB = e.Row.FindControl("ddlPB_Grid") as DropDownList;
                    if (ddlPB != null)
                    {
                        using (SqlConnection conn = new SqlConnection(connStr))
                        {
                            SqlDataAdapter da = new SqlDataAdapter(
                                "SELECT IDPB, TenPB FROM Phong_ban", conn);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            ddlPB.DataSource = dt;
                            ddlPB.DataTextField = "TenPB";
                            ddlPB.DataValueField = "IDPB";
                            ddlPB.DataBind();
                        }

                        // chọn sẵn phòng ban hiện tại
                        object idpb = DataBinder.Eval(e.Row.DataItem, "IDPB");
                        if (idpb != DBNull.Value)
                            ddlPB.SelectedValue = idpb.ToString();
                    }
                }
            }
        }



    }
}
