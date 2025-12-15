using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class Quan_Ly_Nhan_Vien : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserName"] != null)
            //{
            //    lblWelcome.Text = "Xin chào, " + Session["UserName"].ToString();
            //}
            //else
            //{
            //    Response.Redirect("DangNhap.aspx"); // nếu chưa đăng nhập → quay lại login
            //}
            if (Session["UserName"] == null || Session["IDROLE"] == null || Session["MaNV"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }
            lblWelcome.Text = "Xin chào: " + Session["UserName"].ToString();

            if (!IsPostBack)
            {
                LoadDataBaoHiem();


                string role = Session["IDROLE"].ToString();

                // Nếu là USER bắt buộc có MaNV
                if (role == "10" && Session["MaNV"] == null)
                {
                    Response.Redirect("DangNhap.aspx");
                    return;
                }


                if (role == "10")
                {
                    menuThongTinNV.Visible = true;
                    menuTongQuan.Visible = false;
                    menuNhanVien.Visible = false;
                    menuPhongBan.Visible = false;
                    menuHopDong.Visible = false;
                    menuLuong.Visible = true;
                    menuBaoHiem.Visible = true;
                    menuChamCong.Visible = true;
                    menuKhenThuong.Visible = false;
                    gvBaoHiem.Columns[gvBaoHiem.Columns.Count - 1].Visible = false;
                }
                else
                {
                    menuThongTinNV.Visible = false;
                }
            
            }
                //if (Session["UserName"] == null || Session["IDROLE"] == null)
                //{
                //    Response.Redirect("BaoHiem.aspx");
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
            }
        private void LoadDataBaoHiem()
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;
            string role = Session["IDROLE"]?.ToString() ?? "";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                conn.Open();

                if (role == "1" || role == "12")
                {
                    // Admin: xem tất cả
                    cmd.CommandText = @"
                SELECT bh.IDBH, bh.SoBH, bh.TuThang, bh.DenThang, bh.DonVi,
                       nv.MaNV, nv.HoTen, cv.TenCV AS ChucVu
                FROM Bao_hiem bh
                JOIN Nhan_vien nv ON bh.MaNV = nv.MaNV
                LEFT JOIN Chuc_vu cv ON nv.IDCV = cv.IDCV
                ORDER BY bh.IDBH DESC";
                }
                else
                {
                    // User: chỉ xem bản ghi của chính họ
                    cmd.CommandText = @"
                SELECT bh.IDBH, bh.SoBH, bh.TuThang, bh.DenThang, bh.DonVi,
                       nv.MaNV, nv.HoTen, cv.TenCV AS ChucVu
                FROM Bao_hiem bh
                JOIN Nhan_vien nv ON bh.MaNV = nv.MaNV
                LEFT JOIN Chuc_vu cv ON nv.IDCV = cv.IDCV
                WHERE bh.MaNV = @MaNV
                ORDER BY bh.IDBH DESC";
                    cmd.Parameters.Add("@MaNV", SqlDbType.NVarChar, 50).Value = Session["MaNV"].ToString();
                }

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                gvBaoHiem.DataSource = dt;
                gvBaoHiem.DataBind();
            }
        }


        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
         
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Bao_hiem WHERE MaNV LIKE @search OR SoBH LIKE @search",
                    conn);

                da.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvBaoHiem.DataSource = dt;
                gvBaoHiem.DataBind();
            }
        }

        protected void gvBaoHiem_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void gvBaoHiem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            gvBaoHiem.PageIndex = e.NewPageIndex;
            LoadDataBaoHiem();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {

            gvBaoHiem.PageSize = int.Parse(ddlPageSize.SelectedValue);
            LoadDataBaoHiem();
        }
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }

      

        protected void txtTuThang_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtDenThang_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtNoiDung_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtLanKy_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtMaNV_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtChucVu_TextChanged(object sender, EventArgs e)
        {

        }
        protected void btnAddBH_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                string manv = txtMaNV.Text.Trim();
                string soBH = txtSoBH.Text.Trim();
                string tuthang = txtTuThang.Text.Trim();
                string denthang = txtDenThang.Text.Trim();
                string donvi = txtDonVi.Text.Trim();
        

                // 🔥 Câu lệnh INSERT
                string sql = @"INSERT INTO Bao_hiem(MaNV,SoBH, TuThang, DenThang, DonVi)
                       VALUES(@manv,@sbh,@tuthang,@denthang,@donvi)";
                
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@sbh", soBH);
                cmd.Parameters.AddWithValue("@tuthang",tuthang );
                cmd.Parameters.AddWithValue("@denthang", denthang);
                cmd.Parameters.AddWithValue("@donvi", donvi );
               
                cmd.Parameters.AddWithValue("@manv", manv);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            LoadDataBaoHiem();
        }

        protected void gvBaoHiem_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvBaoHiem.EditIndex = e.NewEditIndex;
            LoadDataBaoHiem();
        }

        protected void gvBaoHiem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string IDBH = gvBaoHiem.DataKeys[e.RowIndex].Value.ToString();

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "DELETE FROM Bao_hiem WHERE IDBH=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", IDBH);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadDataBaoHiem();
        }

        protected void gvBaoHiem_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvBaoHiem_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvBaoHiem.Rows[e.RowIndex];
            string idbh = gvBaoHiem.DataKeys[e.RowIndex].Value.ToString();

            string manv = ((TextBox)row.FindControl("txtMaNV")).Text;
            string sbh = ((TextBox)row.FindControl("txtSoBH")).Text;
            string tuthang = ((TextBox)row.FindControl("txtTuThang")).Text;
            string denthang = ((TextBox)row.FindControl("txtDenThang")).Text;
            string donvi = ((TextBox)row.FindControl("txtDonVi")).Text;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"UPDATE Bao_hiem 
                       SET MaNV=@manv, SoBH=@sbh, TuThang=@tuthang, DenThang=@denthang, DonVi=@donvi
                       WHERE IDBH=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@manv", manv);
                cmd.Parameters.AddWithValue("@sbh", sbh);
                cmd.Parameters.AddWithValue("@tuthang", tuthang);
                cmd.Parameters.AddWithValue("@denthang", denthang);
                cmd.Parameters.AddWithValue("@donvi", donvi);
                cmd.Parameters.AddWithValue("@id", idbh);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvBaoHiem.EditIndex = -1; // thoát chế độ edit
            LoadDataBaoHiem();        // load lại dữ liệu
        }

        protected void gvBaoHiem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvBaoHiem.EditIndex = -1;
            LoadDataBaoHiem();
        }

        protected void txtSoBH_TextChanged(object sender, EventArgs e)
        {

        }
    }
}