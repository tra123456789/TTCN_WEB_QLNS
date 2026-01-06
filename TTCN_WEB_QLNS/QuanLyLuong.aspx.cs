using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class QuanLyLuong : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["IDROLE"] == null || Session["MaNV"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                

                string role = Session["IDROLE"].ToString();

                // User chỉ được xem
                if (role == "10")
                {
                    btnTinhLuong.Visible = false;
                    btnChotLuong.Visible = false;
                
                    btnResetLuong.Visible = false;
                    ddlPhongBan.Visible = false;    
                }
                LoadThangNam();
                LoadBangLuong();
                LoadPhongBan();
            }

        }
        void TinhLuongThangChoTatCaNhanVien(int thang, int nam, int idPhongBan)
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    // 1️⃣ Không cho tính lại nếu đã có lương
                    SqlCommand checkCmd = new SqlCommand(@"
                SELECT COUNT(*) 
                FROM BangLuongThang
                WHERE Thang = @Thang 
  AND Nam = @Nam
  AND IDPB = @IDPB

            ", conn, tran);

                    checkCmd.Parameters.AddWithValue("@Thang", thang);
                    checkCmd.Parameters.AddWithValue("@Nam", nam);
                    checkCmd.Parameters.AddWithValue("@IDPB", idPhongBan);

                    if ((int)checkCmd.ExecuteScalar() > 0)
                    {
                        tran.Rollback();
                        return;
                    }

                    // 2️⃣ SQL INSERT LƯƠNG (Đã sửa để lấy từ Hợp đồng mới nhất)
                    string sqlInsertLuong = @"
WITH LatestHD AS (
    SELECT 
        MaNV, LuongCoBan,
        ROW_NUMBER() OVER (PARTITION BY MaNV ORDER BY NgayBatDau DESC) as rn
    FROM Hop_dong
)
INSERT INTO BangLuongThang (
    MaNV, Thang, Nam,
    LuongCoBan, TongNgayCong,
    TongThuong, TongPhat,
    TongPhuCap, -- 1. Thêm tên cột vào đây
    BHXH, BHYT, BHTN,
    ThucLanh, TrangThai, IDPB
)
SELECT
    nv.MaNV,
    @Thang,
    @Nam,
    lhd.LuongCoBan,
    ISNULL(bc.TongCong, 0),
    ISNULL(kt.TongThuong, 0),
    ISNULL(kt.TongPhat, 0),
    ISNULL(pc.TongPhuCap, 0), -- 2. Thêm giá trị SUM từ bảng phụ cấp vào đây
    
    CASE WHEN ISNULL(bc.TongCong, 0) >= 14 THEN lhd.LuongCoBan * 0.08 ELSE 0 END,
    CASE WHEN ISNULL(bc.TongCong, 0) >= 14 THEN lhd.LuongCoBan * 0.015 ELSE 0 END,
    CASE WHEN ISNULL(bc.TongCong, 0) >= 14 THEN lhd.LuongCoBan * 0.01 ELSE 0 END,
  
    (
        ((lhd.LuongCoBan / 26) * ISNULL(bc.TongCong, 0)) 
        + ISNULL(kt.TongThuong, 0)                       
        + ISNULL(pc.TongPhuCap, 0)                       
    ) - (
        ISNULL(kt.TongPhat, 0)                          
        + (CASE WHEN ISNULL(bc.TongCong, 0) >= 14 THEN lhd.LuongCoBan * 0.105 ELSE 0 END)
    ) AS ThucLanh,
    0,
    pb.IDPB
FROM Nhan_vien nv
JOIN Bo_phan bp ON nv.IDBP = bp.IDBP
JOIN Phong_ban pb ON bp.IDPB = pb.IDPB
JOIN LatestHD lhd ON nv.MaNV = lhd.MaNV AND lhd.rn = 1 
LEFT JOIN BangCongThang bc ON bc.MaNV = nv.MaNV AND bc.Thang = @Thang AND bc.Nam = @Nam
LEFT JOIN (
    SELECT MaNV,
           SUM(CASE WHEN Loai = 1 THEN SoKTKL ELSE 0 END) AS TongThuong,
           SUM(CASE WHEN Loai = 2 THEN SoKTKL ELSE 0 END) AS TongPhat
    FROM KhenThuong_KyLuat
    WHERE MONTH(Ngay) = @Thang AND YEAR(Ngay) = @Nam
    GROUP BY MaNV
) kt ON nv.MaNV = kt.MaNV
LEFT JOIN (
    SELECT nvpc.MaNV, SUM(nvpc.SoTien) AS TongPhuCap -- Đã sửa: SUM trực tiếp từ bảng liên kết
    FROM NhanVien_PhuCap nvpc
    GROUP BY nvpc.MaNV
) pc ON nv.MaNV = pc.MaNV
WHERE (@IDPB = 0 OR pb.IDPB = @IDPB);";


                    SqlCommand cmd = new SqlCommand(sqlInsertLuong, conn, tran);
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);
                    cmd.Parameters.AddWithValue("@IDPB", idPhongBan);

                    cmd.ExecuteNonQuery();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }



        void LoadPhongBan()
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT IDPB, TenPB FROM Phong_ban";
                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                ddlPhongBan.DataSource = cmd.ExecuteReader();
                ddlPhongBan.DataTextField = "TenPB";
                ddlPhongBan.DataValueField = "IDPB";
                ddlPhongBan.DataBind();
            }

            ddlPhongBan.Items.Insert(0, new ListItem("-- Tất cả phòng ban --", "0"));
        }

    private void LoadBangLuong()
{
    if (ddlThang.SelectedValue == "" || ddlNam.SelectedValue == "") return;

    int thang = int.Parse(ddlThang.SelectedValue);
    int nam = int.Parse(ddlNam.SelectedValue);
    string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

    using (SqlConnection conn = new SqlConnection(connStr))
    {
        string role = Session["IDROLE"].ToString();
        
        // Lấy trực tiếp các cột từ bảng BangLuongThang (bl)
        // Không tính toán lại SUM ở đây để đảm bảo tốc độ và khớp dữ liệu đã chốt
        string selectFields = @"
            bl.MaNV,
            nv.HoTen,
            pb.TenPB,
            bl.LuongCoBan,
            bl.TongNgayCong,
            bl.TongThuong,
            bl.TongPhat,
            bl.TongPhuCap, 
            bl.BHXH,
            bl.BHYT,
            bl.BHTN,
            bl.ThucLanh,
            bl.TrangThai";

        string sql = "";

        if (role == "1" || role == "12") // Admin hoặc Kế toán
        {
            sql = $@"
                SELECT {selectFields}
                FROM BangLuongThang bl
                JOIN Nhan_vien nv ON bl.MaNV = nv.MaNV
                LEFT JOIN Bo_phan bp ON nv.IDBP = bp.IDBP
                LEFT JOIN Phong_ban pb ON bp.IDPB = pb.IDPB
                WHERE bl.Thang = @Thang AND bl.Nam = @Nam
                ORDER BY pb.TenPB, nv.HoTen";
        }
        else // Nhân viên thường
        {
            sql = $@"
                SELECT {selectFields}
                FROM BangLuongThang bl
                JOIN Nhan_vien nv ON bl.MaNV = nv.MaNV
                LEFT JOIN Bo_phan bp ON nv.IDBP = bp.IDBP
                LEFT JOIN Phong_ban pb ON bp.IDPB = pb.IDPB
                WHERE bl.Thang = @Thang AND bl.Nam = @Nam AND bl.MaNV = @MaNV";
        }

        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Thang", thang);
        cmd.Parameters.AddWithValue("@Nam", nam);

        if (role != "1" && role != "12")
        {
            cmd.Parameters.AddWithValue("@MaNV", Session["MaNV"].ToString());
        }

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);

        gvLuong.DataSource = dt;
        gvLuong.DataBind();
    }
}
        protected void btnChotLuong_Click(object sender, EventArgs e)
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);
            int idPhongBan = int.Parse(ddlPhongBan.SelectedValue);

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
            UPDATE BangLuongThang
            SET TrangThai = 1
           WHERE Thang = @Thang 
  AND Nam = @Nam
  AND IDPB = @IDPB
