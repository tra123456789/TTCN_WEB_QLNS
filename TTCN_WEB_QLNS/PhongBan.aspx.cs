using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class PhanQuyen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //if (Session["UserName"] == null || Session["IDROLE"] == null)
            //{
            //    Response.Redirect("PhongBan.aspx");
            //    return;
            //}

            // Hiển thị tên
       
            //// Phân quyền
            //string role = Session["IDROLE"].ToString();

            //if (role == "User")
            //{
            //   menuTongQuan.Visible = false;
            //    menuNhanVien.Visible = false;
            //    menuPhongBan.Visible = false;
            //    menuHopDong.Visible = false;
            //    menuLuong.Visible = false;
            //    menuKhenThuong.Visible = false;
            //}

            if (!IsPostBack)
            {
                LoadDataPhongBan();
                //string role = Session["IDROLE"].ToString();
                //if (role == "1")
                //{
                //    //menuThongTinNV.Visible = false;
                //}
            }
        }
        void LoadPB(string IDPB)
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Phong_ban WHERE IDPB = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", IDPB);

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    txtTenPB.Text = r["TenPB"].ToString();
                    //txtNgaySinh.Text = Convert.ToDateTime(r["NgaySinh"]).ToString("yyyy-MM-dd");
                    txtSDT.Text = r["SDT"].ToString();
                    //txtCCCD.Text = r["CCCD"].ToString();
                    txtDiaChi.Text = r["DiaChi"].ToString();

                    // lưu để biết đang sửa
                    ViewState["EditingIDPB"] = IDPB;
                }
            }

            //btnSave.Text = "💾 Cập nhật";
        }
        private void LoadDataPhongBan()
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Phong_ban", conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPhongBan.DataSource = dt;
                gvPhongBan.DataBind();
            }
        }
    

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Phong_ban WHERE IDPB LIKE @search OR TenPB LIKE @search",
                    conn);

                da.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPhongBan.DataSource = dt;
                gvPhongBan.DataBind();
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPhongBan.PageSize = int.Parse(ddlPageSize.SelectedValue);
            LoadDataPhongBan();
        }

        protected void gvPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvPhongBan_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            
                string IDPB = gvPhongBan.DataKeys[e.RowIndex].Value.ToString();

                GridViewRow row = gvPhongBan.Rows[e.RowIndex];

                string tenpb = ((TextBox)row.FindControl("txtTenPB")).Text;
                string sdt = ((TextBox)row.FindControl("txtSDT")).Text;
                string diachi = ((TextBox)row.FindControl("txtDiaChi")).Text;

                string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = @"UPDATE Phong_ban
                       SET TenPB=@tenpb, SDT=@sdt, DiaChi=@diachi
                       WHERE IDPB=@id";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@tenpb", tenpb);
                    cmd.Parameters.AddWithValue("@sdt", sdt);
                    cmd.Parameters.AddWithValue("@diachi", diachi);
                    cmd.Parameters.AddWithValue("@id", IDPB);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                gvPhongBan.EditIndex = -1;
                LoadDataPhongBan();
            }

        

        protected void gvPhongBan_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPhongBan.EditIndex = e.NewEditIndex;
            LoadDataPhongBan();
        }

        protected void gvPhongBan_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string IDPB = gvPhongBan.DataKeys[e.RowIndex].Value.ToString();

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "DELETE FROM Phong_ban WHERE IDPB=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", IDPB);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadDataPhongBan();
        }

        protected void gvPhongBan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPhongBan.PageIndex = e.NewPageIndex;
            LoadDataPhongBan();
        }

        protected void gvPhongBan_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPhongBan.EditIndex = -1;
            LoadDataPhongBan();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string tenpb = txtTenPB.Text;
                string sdt = txtSDT.Text.Trim();
                string diachi = txtDiaChi.Text.Trim();


                //// 🔥 Tạo thư mục Images nếu chưa có
                //string folderPath = Server.MapPath("~/Images/");
                //if (!Directory.Exists(folderPath))
                //{
                //    Directory.CreateDirectory(folderPath);
                //}

                //// 🔥 Lưu file ảnh
                //if (fileAvatar.HasFile)
                //{
                //    string filename = Path.GetFileName(fileAvatar.FileName);
                //    imgPath = "~/Images/" + filename;

                //    fileAvatar.SaveAs(Path.Combine(folderPath, filename));
                //}

                // 🔥 Câu lệnh INSERT
                string sql = @"INSERT INTO Phong_ban(TenPB, SDT, DiaChi)
                       VALUES(@tenpb,@sdt, @diachi)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@tenpb", tenpb);
               
                cmd.Parameters.AddWithValue("@sdt", sdt);
            
                cmd.Parameters.AddWithValue("@diachi", diachi);
                

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            LoadDataPhongBan(); // load lại GridView sau khi thêm

        }

        protected void txtTenPB_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtSDT_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtDiaChi_TextChanged(object sender, EventArgs e)
        {

        }
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }
    }
}