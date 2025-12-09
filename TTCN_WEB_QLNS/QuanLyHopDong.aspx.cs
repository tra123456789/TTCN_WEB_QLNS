using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

namespace TTCN_WEB_QLNS
{
    public partial class QuanLyHopDong : System.Web.UI.Page
    {
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
            //    Response.Redirect("QuanLyHopDong.aspx");
            //    return;
            //}

            //// Hiển thị tên
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
                LoadDataQuanLyHD();
            }
        }
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

        void Loaddata(string maNV)
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Hop_dong WHERE MaNV = @ma";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ma", maNV);

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    // Load dữ liệu lên TextBox
                    txtNgayBatDau.Text = Convert.ToDateTime(r["NgayBatDau"]).ToString("yyyy-MM-dd");
                    txtNgayKetThuc.Text = Convert.ToDateTime(r["NgayKetThuc"]).ToString("yyyy-MM-dd");
                    txtNgayKi.Text = Convert.ToDateTime(r["NgayKi"]).ToString("yyyy-MM-dd");

                    txtNoiDung.Text = r["NoiDung"].ToString();
                    txtLanKy.Text = r["LanKy"].ToString();
                    txtThoiHan.Text = r["ThoiHan"].ToString();
                    txtHeSoLuong.Text = r["HeSoLuong"].ToString();
                    txtMaNV.Text = r["MaNV"].ToString();

                    // Đánh dấu đang sửa
                    ViewState["EditingMANV"] = maNV;
                }
            }

            //btnSave.Text = "💾 Cập nhật";
        }


        private void LoadDataQuanLyHD()
    {
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Hop_dong", conn);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvQuanLyHD.DataSource = dt;
            gvQuanLyHD.DataBind();
        }
    }
    protected void btnAddHD_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                
                string ngaybatdau = txtNgayBatDau.Text.Trim();
                string ngayketthuc = txtNgayKetThuc.Text.Trim();
                string ngayki = txtNgayKi.Text.Trim();
                string noidung = txtNoiDung.Text.Trim();
                string lanky = txtLanKy.Text.Trim();
                string thoihan = txtThoiHan.Text.Trim();
                string hesoluong = txtHeSoLuong.Text.Trim();
                string manv = txtMaNV.Text.Trim();


                // 🔥 Câu lệnh INSERT
                string sql = @"INSERT INTO Hop_dong(MaNV, NgayBatDau, NgayKetThuc, NgayKi, NoiDung, LanKy, ThoiHan, HeSoLuong)
                       VALUES(@manv,@ngaybatdau,@ngayketthuc,@ngayki, @noidung, @lanky, @thoihan, @hesoluong)";

                SqlCommand cmd = new SqlCommand(sql, conn);

               
                cmd.Parameters.AddWithValue("@ngaybatdau", ngaybatdau);
                cmd.Parameters.AddWithValue("@ngayketthuc", ngayketthuc);
                cmd.Parameters.AddWithValue("@ngayki", ngayki);
                cmd.Parameters.AddWithValue("@noidung", noidung);
                cmd.Parameters.AddWithValue("@lanky", lanky);
                cmd.Parameters.AddWithValue("@thoihan", thoihan);
                cmd.Parameters.AddWithValue("@hesoluong", hesoluong);
                cmd.Parameters.AddWithValue("@manv", manv);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            LoadDataQuanLyHD(); 

        }

        protected void txtNgayBatDau_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtNgayKetThuc_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtNgayKi_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtNoiDung_TextChanged(object sender, EventArgs e)
        {

        }


        protected void txtSoHD_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtLanKy_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtThoiHan_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtHeSoLuong_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtMaNV_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Hop_hong WHERE SoHD LIKE @search OR MaNV LIKE @search",
                    conn);

                da.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvQuanLyHD.DataSource = dt;
                gvQuanLyHD.DataBind();
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvQuanLyHD.PageSize = int.Parse(ddlPageSize.SelectedValue);
            LoadDataQuanLyHD();
        }

        protected void gvQuanLyHD_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvQuanLyHD_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvQuanLyHD.EditIndex = e.NewEditIndex;
            LoadDataQuanLyHD();
        }

        protected void gvQuanLyHD_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void gvQuanLyHD_SelectedIndexChanged(object sender, EventArgs e)
        {
             
        }

        protected void gvQuanLyHD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            gvQuanLyHD.PageIndex = e.NewPageIndex;
            LoadDataQuanLyHD();
        }

        protected void gvQuanLyHD_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvQuanLyHD.EditIndex = -1;
            LoadDataQuanLyHD();
        }
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }
    }
}