";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);
                cmd.Parameters.AddWithValue("@IDPB", idPhongBan);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ScriptManager.RegisterStartupScript(
                this, GetType(), "ok",
                "alert('Đã chốt lương tháng thành công!');", true);

            LoadBangLuong();
        }

        protected void btnTinhLuong_Click(object sender, EventArgs e)
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);
            int idPhongBan = int.Parse(ddlPhongBan.SelectedValue);

            // ❌ Không cho tính lại nếu đã chốt
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString))
            {
                conn.Open();
                string checkSql = @"
    SELECT COUNT(*) 
    FROM BangLuongThang
    WHERE Thang = @Thang 
      AND Nam = @Nam 
      AND TrangThai = 1
      AND IDPB = @IDPB";

            
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@Thang", thang);
                checkCmd.Parameters.AddWithValue("@Nam", nam);
                checkCmd.Parameters.AddWithValue("@IDPB", idPhongBan);


                if ((int)checkCmd.ExecuteScalar() > 0)
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "lock",
                        "alert('Tháng này đã chốt lương, không thể tính lại!');",
                        true);
                    return;
                }
            }

            // ✅ TÍNH LƯƠNG THEO PHÒNG BAN
            TinhLuongThangChoTatCaNhanVien(thang, nam, idPhongBan);

          
            LoadBangLuong();
        }



        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }
        private void LoadThangNam()
        {
            ddlThang.Items.Clear();
            ddlNam.Items.Clear();

            // Tháng 1 → 12
            for (int i = 1; i <= 12; i++)
            {
                ddlThang.Items.Add(new ListItem("Tháng " + i, i.ToString()));
            }

            // Năm: hiện tại ± 5 năm
            int namHienTai = DateTime.Now.Year;
            for (int i = namHienTai - 5; i <= namHienTai + 1; i++)
            {
                ddlNam.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            ddlThang.SelectedValue = DateTime.Now.Month.ToString();
            ddlNam.SelectedValue = DateTime.Now.Year.ToString();
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);
            int idPhongBan = int.Parse(ddlPhongBan.SelectedValue);

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;
            string role = Session["IDROLE"].ToString();

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "";

                // Admin hoặc Kế toán: Lấy đầy đủ các cột phụ cấp
                if (role == "1" || role == "12")
                {
                    sql = @"
            SELECT 
                bl.MaNV AS N'Mã NV',
                nv.HoTen AS N'Họ tên',
                pb.TenPB AS N'Phòng ban',
                bl.LuongCoBan AS N'Lương cơ bản',
                -- Lấy từng loại phụ cấp từ bảng NhanVien_PhuCap
                ISNULL((SELECT SoTien FROM NhanVien_PhuCap WHERE MaNV = bl.MaNV AND IDPC = 1), 0) AS N'PC Trách nhiệm',
                ISNULL((SELECT SoTien FROM NhanVien_PhuCap WHERE MaNV = bl.MaNV AND IDPC = 2), 0) AS N'PC Độc hại',
                ISNULL((SELECT SoTien FROM NhanVien_PhuCap WHERE MaNV = bl.MaNV AND IDPC = 3), 0) AS N'PC Khác',
                bl.TongNgayCong AS N'Ngày công',
                bl.TongThuong AS N'Thưởng',
                bl.TongPhat AS N'Phạt',
                bl.BHXH AS N'BHXH',
                bl.BHYT AS N'BHYT',
                bl.BHTN AS N'BHTN',
                bl.ThucLanh AS N'Thực lãnh'
            FROM BangLuongThang bl
            JOIN Nhan_vien nv ON bl.MaNV = nv.MaNV
            JOIN Bo_phan bp ON nv.IDBP = bp.IDBP
            JOIN Phong_ban pb ON bp.IDPB = pb.IDPB
            WHERE bl.Thang = @Thang 
              AND bl.Nam = @Nam
              AND (@IDPB = 0 OR pb.IDPB = @IDPB)
            ORDER BY pb.TenPB, nv.HoTen";
                }
                else // User thường: Chỉ lấy tổng phụ cấp để bảo mật hoặc theo yêu cầu
                {
                    sql = @"
            SELECT 
                bl.MaNV AS N'Mã NV',
                nv.HoTen AS N'Họ tên',
                pb.TenPB AS N'Phòng ban',
                bl.LuongCoBan AS N'Lương cơ bản',
                (SELECT ISNULL(SUM(SoTien), 0) FROM NhanVien_PhuCap WHERE MaNV = bl.MaNV) AS N'Tổng phụ cấp',
                bl.TongNgayCong AS N'Ngày công',
                bl.ThucLanh AS N'Thực lãnh'
            FROM BangLuongThang bl
            JOIN Nhan_vien nv ON bl.MaNV = nv.MaNV
            JOIN Bo_phan bp ON nv.IDBP = bp.IDBP
            JOIN Phong_ban pb ON bp.IDPB = pb.IDPB
            WHERE bl.Thang = @Thang 
              AND bl.Nam = @Nam 
              AND bl.MaNV = @MaNV";
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);
                cmd.Parameters.AddWithValue("@IDPB", idPhongBan);

                if (role != "1" && role != "12")
                {
                    cmd.Parameters.AddWithValue("@MaNV", Session["MaNV"].ToString());
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "nodata", "alert('Không có dữ liệu để xuất!');", true);
                return;
            }

            string tenPB = ddlPhongBan.SelectedItem.Text.Replace("--", "").Trim();
            ExportToExcel(dt, thang, nam, tenPB);
        }
        private void ExportToExcel(DataTable dt, int thang, int nam, string tenFileCustom)
        {
            Response.Clear();
            Response.Buffer = true;
            string fileName = $"{tenFileCustom}.xls";

            Response.AddHeader("content-disposition", $"attachment;filename={fileName}");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    // 1. Tạo style CSS cho bảng (Kẻ bảng, màu sắc, font chữ)
                    sw.Write("<style>");
                    sw.Write(".header-title { font-size: 16pt; font-weight: bold; text-align: center; color: #2c3e50; }");
                    sw.Write(".company-name { font-size: 12pt; font-weight: bold; color: #c0392b; }");
                    sw.Write(".table-style { border-collapse: collapse; width: 100%; }");
                    sw.Write(".table-style th { background-color: #34495e; color: white; border: 1px solid #000; padding: 5px; }");
                    sw.Write(".table-style td { border: 1px solid #000; padding: 5px; text-align: left; }");
                    sw.Write(".footer-sign { margin-top: 20px; text-align: right; font-style: italic; }");
                    sw.Write("</style>");

                    // 2. Chèn thông tin Công ty & Tiêu đề
                    sw.Write("<table>");
                    sw.Write("<tr><td colspan='5' class='company-name'>CÔNG TY TNHH QUẢN LÝ NHÂN SỰ ABC</td></tr>");
                    sw.Write("<tr><td colspan='5' style='text-align:left;'>Địa chỉ: 123 Đường Láng, Đống Đa, Hà Nội</td></tr>");
                    sw.Write("<tr><td colspan='5' class='header-title'><br/>PHIẾU LƯƠNG NHÂN VIÊN<br/></td></tr>");
                    sw.Write($"<tr><td colspan='5' style='text-align:center;'>Tháng {thang} Năm {nam}</td></tr>");
                    sw.Write("<tr><td colspan='5'><br/></td></tr>");
                    sw.Write("</table>");

                    // 3. Render GridView vào Table
                    GridView gv = new GridView();
                    gv.DataSource = dt;
                    gv.DataBind();

                    // Áp dụng class CSS cho GridView
                    gv.Attributes.Add("class", "table-style");
                    gv.RenderControl(hw);

                    // 4. Chèn phần chữ ký
                    sw.Write("<br/>");
                    sw.Write("<table style='width: 100%;'>");
                    sw.Write("<tr>");
                    sw.Write("<td colspan='3'></td>");
                    sw.Write($"<td colspan='2' class='footer-sign'>Hà Nội, Ngày .... tháng {thang} năm {nam}<br/><b>Người lập biểu</b><br/><br/><br/><br/>(Ký và ghi rõ họ tên)</td>");
                    sw.Write("</tr>");
                    sw.Write("</table>");

                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
        }
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvLuong.PageSize = int.Parse(ddlPageSize.SelectedValue);
            LoadBangLuong();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string role = Session["IDROLE"].ToString();
                // Phải có bl.TongPhuCap ở đây
                string sql = @"
            SELECT bl.MaNV, nv.HoTen, pb.TenPB, bl.LuongCoBan, bl.TongNgayCong, 
                   bl.TongThuong, bl.TongPhat, bl.TongPhuCap, bl.BHXH, bl.BHYT, bl.BHTN, bl.ThucLanh
            FROM BangLuongThang bl
            JOIN Nhan_vien nv ON bl.MaNV = nv.MaNV
            LEFT JOIN Bo_phan bp ON nv.IDBP = bp.IDBP
            LEFT JOIN Phong_ban pb ON bp.IDPB = pb.IDPB
            WHERE bl.Thang = @Thang AND bl.Nam = @Nam 
            AND (nv.MaNV LIKE @search OR nv.HoTen LIKE @search)";

                if (role != "1" && role != "12")
                {
                    sql += " AND bl.MaNV = @MaNV";
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);
                cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");
                if (role != "1" && role != "12")
                    cmd.Parameters.AddWithValue("@MaNV", Session["MaNV"].ToString());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvLuong.DataSource = dt;
                gvLuong.DataBind();
            }
        }
        protected void btnResetLuong_Click(object sender, EventArgs e)
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // ❌ Không cho reset nếu đã chốt
                string checkSql = @"
        SELECT COUNT(*) 
        FROM BangLuongThang
        WHERE Thang = @Thang 
  AND Nam = @Nam 
  AND TrangThai = 1
";

                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@Thang", thang);
                checkCmd.Parameters.AddWithValue("@Nam", nam);

                int daChot = (int)checkCmd.ExecuteScalar();
                if (daChot > 0)
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "lock",
                        "alert('Tháng này đã CHỐT lương, không thể reset!');",
                        true);
                    return;
                }

                // ✅ Xóa bảng lương tháng
                string deleteSql = @"
        DELETE FROM BangLuongThang
        WHERE Thang = @Thang AND Nam = @Nam";

                SqlCommand delCmd = new SqlCommand(deleteSql, conn);
                delCmd.Parameters.AddWithValue("@Thang", thang);
                delCmd.Parameters.AddWithValue("@Nam", nam);

                delCmd.ExecuteNonQuery();
            }

           

            LoadBangLuong();
        }

        protected void ddlThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBangLuong();
        }

        protected void ddlNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBangLuong();
        }

        protected void gvLuong_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvLuong_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLuong.PageIndex = e.NewPageIndex;
            LoadBangLuong();
        }
    }
}