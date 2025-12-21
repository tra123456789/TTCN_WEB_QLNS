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
                    btnExportExcel.Visible = false;
                }
                LoadThangNam();
                LoadBangLuong();

            }

        }
        void TinhLuongThangChoTatCaNhanVien(int thang, int nam)
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
                WHERE Thang = @Thang AND Nam = @Nam
            ", conn, tran);

                    checkCmd.Parameters.AddWithValue("@Thang", thang);
                    checkCmd.Parameters.AddWithValue("@Nam", nam);

                    if ((int)checkCmd.ExecuteScalar() > 0)
                    {
                        tran.Rollback();
                        return;
                    }

                    // 2️⃣ SQL INSERT LƯƠNG
                    string sqlInsertLuong = @"
INSERT INTO BangLuongThang
(
    MaNV, Thang, Nam,
    LuongCoBan,
    TongNgayCong,
    TongThuong,
    TongPhat,
    BHXH, BHYT, BHTN,
    ThucLanh,
    TrangThai
)
SELECT
    nv.MaNV,
    @Thang,
    @Nam,
    pb.LuongCoBan,
    ISNULL(bc.TongCong, 0),
    ISNULL(kt.TongThuong, 0),
    ISNULL(kt.TongPhat, 0),

    pb.LuongCoBan * 0.08,
    pb.LuongCoBan * 0.015,
    pb.LuongCoBan * 0.01,

    ((pb.LuongCoBan / 26) * ISNULL(bc.TongCong, 0))
    + ISNULL(kt.TongThuong, 0)
    - ISNULL(kt.TongPhat, 0)
    - (pb.LuongCoBan * 0.08)
    - (pb.LuongCoBan * 0.015)
    - (pb.LuongCoBan * 0.01),

    0
FROM Nhan_vien nv
LEFT JOIN BangCongThang bc 
       ON bc.MaNV = nv.MaNV
      AND bc.Thang = @Thang
      AND bc.Nam = @Nam
JOIN Bo_phan bp ON nv.IDBP = bp.IDBP
JOIN Phong_ban pb ON bp.IDPB = pb.IDPB
LEFT JOIN (
    SELECT MaNV,
           SUM(CASE WHEN Loai = 1 THEN SoKTKL ELSE 0 END) AS TongThuong,
           SUM(CASE WHEN Loai = 2 THEN SoKTKL ELSE 0 END) AS TongPhat
    FROM KhenThuong_KyLuat
   WHERE MONTH(Ngay) = @Thang
  AND YEAR(Ngay) = @Nam
    GROUP BY MaNV
) kt ON nv.MaNV = kt.MaNV;
";


                    SqlCommand cmd = new SqlCommand(sqlInsertLuong, conn, tran);
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

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




        private void LoadBangLuong()
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);


            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string role = Session["IDROLE"].ToString();
                string sql = "";

                // ADMIN / KẾ TOÁN
                if (role == "1" || role == "12")
                {
                    sql = @"
                SELECT 
                bl.MaNV,
                nv.HoTen,
                pb.TenPB,
                bl.LuongCoBan,
                bl.TongNgayCong,
                bl.TongThuong,
                bl.TongPhat,
                bl.BHXH,
                bl.BHYT,
                bl.BHTN,
                bl.ThucLanh
            FROM BangLuongThang bl
            JOIN Nhan_vien nv ON bl.MaNV = nv.MaNV
            JOIN Bo_phan bp ON nv.IDBP = bp.IDBP
            JOIN Phong_ban pb ON bp.IDPB = pb.IDPB
            WHERE bl.Thang = @Thang AND bl.Nam = @Nam
            ORDER BY pb.TenPB, nv.HoTen
";
                }
                else // USER → chỉ xem lương của mình
                {
                    sql = @"
                SELECT 
                bl.MaNV,
                nv.HoTen,
                pb.TenPB,
                bl.LuongCoBan,
                bl.TongNgayCong,
                bl.TongThuong,
                bl.TongPhat,
                bl.BHXH,
                bl.BHYT,
                bl.BHTN,
                bl.ThucLanh
            FROM BangLuongThang bl
            JOIN Nhan_vien nv ON bl.MaNV = nv.MaNV
            JOIN Bo_phan bp ON nv.IDBP = bp.IDBP
            JOIN Phong_ban pb ON bp.IDPB = pb.IDPB
            WHERE bl.Thang = @Thang 
              AND bl.Nam = @Nam
              AND bl.MaNV = @MaNV
";
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


            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
            UPDATE BangLuongThang
            SET TrangThai = 1
            WHERE Thang = @Thang AND Nam = @Nam";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

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

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // ❌ Không cho tính lại nếu đã chốt
                string checkSql = @"
            SELECT COUNT(*) 
            FROM BangLuongThang
            WHERE Thang = @Thang AND Nam = @Nam AND TrangThai = 1";

                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@Thang", thang);
                checkCmd.Parameters.AddWithValue("@Nam", nam);

                int daChot = (int)checkCmd.ExecuteScalar();
                if (daChot > 0)
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "lock",
                        "alert('Tháng này đã chốt lương, không thể tính lại!');",
                        true);
                    return;
                }
            }

            
             TinhLuongThangChoTatCaNhanVien(thang, nam);

            ScriptManager.RegisterStartupScript(
                this, GetType(), "ok",
                "alert('Tính lương tháng thành công!');", true);

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

        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);


            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string role = Session["IDROLE"].ToString();
                SqlCommand cmd;

                // ADMIN / KẾ TOÁN
                if (role == "1" || role == "12")
                {
                    string sql = @"
                SELECT 
                    bl.MaNV,
                    nv.HoTen,
                    pb.TenPB,
                    bl.LuongCoBan,
                    bl.TongNgayCong,
                    bl.TongThuong,
                    bl.TongPhat,
                    bl.BHXH,
                    bl.BHYT,
                    bl.BHTN,
                    bl.ThucLanh
                FROM BangLuongThang bl
                JOIN Nhan_vien nv ON bl.MaNV = nv.MaNV
                JOIN Bo_phan bp ON nv.IDBP = bp.IDBP
                JOIN Phong_ban pb ON bp.IDPB = pb.IDPB
                WHERE bl.Thang = @Thang
                  AND bl.Nam = @Nam
                  AND (nv.MaNV LIKE @search OR nv.HoTen LIKE @search)
                ORDER BY pb.TenPB, nv.HoTen";

                    cmd = new SqlCommand(sql, conn);
                }
                else // USER → chỉ search chính mình
                {
                    string sql = @"
                SELECT 
                    bl.MaNV,
                    nv.HoTen,
                    pb.TenPB,
                    bl.LuongCoBan,
                    bl.TongNgayCong,
                    bl.TongThuong,
                    bl.TongPhat,
                    bl.BHXH,
                    bl.BHYT,
                    bl.BHTN,
                    bl.ThucLanh
                FROM BangLuongThang bl
                JOIN Nhan_vien nv ON bl.MaNV = nv.MaNV
                JOIN Bo_phan bp ON nv.IDBP = bp.IDBP
                JOIN Phong_ban pb ON bp.IDPB = pb.IDPB
                WHERE bl.Thang = @Thang
                  AND bl.Nam = @Nam
                  AND bl.MaNV = @MaNV
                  AND nv.HoTen LIKE @search";

                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaNV", Session["MaNV"].ToString());
                }

                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);
                cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvLuong.DataSource = dt;
                gvLuong.DataBind();
            }
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
    }
}