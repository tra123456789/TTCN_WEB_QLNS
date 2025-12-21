using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class QuanLyChamCong : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;
        private const double NGHI_TRUA = 1; // trừ 1 giờ nghỉ trưa

        protected void Page_Load(object sender, EventArgs e)
        {
            string role = Session["IDROLE"] as string;

            if (Session["UserName"] == null || role == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }

            // 🔒 User thường mới cần MaNV
            if (role == "10" && Session["MaNV"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }
            if (!IsPostBack)
            {
                //    LoadDropdown();


                //    ddlThang.SelectedValue = DateTime.Now.Month.ToString();
                //    ddlNam.SelectedValue = DateTime.Now.Year.ToString();

                //    // load luôn bảng tháng hiện tại
                //    btnLoad_Click(null, null);
                InitPage();
            }
        }
        void LoadThangNam()
        {
            // ===== THÁNG =====
            ddlThang.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                ddlThang.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            // ===== NĂM =====
            ddlNam.Items.Clear();
            int namHT = DateTime.Now.Year;
            for (int y = namHT - 3; y <= namHT + 1; y++)
            {
                ddlNam.Items.Add(new ListItem(y.ToString(), y.ToString()));
            }
        }

        bool IsUser()
        {
            return Session["IDROLE"].ToString() == "10";
        }

        int? GetMaNVLogin()
        {
            if (IsUser())
                return int.Parse(Session["MaNV"].ToString());

            return null; // admin
        }

        // ====================================
        // LOAD DROPDOWN (Tháng, năm, nhân viên)
        // ====================================
        private void LoadDropdown()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da;

                if (IsUser())
                {
                    da = new SqlDataAdapter(
                        "SELECT MaNV, HoTen FROM Nhan_vien WHERE MaNV=@MaNV", conn);
                    da.SelectCommand.Parameters.AddWithValue("@MaNV", GetMaNVLogin());
                }
                else
                {
                    da = new SqlDataAdapter(
                        "SELECT MaNV, HoTen FROM Nhan_vien", conn);
                }

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlNhanVien.DataSource = dt;
                ddlNhanVien.DataTextField = "HoTen";
                ddlNhanVien.DataValueField = "MaNV";
                ddlNhanVien.DataBind();
            }

            // đảm bảo có SelectedValue
            if (ddlNhanVien.Items.Count > 0)
                ddlNhanVien.SelectedIndex = 0;

            if (IsUser())
                ddlNhanVien.Enabled = false;
            
        }

        void InitPage()
        {
            LoadThangNam();     // 🔴 BẮT BUỘC
            LoadDropdown();     // nhân viên

            ddlThang.SelectedValue = DateTime.Now.Month.ToString();
            ddlNam.SelectedValue = DateTime.Now.Year.ToString();

            ApplyUIByRole();
        }


        void ApplyUIByRole()
        {
            if (IsUser())
            {
                btnSave.Visible = false;
                btnTinhCong.Visible = false;
            }
        }

        
   

        // ====================================
        // LƯU CHẤM CÔNG
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            // ⚠️ MaNV của bạn là sinh tự động → thường là STRING
            string maNV = ddlNhanVien.SelectedValue;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                foreach (GridViewRow row in gvChamCong.Rows)
                {
                    // 1️⃣ Lấy NGÀY từ DataKey
                    DateTime ngay = Convert.ToDateTime(gvChamCong.DataKeys[row.RowIndex].Value);

                    // 2️⃣ Lấy công
                    DropDownList ddl = row.FindControl("ddlCong") as DropDownList;
                    if (ddl == null) continue;

                    double cong = double.Parse(
                        ddl.SelectedValue,
                        CultureInfo.InvariantCulture
                    );

                    // 3️⃣ Ghi chú
                    TextBox txtGhiChu = row.FindControl("txtGhiChu") as TextBox;
                    string ghiChu = txtGhiChu?.Text ?? "";
                    

                    System.Diagnostics.Debug.WriteLine(
                        $"ROW {row.RowIndex} DATEKEY = {ngay:yyyy-MM-dd HH:mm:ss}"
                    );

                    // 4️⃣ LƯU VÀO DB
                    SaveChamCong(conn, maNV, ngay, cong, ghiChu);
                }

            }

            ScriptManager.RegisterStartupScript(
                this, GetType(), "ok",
                "alert('Lưu chấm công thành công!');", true);
        }



        // ====================================
        // TÍNH CÔNG
        // ====================================
        private void SaveChamCong(
    SqlConnection conn,
    string maNV,
    DateTime ngay,
    double cong,
    string ghiChu)
        {
            string sql = @"
IF EXISTS (
    SELECT 1
    FROM Bangcong_nhanvien_chitiet
    WHERE MaNV = @MaNV
      AND CAST(Ngay AS DATE) = CAST(@Ngay AS DATE)
)
BEGIN
    UPDATE Bangcong_nhanvien_chitiet
    SET Cong = @Cong,
        GhiChu = @GhiChu
    WHERE MaNV = @MaNV
      AND CAST(Ngay AS DATE) = CAST(@Ngay AS DATE)
END
ELSE
BEGIN
    INSERT INTO Bangcong_nhanvien_chitiet
    (MaNV, Ngay, Cong, GhiChu)
    VALUES
    (@MaNV, @Ngay, @Cong, @GhiChu)
END";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Ngay", ngay.Date); // 👈 RẤT QUAN TRỌNG
                cmd.Parameters.AddWithValue("@Cong", cong);
                cmd.Parameters.AddWithValue("@GhiChu", ghiChu ?? "");
                cmd.ExecuteNonQuery();
            }
            System.Diagnostics.Debug.WriteLine(
    $"SAVE: {maNV} - {ngay:yyyy-MM-dd} - {cong}"
);

        }

        protected void btnTinhCong_Click(object sender, EventArgs e)
        {
            double tong = 0;

            foreach (GridViewRow row in gvChamCong.Rows)
            {
                DropDownList ddl = row.FindControl("ddlCong") as DropDownList;
                if (ddl == null) continue;

                if (double.TryParse(
                        ddl.SelectedValue,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out double cong))
                {
                    tong += cong;
                }
            }

            lblTongCong.Text = tong.ToString("0.##");
        }


        private double TinhCongNgay(string vaoStr, string raStr)
        {
            if (DateTime.TryParse(vaoStr, out DateTime vao) &&
                DateTime.TryParse(raStr, out DateTime ra))
            {
                double gio = (ra - vao).TotalHours - NGHI_TRUA;

                if (gio >= 8) return 1;
                if (gio >= 4) return 0.5;
            }
            return 0;
        }

      

        // ====================================
        // LOAD GRID
        // ====================================
        DataTable TaoDuLieuThangMoi(int thang, int nam)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Ngay", typeof(DateTime));
            dt.Columns.Add("Thu");
            dt.Columns.Add("Cong");
            dt.Columns.Add("GhiChu");

            int soNgay = DateTime.DaysInMonth(nam, thang);

            for (int d = 1; d <= soNgay; d++)
            {
                DateTime ngay = new DateTime(nam, thang, d);
                DataRow row = dt.NewRow();
                row["Ngay"] = ngay;
                row["Thu"] = ngay.DayOfWeek.ToString();
                row["Cong"] = 0;
                row["GhiChu"] = "";
                dt.Rows.Add(row);
            }

            return dt;
        }

        void LoadChamCong(int thang, int nam, int maNV)
        {
            // 1️⃣ TẠO ĐỦ NGÀY TRƯỚC
            DataTable dt = TaoDuLieuThangMoi(thang, nam);

            // 2️⃣ LẤY DỮ LIỆU DB
            string sql = @"
        SELECT Ngay, Cong, GhiChu
        FROM Bangcong_nhanvien_chitiet
        WHERE MaNV = @MaNV
          AND MONTH(Ngay) = @Thang
          AND YEAR(Ngay) = @Nam";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

                DataTable dtDB = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtDB);
                }

                // 3️⃣ GHÉP DỮ LIỆU DB VÀO BẢNG ĐẦY ĐỦ NGÀY
                foreach (DataRow r in dtDB.Rows)
                {
                    DateTime ngay = Convert.ToDateTime(r["Ngay"]);

                    DataRow[] rows = dt.Select($"Ngay = #{ngay:MM/dd/yyyy}#");
                    if (rows.Length > 0)
                    {
                        rows[0]["Cong"] = r["Cong"];
                        rows[0]["GhiChu"] = r["GhiChu"];
                    }
                }
            }
            double tongCong = 0;

            foreach (DataRow r in dt.Rows)
            {
                tongCong += Convert.ToDouble(r["Cong"]);
            }

            // bind GridView
            gvChamCong.DataSource = dt;
            gvChamCong.DataBind();

            // hiển thị tổng công
            lblTongCong.Text = tongCong.ToString("0.##");

            gvChamCong.DataSource = dt;
            gvChamCong.DataBind();

        }


        protected void btnLoad_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(ddlThang.SelectedValue, out int thang))
                return;

            if (!int.TryParse(ddlNam.SelectedValue, out int nam))
                return;

            int maNV = IsUser()
                ? GetMaNVLogin().Value
                : int.Parse(ddlNhanVien.SelectedValue);

            LoadChamCong(thang, nam, maNV);
        }


        protected void ddlThang_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlNam_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }


        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }

        protected void gvChamCong_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }

        protected void gvChamCong_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && IsUser())
            {
                DropDownList ddlCong = (DropDownList)e.Row.FindControl("ddlCong");
                TextBox txtGhiChu = (TextBox)e.Row.FindControl("txtGhiChu");

                if (ddlCong != null) ddlCong.Enabled = false;
                if (txtGhiChu != null) txtGhiChu.ReadOnly = true;
            }
        }

        protected void btnTongHopCong_Click(object sender, EventArgs e)
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);

            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // ❌ Không cho tổng hợp lại
                SqlCommand check = new SqlCommand(@"
            SELECT COUNT(*) FROM BangCongThang
            WHERE Thang = @Thang AND Nam = @Nam", conn);

                check.Parameters.AddWithValue("@Thang", thang);
                check.Parameters.AddWithValue("@Nam", nam);

                if ((int)check.ExecuteScalar() > 0)
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "x",
                        "alert('Tháng này đã tổng hợp công!');", true);
                    return;
                }

                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO BangCongThang (MaNV, Thang, Nam, TongCong)
            SELECT MaNV, @Thang, @Nam, SUM(Cong)
            FROM Bangcong_nhanvien_chitiet
            WHERE MONTH(Ngay) = @Thang AND YEAR(Ngay) = @Nam
            GROUP BY MaNV", conn);

                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

                cmd.ExecuteNonQuery();
            }

            ScriptManager.RegisterStartupScript(
                this, GetType(), "ok",
                "alert('Tổng hợp công tháng thành công!');", true);
        }

    }
}
