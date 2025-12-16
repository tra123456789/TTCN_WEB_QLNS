using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;



namespace TTCN_WEB_QLNS
{
    public partial class QuanLyHopDong : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
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
            int soHD = Convert.ToInt32(gvQuanLyHD.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Hop_dong WHERE SoHD = @soHD", conn);
                cmd.Parameters.AddWithValue("@soHD", soHD);
                cmd.ExecuteNonQuery();
            }

            LoadDataQuanLyHD();
        }


        protected void gvQuanLyHD_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvQuanLyHD.EditIndex = e.NewEditIndex;
            LoadDataQuanLyHD();
        }

        protected void gvQuanLyHD_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int soHD = Convert.ToInt32(gvQuanLyHD.DataKeys[e.RowIndex].Value);

            GridViewRow row = gvQuanLyHD.Rows[e.RowIndex];

            string manv = ((TextBox)row.FindControl("txtGV_MaNV")).Text.Trim();
            string nbd = ((TextBox)row.FindControl("txtGV_NgayBatDau")).Text.Trim();
            string nkt = ((TextBox)row.FindControl("txtGV_NgayKetThuc")).Text.Trim();
            string nki = ((TextBox)row.FindControl("txtGV_NgayKi")).Text.Trim();
            string noidung = ((TextBox)row.FindControl("txtGV_NoiDung")).Text.Trim();
            string lanky = ((TextBox)row.FindControl("txtGV_LanKy")).Text.Trim();
            string thoihan = ((TextBox)row.FindControl("txtGV_ThoiHan")).Text.Trim();
            string hesoluong = ((TextBox)row.FindControl("txtGV_HeSoLuong")).Text.Trim();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"UPDATE Hop_dong SET
                        MaNV=@manv,
                        NgayBatDau=@nbd,
                        NgayKetThuc=@nkt,
                        NgayKi=@nki,
                        NoiDung=@noidung,
                        LanKy=@lanky,
                        ThoiHan=@thoihan,
                        HeSoLuong=@hesoluong
                       WHERE SoHD=@soHD";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@manv", manv);
                cmd.Parameters.AddWithValue("@nbd", nbd);
                cmd.Parameters.AddWithValue("@nkt", nkt);
                cmd.Parameters.AddWithValue("@nki", nki);
                cmd.Parameters.AddWithValue("@noidung", noidung);
                cmd.Parameters.AddWithValue("@lanky", lanky);
                cmd.Parameters.AddWithValue("@thoihan", thoihan);
                cmd.Parameters.AddWithValue("@hesoluong", hesoluong);
                cmd.Parameters.AddWithValue("@soHD", soHD);

                cmd.ExecuteNonQuery();
            }

            gvQuanLyHD.EditIndex = -1;
            LoadDataQuanLyHD();
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
        private void ExportHopDongPDF(string soHD)
        {
            string hoTen = "", maNV = "", noiDung = "", heSoLuong = "";
            DateTime ngayBatDau = DateTime.MinValue;
            DateTime ngayKetThuc = DateTime.MinValue;
            DateTime ngayKy = DateTime.MinValue;

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString))
            {
                string sql = @"
        SELECT hd.SoHD, hd.NgayBatDau, hd.NgayKetThuc, hd.NgayKi,
               hd.NoiDung, hd.HeSoLuong, nv.HoTen, nv.MaNV
        FROM Hop_dong hd
        JOIN Nhan_vien nv ON hd.MaNV = nv.MaNV
        WHERE hd.SoHD = @soHD";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@soHD", soHD);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    hoTen = dr["HoTen"].ToString();
                    maNV = dr["MaNV"].ToString();
                    noiDung = dr["NoiDung"].ToString();
                    heSoLuong = dr["HeSoLuong"].ToString();

                    if (dr["NgayBatDau"] != DBNull.Value)
                        ngayBatDau = Convert.ToDateTime(dr["NgayBatDau"]);
                    if (dr["NgayKetThuc"] != DBNull.Value)
                        ngayKetThuc = Convert.ToDateTime(dr["NgayKetThuc"]);
                    if (dr["NgayKi"] != DBNull.Value)
                        ngayKy = Convert.ToDateTime(dr["NgayKi"]);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "err",
                        "alert('Không tìm thấy hợp đồng');", true);
                    return;
                }
            }

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename=HopDong_{soHD}.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
            PdfWriter.GetInstance(doc, Response.OutputStream);
            doc.Open();

            string fontPath = Server.MapPath("~/fonts/TIMES.ttf");
            BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font titleFont = new Font(bf, 16, Font.BOLD);
            Font normalFont = new Font(bf, 12);

            Paragraph title = new Paragraph("HỢP ĐỒNG LAO ĐỘNG\n\n", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            doc.Add(title);

            doc.Add(new Paragraph($"Số hợp đồng: {soHD}", normalFont));
            doc.Add(new Paragraph($"Họ tên nhân viên: {hoTen}", normalFont));
            doc.Add(new Paragraph($"Mã nhân viên: {maNV}", normalFont));
            doc.Add(new Paragraph($"Ngày ký: {ngayKy:dd/MM/yyyy}", normalFont));
            doc.Add(new Paragraph($"Thời hạn: {ngayBatDau:dd/MM/yyyy} - {ngayKetThuc:dd/MM/yyyy}", normalFont));
            doc.Add(new Paragraph($"Hệ số lương: {heSoLuong}", normalFont));

            doc.Add(new Paragraph("\nNội dung hợp đồng:\n", normalFont));
            doc.Add(new Paragraph(noiDung, normalFont));

            doc.Add(new Paragraph("\n\nĐẠI DIỆN CÔNG TY", normalFont));
            doc.Add(new Paragraph("(Ký và ghi rõ họ tên)", normalFont));

            doc.Close();
            Response.End();
        }

        protected void gvQuanLyHD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ExportPDF")
            {
                string soHD = e.CommandArgument.ToString();
                ExportHopDongPDF(soHD);
            }
        }

    }
}