using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                lblWelcome.Text = "Xin chào, " + Session["UserName"].ToString();
            }
            else
            {
                Response.Redirect("DangNhap.aspx"); // nếu chưa đăng nhập → quay lại login
            }
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
                LoadDataQuanLyUser();
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



        private void LoadDataQuanLyUser()
        {
        
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Nhan_vien", conn);

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

                   string hoten = txtHoTen.Text.Trim();
                string ngaysinh = txtNgaySinh.Text.Trim();
                string sdt = txtSDT.Text.Trim();
                string cccd = txtCCCD.Text.Trim();
                string diachi = txtDiaChi.Text.Trim();
                string imgPath = "";

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
                    string sqlNV = @"INSERT INTO Nhan_vien(HoTen, NgaySinh, SDT, CCCD, DiaChi, HinhAnh)
                         VALUES(@ten, @ngay, @sdt, @cccd, @diachi, @img);
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

                    string sqlUser = @"INSERT INTO [User](Username, Password, MaNV, IDROLE)
                           VALUES(@u, @p, @manv, @role)";

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
            string maNV = e.CommandArgument.ToString();

            // ❌ XÓA
            if (e.CommandName == "DeleteRow")
            {
               

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = "DELETE FROM Nhan_vien WHERE MaNV = @ma";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", maNV);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadDataQuanLyUser();
            }

            // ✏ SỬA
            if (e.CommandName == "EditRow")
            {
                LoadSingleUser(maNV);
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
                string sql = "DELETE FROM Nhan_vien WHERE MaNV=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", manv);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadDataQuanLyUser();
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

         
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"UPDATE Nhan_vien 
                               SET HoTen=@ten, NgaySinh=@ngay, SDT=@sdt, 
                                   CCCD=@cccd, DiaChi=@diachi 
                               WHERE MaNV=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", hoten);
                cmd.Parameters.AddWithValue("@ngay", ngaysinh);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.Parameters.AddWithValue("@cccd", cccd);
                cmd.Parameters.AddWithValue("@diachi", diachi);
                cmd.Parameters.AddWithValue("@id", manv);

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
    }
}
