using System;
using System.Collections.Generic;
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
        void InitPage()
        {
            LoadThangNam();
            LoadPhongBan(); 
            LoadDropdown();

            ddlThang.SelectedValue = DateTime.Now.Month.ToString();
            ddlNam.SelectedValue = DateTime.Now.Year.ToString();

            LoadNgay(
                int.Parse(ddlThang.SelectedValue),
                int.Parse(ddlNam.SelectedValue)
            );

            ApplyUIByRole();

            if (!IsUser())
            {
                btnSave.Visible = false;
                
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
        private void LoadPhongBan()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Lấy từ bảng Bo_phan theo sơ đồ của bạn
                string sql = "SELECT IDBP, TenBP FROM Bo_phan";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                ddlPhongBan.Items.Clear();
                ddlPhongBan.DataSource = cmd.ExecuteReader();
                ddlPhongBan.DataTextField = "TenBP";
                ddlPhongBan.DataValueField = "IDBP"; // Giá trị này sẽ nạp vào @IDBP ở hàm trên
                ddlPhongBan.DataBind();

                ddlPhongBan.Items.Insert(0, new ListItem("-- Chọn Bộ Phận --", "0"));
            }
        }
        protected void ddlPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Load lại Dropdown nhân viên dựa trên phòng ban mới chọn
            LoadDropdown();

            // Reset GridView
            gvChamCong.DataSource = null;
            gvChamCong.DataBind();
            lblTongCong.Text = "0";
            btnSave.Visible = false;
        }
        public bool IsNgayLe(DateTime ngay)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Danh_muc_ngay_le WHERE NgayLe = @Ngay";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Ngay", ngay.Date);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
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
        private void LoadDropdown(string keyword = "")
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                if (IsUser())
                {
                    cmd.CommandText = "SELECT MaNV, HoTen FROM Nhan_vien WHERE MaNV = @MaNV";
                    cmd.Parameters.AddWithValue("@MaNV", GetMaNVLogin());
                }
                else
                {
                    // Lọc theo Bộ phận (IDBP) VÀ Keyword
                    string sql = "SELECT MaNV, HoTen FROM Nhan_vien WHERE 1=1";

                    // Kiểm tra ddlPhongBan (thực tế là ddlBoPhan) có chọn giá trị không
                    if (ddlPhongBan.SelectedValue != "0")
                    {
                        // SỬA TẠI ĐÂY: Dùng IDBP thay vì IDPB hay MaPB
                        sql += " AND IDBP = @IDBP";
                        cmd.Parameters.AddWithValue("@IDBP", ddlPhongBan.SelectedValue);
                    }

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        sql += " AND HoTen LIKE @Keyword";
                        cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                    }

                    cmd.CommandText = sql;
                }

                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);

                ddlNhanVien.Items.Clear();
                if (!IsUser())
                {
                    ddlNhanVien.Items.Add(new ListItem("-- Chọn Nhân Viên --", "0"));
                }

                ddlNhanVien.DataSource = dt;
                ddlNhanVien.DataTextField = "HoTen";
                ddlNhanVien.DataValueField = "MaNV";
                ddlNhanVien.DataBind();
            }
        

            if (IsUser())
            {
              
                ddlPhongBan.Enabled = false;
                ddlNhanVien.Enabled = false;
                btnLoad_Click(null, null);
            }
        }



        void LoadNgay(int thang, int nam)
        {
            ddlNgay.Items.Clear();

            int soNgay = DateTime.DaysInMonth(nam, thang);
            for (int d = 1; d <= soNgay; d++)
            {
                ddlNgay.Items.Add(new ListItem(d.ToString(), d.ToString()));
            }

            // tránh lỗi ngày > số ngày trong tháng
            int ngayHT = DateTime.Now.Day;
            if (ngayHT <= soNgay)
                ddlNgay.SelectedValue = ngayHT.ToString();
        }


        void ApplyUIByRole()
        {
            bool isUser = IsUser();
            btnResetCongThang.Visible = !isUser;
            btnTongHopCong.Visible = !isUser;
            btnChamCongAll.Visible = !isUser;
            ddlCongAll.Visible = !isUser;
            btnSave.Visible = !isUser; 
            pnlCong.Visible = !isUser;
        }
        protected void btnChamCongAll_Click(object sender, EventArgs e)
        {
            try
            {
                int thang = int.Parse(ddlThang.SelectedValue);
                int nam = int.Parse(ddlNam.SelectedValue);
                int ngay = int.Parse(ddlNgay.SelectedValue);
                DateTime ngayCham = new DateTime(nam, thang, ngay);
                double cong = double.Parse(ddlCongAll.SelectedValue, CultureInfo.InvariantCulture);

                List<string> dsMaNV = new List<string>();

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    // Bắt đầu Transaction từ đây
                    SqlTransaction tran = conn.BeginTransaction();

                    try
                    {
                        // 1️⃣ BỔ SUNG: Lấy danh sách MaNV từ Database
                        string sqlGetNV = "SELECT MaNV FROM Nhan_vien";
                        using (SqlCommand cmdGet = new SqlCommand(sqlGetNV, conn, tran))
                        {
                            using (SqlDataReader rd = cmdGet.ExecuteReader())
                            {
                                while (rd.Read())
                                {
                                    dsMaNV.Add(rd["MaNV"].ToString());
                                }
                            }
                        }

                        // 2️⃣ KIỂM TRA: Nếu không có ai thì báo lỗi luôn
                        if (dsMaNV.Count == 0)
                        {
                            tran.Rollback();
                            ScriptManager.RegisterStartupScript(this, GetType(), "warn", "alert('Không có nhân viên nào trong danh sách!');", true);
                            return;
                        }

                        // 3️⃣ THỰC THI: Chấm công cho từng người trong danh sách đã lấy
                        foreach (string maNV in dsMaNV)
                        {
                            SaveChamCong(conn, maNV, ngayCham, cong, "Đi Làm", tran);
                        }

                        tran.Commit(); // Xác nhận hoàn tất
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback(); // Hủy bỏ nếu có bất kỳ lỗi nào xảy ra
                        throw ex;
                    }
                }

                // 🔥 RESET GIAO DIỆN
                if (!IsUser())
                {
                    ddlNhanVien.SelectedIndex = 0;
                    gvChamCong.DataSource = null;
                    gvChamCong.DataBind();
                    lblTongCong.Text = "0";
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "ok",
                    $"alert('Đã chấm {cong} công cho {dsMaNV.Count} nhân viên ngày {ngay:00}/{thang:00}/{nam}!');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "err", $"alert('Lỗi hệ thống: {ex.Message}');", true);
            }
        }

        // ====================================
        // LƯU CHẤM CÔNG
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string maNV = ddlNhanVien.SelectedValue;
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);
            int ngayChon = int.Parse(ddlNgay.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(); // Bắt đầu giao dịch

                try
                {
                    foreach (GridViewRow row in gvChamCong.Rows)
                    {
                        DateTime ngayFull = Convert.ToDateTime(gvChamCong.DataKeys[row.RowIndex].Value);

                        // 1. Lưu Công
                        DropDownList ddl = row.FindControl("ddlCong") as DropDownList;
                        double cong = double.Parse(ddl.SelectedValue, CultureInfo.InvariantCulture);
                        TextBox txtGhiChu = row.FindControl("txtGhiChu") as TextBox;

                        // Truyền thêm transaction vào hàm này
                        SaveChamCong(conn, maNV, ngayFull, cong, txtGhiChu.Text, transaction);

                        // 2. Lưu/Xóa Tăng Ca
                        TextBox txtSoGioTC = (TextBox)row.FindControl("txtSoGioTC");
                        HiddenField hfIDLoaiCa = (HiddenField)row.FindControl("hfIDLoaiCa");

                        if (float.TryParse(txtSoGioTC.Text, out float soGio))
                        {
                            if (soGio > 0)
                                SaveTangCaWithTran(conn, maNV, ngayFull.Day, thang, nam, soGio, int.Parse(hfIDLoaiCa.Value), transaction);
                            else
                                DeleteTangCaWithTran(conn, maNV, ngayFull.Day, thang, nam, transaction);
                        }
                    }

                    transaction.Commit(); // Nếu đến đây không lỗi thì xác nhận lưu
                    UpdateLabelTongCongThang(maNV, thang, nam);
                    ScriptManager.RegisterStartupScript(this, GetType(), "success", $"alert('Lưu thành công ngày {ngayChon}!');", true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Nếu lỗi thì hủy bỏ toàn bộ thay đổi
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", $"alert('Lỗi: {ex.Message}');", true);
                }
            }
        }
        private void UpdateLabelTongCongThang(string maNV, int thang, int nam)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT SUM(Cong) FROM Bangcong_nhanvien_chitiet WHERE MaNV = @MaNV AND MONTH(Ngay) = @Thang AND YEAR(Ngay) = @Nam";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);
                conn.Open();
                object result = cmd.ExecuteScalar();
                lblTongCong.Text = (result != DBNull.Value) ? Convert.ToDouble(result).ToString("0.##") : "0";
                HienThiTongCongThang(int.Parse(maNV), thang, nam);
            }
        }
        private void SaveTangCaWithTran(SqlConnection conn, string maNV, int ngay, int thang, int nam, float soGio, int idLoaiCa, SqlTransaction tran)
        {
            string sql = @"
        IF EXISTS (SELECT 1 FROM Tang_ca WHERE MaNV = @MaNV AND Ngay = @Ngay AND Thang = @Thang AND Nam = @Nam)
        BEGIN
            UPDATE Tang_ca SET SoGio = @SoGio, IDLoaiCa = @IDLoaiCa 
            WHERE MaNV = @MaNV AND Ngay = @Ngay AND Thang = @Thang AND Nam = @Nam
        END
        ELSE
        BEGIN
            INSERT INTO Tang_ca (Nam, Thang, Ngay, SoGio, MaNV, IDLoaiCa)
            VALUES (@Nam, @Thang, @Ngay, @SoGio, @MaNV, @IDLoaiCa)
        END";

            using (SqlCommand cmd = new SqlCommand(sql, conn, tran)) // Thêm tham số tran ở đây
            {
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Ngay", ngay);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);
                cmd.Parameters.AddWithValue("@SoGio", soGio);
                cmd.Parameters.AddWithValue("@IDLoaiCa", idLoaiCa);
                cmd.ExecuteNonQuery();
            }
        }
        private void DeleteTangCaWithTran(SqlConnection conn, string maNV, int ngay, int thang, int nam, SqlTransaction tran)
        {
            string sql = "DELETE FROM Tang_ca WHERE MaNV = @MaNV AND Ngay = @Ngay AND Thang = @Thang AND Nam = @Nam";
            using (SqlCommand cmd = new SqlCommand(sql, conn, tran)) // Thêm tham số tran ở đây
            {
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Ngay", ngay);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);
                cmd.ExecuteNonQuery();
            }
        }
        // ====================================
        // TÍNH CÔNG
        // ====================================
        private void SaveChamCong(SqlConnection conn, string maNV, DateTime ngay, double cong, string ghiChu, SqlTransaction tran = null)
        {
            string sql = @"
        IF EXISTS (SELECT 1 FROM Bangcong_nhanvien_chitiet WHERE MaNV = @MaNV AND CAST(Ngay AS DATE) = @Ngay)
        BEGIN
            UPDATE Bangcong_nhanvien_chitiet SET Cong = @Cong, GhiChu = @GhiChu WHERE MaNV = @MaNV AND CAST(Ngay AS DATE) = @Ngay
        END
        ELSE
        BEGIN
            INSERT INTO Bangcong_nhanvien_chitiet (MaNV, Ngay, Cong, GhiChu) VALUES (@MaNV, @Ngay, @Cong, @GhiChu)
        END";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Ngay", ngay.Date);
                cmd.Parameters.AddWithValue("@Cong", cong);
                cmd.Parameters.AddWithValue("@GhiChu", ghiChu ?? "");
                cmd.ExecuteNonQuery();
            }
        }
        private void CapNhatTongCong()
        {
            double tong = 0;

            foreach (GridViewRow row in gvChamCong.Rows)
            {
                DropDownList ddl = row.FindControl("ddlCong") as DropDownList;
                if (ddl != null)
                {
                    tong += double.Parse(
                        ddl.SelectedValue,
                        CultureInfo.InvariantCulture);
                }
            }

            lblTongCong.Text = tong.ToString("0.##");
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

        void LoadChamCong(int thang, int nam, int maNV, int ngay)
        {
            // 1. Tạo DataTable chỉ với 1 dòng duy nhất cho ngày được chọn
            DataTable dt = new DataTable();
            dt.Columns.Add("Ngay", typeof(DateTime));
            dt.Columns.Add("Thu");
            dt.Columns.Add("Cong", typeof(double));
            dt.Columns.Add("GhiChu");
            dt.Columns.Add("SoGio", typeof(float));

            DateTime ngayChon = new DateTime(nam, thang, ngay);
            DataRow row = dt.NewRow();
            row["Ngay"] = ngayChon;
            row["Thu"] = ngayChon.ToString("dddd");
            row["Cong"] = 0;
            row["GhiChu"] = "";
            row["SoGio"] = 0;
            dt.Rows.Add(row);

            // 2. Truy vấn DB đúng ngày đó (Sửa SQL lọc theo DAY)
            string sql = @"
    SELECT bc.Cong, bc.GhiChu, 
           (SELECT TOP 1 SoGio FROM Tang_ca tc 
            WHERE tc.MaNV = bc.MaNV 
              AND tc.Ngay = DAY(bc.Ngay) 
              AND tc.Thang = MONTH(bc.Ngay) 
              AND tc.Nam = YEAR(bc.Ngay)) as SoGio
    FROM Bangcong_nhanvien_chitiet bc
    WHERE bc.MaNV = @MaNV
      AND DAY(bc.Ngay) = @Ngay
      AND MONTH(bc.Ngay) = @Thang
      AND YEAR(bc.Ngay) = @Nam";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Ngay", ngay);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dt.Rows[0]["Cong"] = reader["Cong"];
                        dt.Rows[0]["GhiChu"] = reader["GhiChu"];
                        dt.Rows[0]["SoGio"] = reader["SoGio"];
                    }
                }
            }

            // 3. Bind lên GridView (Lúc này chỉ hiện 1 dòng duy nhất)
            gvChamCong.DataSource = dt;
            gvChamCong.DataBind();

            // Cập nhật nhãn hiển thị (Tổng công lúc này chỉ là công của ngày đó)
            HienThiTongCongThang(maNV, thang, nam);
        }
        private void HienThiTongCongThang(int maNV, int thang, int nam)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Sử dụng DISTINCT hoặc GROUP BY để tránh trường hợp Join làm nhân bản số liệu
                string sql = @"
            SELECT SUM(t.Cong) 
            FROM (
                SELECT DISTINCT Ngay, Cong 
                FROM Bangcong_nhanvien_chitiet 
                WHERE MaNV = @MaNV 
                  AND MONTH(Ngay) = @Thang 
                  AND YEAR(Ngay) = @Nam
            ) as t";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

                conn.Open();
                object result = cmd.ExecuteScalar();

                double tong = (result != DBNull.Value) ? Convert.ToDouble(result) : 0;
                lblTongCong.Text = " " + tong.ToString("0.##");
            }
        }
      
        private void LoadChamCongCaThang(int thang, int nam, int maNV)
        {
            // 1. Tạo DataTable cấu trúc chuẩn cho GridView
            DataTable dt = new DataTable();
            dt.Columns.Add("Ngay", typeof(DateTime));
            dt.Columns.Add("Thu");
            dt.Columns.Add("Cong", typeof(double));
            dt.Columns.Add("GhiChu");
            dt.Columns.Add("SoGio", typeof(float));

            // 2. Tạo danh sách tất cả các ngày trong tháng (Mặc định là chưa có công)
            int soNgay = DateTime.DaysInMonth(nam, thang);

            for (int d = 1; d <= soNgay; d++)
            {
                DateTime ngay = new DateTime(nam, thang, d);
                DataRow row = dt.NewRow();
                row["Ngay"] = ngay;
                row["Thu"] = ngay.ToString("dddd");
                row["Cong"] = 0;
                row["GhiChu"] = "";
                row["SoGio"] = 0;
                dt.Rows.Add(row);
            }

            // 3. Truy vấn dữ liệu thực tế đã lưu trong DB (cả bảng công và tăng ca)
            string sql = @"
        SELECT bc.Ngay, bc.Cong, bc.GhiChu, 
               (SELECT TOP 1 SoGio FROM Tang_ca tc 
                WHERE tc.MaNV = bc.MaNV AND tc.Ngay = DAY(bc.Ngay) 
                AND tc.Thang = MONTH(bc.Ngay) AND tc.Nam = YEAR(bc.Ngay)) as SoGio
        FROM Bangcong_nhanvien_chitiet bc
        WHERE bc.MaNV = @MaNV AND MONTH(bc.Ngay) = @Thang AND YEAR(bc.Ngay) = @Nam";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime ngayDB = Convert.ToDateTime(reader["Ngay"]);
                    // Tìm dòng tương ứng trong DataTable tạm để cập nhật dữ liệu
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (((DateTime)dr["Ngay"]).Date == ngayDB.Date)
                        {
                            dr["Cong"] = reader["Cong"];
                            dr["GhiChu"] = reader["GhiChu"];
                            dr["SoGio"] = reader["SoGio"] != DBNull.Value ? reader["SoGio"] : 0;
                            break;
                        }
                    }
                }
            }

            // 4. Đổ vào GridView
            gvChamCong.DataSource = dt;
            gvChamCong.DataBind();

            // 5. Cập nhật tổng công hiển thị phía dưới
            HienThiTongCongThang(maNV, thang, nam);
        }
        protected void rblViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
   
            // Tự động load lại dữ liệu khi đổi chế độ
            if (ddlNhanVien.SelectedValue != "0" || IsUser())
            {
                ExecuteLoad();
            }
        }
        // Gom logic Load vào 1 hàm chung để tránh viết đi viết lại
        private void ExecuteLoad()
        {
            // Kiểm tra nếu chưa chọn nhân viên (đối với Admin)
            if (!IsUser() && (ddlNhanVien.SelectedValue == "0" || string.IsNullOrEmpty(ddlNhanVien.SelectedValue)))
            {
                gvChamCong.DataSource = null;
                gvChamCong.DataBind();
                btnSave.Visible = false;
                return;
            }

            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);
            int maNV = IsUser() ? GetMaNVLogin().Value : int.Parse(ddlNhanVien.SelectedValue);

            if (rblViewMode.SelectedValue == "Day")
            {
                int ngay = int.Parse(ddlNgay.SelectedValue);
                LoadChamCong(thang, nam, maNV, ngay);
            }
            else
            {
                LoadChamCongCaThang(thang, nam, maNV);
            }

            // Chỉ hiện nút Lưu nếu là Admin (hoặc người có quyền sửa)
            if (!IsUser()) btnSave.Visible = true;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(ddlThang.SelectedValue, out int thang))
                return;

            if (!int.TryParse(ddlNam.SelectedValue, out int nam))
                return;

            // Lấy thêm giá trị ngày từ DropDownList ddlNgay
            if (!int.TryParse(ddlNgay.SelectedValue, out int ngay))
                return;

            int maNV = IsUser()
                ? GetMaNVLogin().Value
                : int.Parse(ddlNhanVien.SelectedValue);

            if (maNV > 0)
            {
                LoadChamCong(thang, nam, maNV, ngay);

                // Hiện nút Save nếu không phải là User thường
                if (!IsUser()) btnSave.Visible = true;
            }
            else
            {
                btnSave.Visible = false;
            }
            // Cập nhật: Truyền thêm biến 'ngay' vào tham số thứ 4
            LoadChamCong(thang, nam, maNV, ngay);
            ExecuteLoad();
        }
        protected void ddlThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadNgay(
                int.Parse(ddlThang.SelectedValue),
                int.Parse(ddlNam.SelectedValue)
            );
        }

        protected void ddlNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadNgay(
                int.Parse(ddlThang.SelectedValue),
                int.Parse(ddlNam.SelectedValue)
            );
        }
        protected void ddlNgay_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ngay = int.Parse(ddlNgay.SelectedValue);

            // test nhanh
            ScriptManager.RegisterStartupScript(
                this, GetType(), "x",
                "alert('Ngày đã chọn: " + ngay + "');", true);
        }


        protected void ddlNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy giá trị được chọn từ DropDownList
            string maNV = ddlNhanVien.SelectedValue;

            // Nếu chọn dòng "-- Chọn Nhân Viên --" (có Value = "0")
            if (maNV == "0")
            {
                // Xóa sạch dữ liệu trên GridView
                gvChamCong.DataSource = null;
                gvChamCong.DataBind();

                // Reset nhãn tổng công về 0
                lblTongCong.Text = "0";

                // Bạn có thể ẩn luôn nút Lưu nếu muốn
                btnSave.Visible = false;
            }
            else
            {
                // Nếu chọn một nhân viên cụ thể
                int thang = int.Parse(ddlThang.SelectedValue);
                int nam = int.Parse(ddlNam.SelectedValue);

                // LẤY THÊM: Giá trị ngày đang chọn từ DropDownList ddlNgay
                int ngay = int.Parse(ddlNgay.SelectedValue);

                // CẬP NHẬT: Truyền đủ 4 tham số (thang, nam, maNV, ngay)
                LoadChamCong(thang, nam, int.Parse(maNV), ngay);

                // Hiện lại nút Lưu
                btnSave.Visible = true;
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            LoadDropdown(keyword);
        }


        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }

     
        private void TinhTongCong()
        {
            double tong = 0;

            foreach (GridViewRow row in gvChamCong.Rows)
            {
                DropDownList ddl = row.FindControl("ddlCong") as DropDownList;
                if (ddl != null)
                    tong += double.Parse(ddl.SelectedValue, CultureInfo.InvariantCulture);
            }

            lblTongCong.Text = tong.ToString("0.##");
        }


        protected void gvChamCong_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // 1. Lấy ngày từ DataKeyNames của GridView
                DateTime ngay = Convert.ToDateTime(gvChamCong.DataKeys[e.Row.RowIndex].Value);

                // 2. Tìm các Control đã thêm vào ASPX
                Label lblLoaiCa = (Label)e.Row.FindControl("lblLoaiCa");
                HiddenField hfIDLoaiCa = (HiddenField)e.Row.FindControl("hfIDLoaiCa");
                DropDownList ddlCong = (DropDownList)e.Row.FindControl("ddlCong");
                TextBox txtGhiChu = (TextBox)e.Row.FindControl("txtGhiChu");
                TextBox txtSoGioTC = (TextBox)e.Row.FindControl("txtSoGioTC"); // TextBox nhập giờ tăng ca

                // 3. Logic phân loại (Ưu tiên: Lễ > Chủ Nhật > Thường)
                if (IsNgayLe(ngay))
                {
                    lblLoaiCa.Text = "Ngày Lễ";
                    lblLoaiCa.ForeColor = System.Drawing.Color.OrangeRed;
                    lblLoaiCa.Font.Bold = true;
                    hfIDLoaiCa.Value = "3"; // IDLoaiCa trong DB cho ngày Lễ
                }
                else if (ngay.DayOfWeek == DayOfWeek.Sunday)
                {
                    lblLoaiCa.Text = "Chủ nhật";
                    lblLoaiCa.ForeColor = System.Drawing.Color.Red;
                    hfIDLoaiCa.Value = "2"; // IDLoaiCa trong DB cho Chủ nhật
                }
                else
                {
                    lblLoaiCa.Text = "Ngày thường";
                    lblLoaiCa.ForeColor = System.Drawing.Color.Black;
                    hfIDLoaiCa.Value = "1"; // IDLoaiCa trong DB cho Ngày thường
                }

                // 4. Logic khóa quyền nếu là User thường (role 10)
                if (IsUser())
                {
                    if (ddlCong != null) ddlCong.Enabled = false;
                    if (txtGhiChu != null) txtGhiChu.ReadOnly = true;
                    if (txtSoGioTC != null) txtSoGioTC.ReadOnly = true; // User không được tự nhập tăng ca
                }
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
                        "alert('Tháng này đã tổng hợp công, Cần reset lại lương tháng và bảng công tháng nếu muốn tổng hợp lại công tháng !');", true);
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
            Response.Redirect("TongHopCongThang.aspx?thang=" + thang + "&nam=" + nam);
        }
        protected void btnResetCongThang_Click(object sender, EventArgs e)
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // ❌ Không cho reset nếu đã tính lương
                SqlCommand checkLuong = new SqlCommand(@"
            SELECT COUNT(*) 
            FROM BangLuongThang
            WHERE Thang = @Thang AND Nam = @Nam", conn);

                checkLuong.Parameters.AddWithValue("@Thang", thang);
                checkLuong.Parameters.AddWithValue("@Nam", nam);

                if ((int)checkLuong.ExecuteScalar() > 0)
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "lock",
                        "alert('Đã tính lương tháng này, không thể reset công!');",
                        true);
                    return;
                }

                // ✅ Xóa bảng tổng hợp công
                SqlCommand del = new SqlCommand(@"
            DELETE FROM BangCongThang
            WHERE Thang = @Thang AND Nam = @Nam", conn);

                del.Parameters.AddWithValue("@Thang", thang);
                del.Parameters.AddWithValue("@Nam", nam);
                del.ExecuteNonQuery();
            }

            ScriptManager.RegisterStartupScript(
                this, GetType(), "ok",
                "alert('Đã reset công tháng. Có thể tổng hợp lại!');",
                true);
        }

        protected void gvChamCong_SelectedIndexChanged1(object sender, EventArgs e)
        {
            DropDownList ddl = sender as DropDownList;
            GridViewRow row = ddl.NamingContainer as GridViewRow;

            string maNV = ddlNhanVien.SelectedValue;
            DateTime ngay = Convert.ToDateTime(
                gvChamCong.DataKeys[row.RowIndex].Value);

            double cong = double.Parse(
                ddl.SelectedValue,
                CultureInfo.InvariantCulture);

            TextBox txtGhiChu = row.FindControl("txtGhiChu") as TextBox;
            string ghiChu = txtGhiChu?.Text ?? "";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SaveChamCong(conn, maNV, ngay, cong, ghiChu);
            }

            // 👉 cập nhật tổng công
            TinhTongCong();
        }

       
    }
}
