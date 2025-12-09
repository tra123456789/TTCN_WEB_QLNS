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
                LoadDataBaoHiem();
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

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"
            SELECT 
                bh.SoBH,
                bh.TuThang,
                bh.DenThang,
                bh.DonVi,
                cv.TenCV AS Chucvu,
                nv.MaNV
            FROM Bao_hiem bh
            JOIN Nhan_vien nv ON bh.MaNV = nv.MaNV
            JOIN Chuc_vu cv ON nv.IDCV = cv.IDCV";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

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

        protected void txtSoBaoHiem_TextChanged(object sender, EventArgs e)
        {

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
                string tuthang = txtTuThang.Text.Trim();
                string denthang = txtDenThang.Text.Trim();
                string donvi = txtDonVi.Text.Trim();
        

                // 🔥 Câu lệnh INSERT
                string sql = @"INSERT INTO Bao_hiem(MaNV, TuThang, DenThang, DonVi)
                       VALUES(@manv,@tuthang,@denthang,@donvi)";
                
                SqlCommand cmd = new SqlCommand(sql, conn);


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
    }
}