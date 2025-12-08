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
                string sql = "SELECT * FROM Nhan_vien WHERE MANV = @ma";
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
                    ViewState["EditingMANV"] = maNV;
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
                    "SELECT * FROM Nhan_vien WHERE MANV LIKE @search OR HoTen LIKE @search",
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
           

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string hoten = txtHoTen.Text;
                string ngaysinh = txtNgaySinh.Text;
                string sdt = txtSDT.Text.Trim();
                string cccd = txtCCCD.Text.Trim();
                string diachi = txtDiaChi.Text.Trim();
                string imgPath = "";

                // 🔥 Tạo thư mục Images nếu chưa có
                string folderPath = Server.MapPath("~/Images/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 🔥 Lưu file ảnh
                if (fileAvatar.HasFile)
                {
                    string filename = Path.GetFileName(fileAvatar.FileName);
                    imgPath = "~/Images/" + filename;

                    fileAvatar.SaveAs(Path.Combine(folderPath, filename));
                }

                // 🔥 Câu lệnh INSERT
                string sql = @"INSERT INTO Nhan_vien(HoTen,NgaySinh, SDT, CCCD, DiaChi, HinhAnh)
                       VALUES(@ten,@ngay, @sdt, @cccd, @diachi, @img)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ten", hoten);
                cmd.Parameters.AddWithValue("@ngay", ngaysinh);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.Parameters.AddWithValue("@cccd", cccd);
                cmd.Parameters.AddWithValue("@diachi", diachi);
                cmd.Parameters.AddWithValue("@img", imgPath);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            LoadDataQuanLyUser(); // load lại GridView sau khi thêm

        
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
                    string sql = "DELETE FROM Nhan_vien WHERE MANV = @ma";
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
                string sql = "DELETE FROM Nhan_vien WHERE MANV=@id";

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
                               WHERE MANV=@id";

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
    }
}
