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

   
     

        private void LoadDataQuanLyHD()
    {
     
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(@"
    SELECT 
        SoHD,
        MaNV,
        NgayBatDau,
        NgayKetThuc,
        NgayKi,
        NoiDung,
        LanKy,
        ThoiHan,
        LuongCoBan
    FROM Hop_dong
", conn);

                DataTable dt = new DataTable();
            da.Fill(dt);

            gvQuanLyHD.DataSource = dt;
            gvQuanLyHD.DataBind();
        }
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
            string luong = ((TextBox)row.FindControl("txtGV_LuongCoBan")).Text.Trim();

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
                       LuongCoBan=@luong
                       WHERE SoHD=@soHD";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@manv", manv);
                cmd.Parameters.AddWithValue("@nbd", nbd);
                cmd.Parameters.AddWithValue("@nkt", nkt);
                cmd.Parameters.AddWithValue("@nki", nki);
                cmd.Parameters.AddWithValue("@noidung", noidung);
                cmd.Parameters.AddWithValue("@lanky", lanky);
                cmd.Parameters.AddWithValue("@thoihan", thoihan);
                cmd.Parameters.AddWithValue("@luong", luong);
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
        private void AddPara(Document doc, string text, Font font, int align = Element.ALIGN_LEFT)
        {
            Paragraph p = new Paragraph(text, font);
            p.Alignment = align;
            p.SpacingAfter = 5f;
            doc.Add(p);
        }

        private void ExportHopDongPDF(string soHD)
        {
            string hoTen = "", maNV = "";
            string luongCoBan = "";
            string chucVu = "", boPhan = "";
            string diaChi = "", cccd = "";

            DateTime ngaySinh = DateTime.MinValue;

            DateTime ngayBatDau = DateTime.MinValue;
            DateTime ngayKetThuc = DateTime.MinValue;
            DateTime ngayKy = DateTime.MinValue;

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString))
            {
                string sql = @"
     SELECT 
    hd.SoHD,
    hd.NgayBatDau,
    hd.NgayKetThuc,
    hd.NgayKi,
    hd.NoiDung,
    hd.LuongCoBan,
    nv.HoTen,
    nv.MaNV,
    nv.NgaySinh,
    nv.DiaChi,
    nv.CCCD,

    cv.TenCV  AS ChucVu,
    bp.TenBP  AS BoPhan

FROM Hop_dong hd
JOIN Nhan_vien nv ON hd.MaNV = nv.MaNV
LEFT JOIN Chuc_vu cv ON nv.IDCV = cv.IDCV
LEFT JOIN Bo_phan bp ON nv.IDBP = bp.IDBP

WHERE hd.SoHD = @soHD

";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@soHD", soHD);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    hoTen = dr["HoTen"].ToString();
                    maNV = dr["MaNV"].ToString();
                    diaChi = dr["DiaChi"].ToString();
                    cccd = dr["CCCD"].ToString();
                    luongCoBan = dr["LuongCoBan"].ToString();
                    chucVu = dr["ChucVu"].ToString();
                    boPhan = dr["BoPhan"].ToString();

                    if (dr["NgaySinh"] != DBNull.Value)
                        ngaySinh = Convert.ToDateTime(dr["NgaySinh"]);

                    if (dr["NgayBatDau"] != DBNull.Value)
                        ngayBatDau = Convert.ToDateTime(dr["NgayBatDau"]);

                    if (dr["NgayKetThuc"] != DBNull.Value)
                        ngayKetThuc = Convert.ToDateTime(dr["NgayKetThuc"]);

                    if (dr["NgayKi"] != DBNull.Value)
                        ngayKy = Convert.ToDateTime(dr["NgayKi"]);
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

            Font titleFont = new Font(bf, 14, Font.BOLD);
            Font boldFont = new Font(bf, 12, Font.BOLD);
            Font normalFont = new Font(bf, 12);

            // ===== QUỐC HIỆU =====
            AddPara(doc, "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM", boldFont, Element.ALIGN_CENTER);
            AddPara(doc, "Độc lập – Tự do – Hạnh phúc\n", boldFont, Element.ALIGN_CENTER);

            string ngayKyText =
      $"………, ngày {ngayKy:dd} tháng {ngayKy:MM} năm {ngayKy:yyyy}";

            AddPara(doc, ngayKyText, normalFont, Element.ALIGN_CENTER);

            // ===== TIÊU ĐỀ =====
            AddPara(doc, "HỢP ĐỒNG LAO ĐỘNG", titleFont, Element.ALIGN_CENTER);
            AddPara(doc, $"Số: {soHD}/HĐLĐ\n", normalFont, Element.ALIGN_CENTER);

            // ===== MỞ ĐẦU =====
            AddPara(doc,
  $@"Hôm nay, ngày {ngayKy:dd} tháng {ngayKy:MM} năm {ngayKy:yyyy},
tại ………………………………………",
  normalFont);

         

            // ===== BÊN A =====
            AddPara(doc, "\nBÊN A:", boldFont);
            AddPara(doc, "Tên đơn vị: …………………………………………………………………………………", normalFont);
            AddPara(doc, "Đại diện Ông/Bà: …………………………………………………………………………", normalFont);
            AddPara(doc, "Chức vụ: ……………………………………………………………………………………", normalFont);
            AddPara(doc, "Địa chỉ: ……………………………………………………………………………………", normalFont);
            AddPara(doc, "Điện thoại: …………………………………………………………………………………", normalFont);

            // ===== BÊN B =====
            AddPara(doc, "\nBÊN B:", boldFont);
            AddPara(doc, $"Ông/Bà: {hoTen}", normalFont);
            AddPara(doc, $"Mã nhân viên: {maNV}", normalFont);
            AddPara(doc, $"Ngày sinh: {ngaySinh:dd/MM/yyyy}", normalFont);
            AddPara(doc, "Quốc tịch: Việt Nam", normalFont);
            AddPara(doc, $"Chức vụ: {chucVu}", normalFont);
            AddPara(doc, $"Địa chỉ thường trú: {diaChi}", normalFont);
            AddPara(doc, $"Số CMTND/CCCD: {cccd}", normalFont);


            // ===== ĐIỀU 1 =====
            AddPara(doc, "\nĐiều 1: Điều khoản chung", boldFont);
            AddPara(doc, "Loại HĐLĐ: …………………………………………………………………………………", normalFont);
            AddPara(doc, $"Thời hạn HĐLĐ từ ngày {ngayBatDau:dd/MM/yyyy} đến ngày {ngayKetThuc:dd/MM/yyyy}", normalFont);
            AddPara(doc, "Địa điểm làm việc: …………………………………………………………………………", normalFont);
            AddPara(doc, $"Bộ phận: {boPhan}", normalFont);

            AddPara(doc,
            @"Nhiệm vụ công việc:
– Thực hiện công việc theo sự phân công của Ban Giám đốc.
– Phối hợp với các phòng ban để hoàn thành công việc.
– Thực hiện các nhiệm vụ khác theo yêu cầu của Công ty.", normalFont);

            // ===== ĐIỀU 2 =====
            AddPara(doc, "\nĐiều 2: Chế độ làm việc", boldFont);
            AddPara(doc,
            @"Thời gian làm việc:
– Sáng: 08h00 – 12h00
– Chiều: 13h30 – 17h30
– Nghỉ chiều Thứ 7 và Chủ nhật.", normalFont);

            // ===== ĐIỀU 3 =====
            AddPara(doc, "\nĐiều 3: Quyền lợi và nghĩa vụ của người lao động", boldFont);
            AddPara(doc,
            $@"Tiền lương:
- Mức lương cơ bản: {luongCoBan} 
– Hình thức trả lương: Lương thời gian
– Tham gia BHXH, BHYT, BHTN theo quy định.", normalFont);

            // ===== ĐIỀU 7 =====
            AddPara(doc, "\nĐiều 7: Điều khoản thi hành", boldFont);
            AddPara(doc,
            @"Hợp đồng này được lập thành 02 bản có giá trị như nhau,
mỗi bên giữ 01 bản và có hiệu lực kể từ ngày ký.", normalFont);

            // ===== KÝ TÊN =====
            AddPara(doc, "\n\nNGƯỜI LAO ĐỘNG", boldFont, Element.ALIGN_LEFT);
            AddPara(doc, "(Ký, ghi rõ họ tên)\n\n", normalFont);

            AddPara(doc, "NGƯỜI SỬ DỤNG LAO ĐỘNG", boldFont, Element.ALIGN_RIGHT);
            AddPara(doc, "(Ký, ghi rõ họ tên)", normalFont);

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

        protected void btnAddHD_Click(object sender, EventArgs e)
        {
            Response.Redirect("ThemHopDong.aspx");
        }
    }
}