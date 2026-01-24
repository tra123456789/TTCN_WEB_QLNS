using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class QuanLyChamCong : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;
        private const double NGHI_TRUA = 1; // trừ 1 giờ nghỉ trưa
        public int SoNgayTrongThang;

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
                SoNgayTrongThang = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
              
                InitPage();
                LoadChamCong();
               
            }
            //CapNhatTrangThaiNutChamCong();

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
            btnUserChamCong.Visible = IsUser();
            notify.Visible = !isUser;
            pnlPhongBan.Visible = !isUser;
            pnlNhanVien.Visible= !isUser;

        }
        void InitPage()
        {

            LoadThongBao();
            LoadThangNam();
            LoadPhongBan();
            
            
            LoadNotifyCount();
           

            ddlThang.SelectedValue = DateTime.Now.Month.ToString();
            ddlNam.SelectedValue = DateTime.Now.Year.ToString();

            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);

            LoadNgay(thang, nam);
            // 1. Load tiêu đề ngày (1, 2, 3... Thứ)
            LoadHeaderNgay(thang, nam);



            ApplyUIByRole();
            
            if (!IsUser())
            {
                btnSave.Visible = false;
                
            }
        }
        void LoadChamCong()
        {
            ExecuteLoad();
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
        private void LoadHeaderNgay(int thang, int nam)
        {
            // CẬP NHẬT QUAN TRỌNG: Gán giá trị cho biến dùng ở Colspan ngoài ASPX
            SoNgayTrongThang = DateTime.DaysInMonth(nam, thang);

            DataTable dtHeader = new DataTable();
            dtHeader.Columns.Add("Ngay");
            dtHeader.Columns.Add("Thu");
            dtHeader.Columns.Add("CssClass");

            for (int i = 1; i <= SoNgayTrongThang; i++)
            {
                DateTime d = new DateTime(nam, thang, i);
                DataRow dr = dtHeader.NewRow();
                dr["Ngay"] = i;
                dr["Thu"] = d.ToString("ddd", new System.Globalization.CultureInfo("vi-VN"));
                dr["CssClass"] = (d.DayOfWeek == DayOfWeek.Sunday) ? "bg-sunday" : "";
                dtHeader.Rows.Add(dr);
            }

            rptHeaderNgay.DataSource = dtHeader;
            rptHeaderNgay.DataBind();
        }
        private void LoadBangChamCong()
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);
            SoNgayTrongThang = DateTime.DaysInMonth(nam, thang);

            // 1. Vẽ Header (Ngày 1, 2, 3... và Thứ)
            var listHeader = new List<object>();
            for (int i = 1; i <= SoNgayTrongThang; i++)
            {
                DateTime ngay = new DateTime(nam, thang, i);
                string thu = (ngay.DayOfWeek == DayOfWeek.Sunday) ? "CN" : "T" + ((int)ngay.DayOfWeek + 1);
                string color = (ngay.DayOfWeek == DayOfWeek.Sunday) ? "background-color: yellow; color: red;" : "";
                listHeader.Add(new { Ngay = i, Thu = thu, Color = color });
            }
            rptHeaderNgay.DataSource = listHeader;
            rptHeaderNgay.DataBind();

            // 2. Lấy danh sách nhân viên (Dùng hàm này để thay thế cho gvChamCong.DataSource)
            DataTable dtNhanVien = LayDanhSachNhanVien();
            rptNhanVien.DataSource = dtNhanVien;
            rptNhanVien.DataBind();
        }

        // Thêm tham số string maNV = "" vào cuối
        private DataTable LayDanhSachNhanVien(int idBP = 0, string maNV = "")
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
        SELECT 
            nv.MaNV,
            nv.HoTen,
            cv.TenCV AS TenChucVu,
            '' AS GhiChu
        FROM Nhan_vien nv
        INNER JOIN Hop_dong hd ON nv.MaNV = hd.MaNV
        LEFT JOIN Chuc_vu cv ON nv.IDCV = cv.IDCV
        WHERE 
            nv.TrangThai = 1
            AND hd.NgayBatDau <= GETDATE()
            AND hd.NgayKetThuc >= GETDATE()
        ";

                if (!string.IsNullOrEmpty(maNV))
                    sql += " AND nv.MaNV = @MaNV";
                else if (idBP > 0)
                    sql += " AND nv.IDBP = @IDBP";

                SqlCommand cmd = new SqlCommand(sql, conn);

                if (!string.IsNullOrEmpty(maNV))
                    cmd.Parameters.AddWithValue("@MaNV", maNV);

                if (idBP > 0)
                    cmd.Parameters.AddWithValue("@IDBP", idBP);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private void XuLyNutChamCongHomNay()
        {
            btnUserChamCong.Visible = false;

            if (!IsUser()) return;

            if (!int.TryParse(ddlNgay.SelectedValue, out int ngay) ||
                !int.TryParse(ddlThang.SelectedValue, out int thang) ||
                !int.TryParse(ddlNam.SelectedValue, out int nam))
                return;

            DateTime ngayDangChon = new DateTime(nam, thang, ngay);

            // ❌ Không phải hôm nay → không hiện
            if (ngayDangChon.Date != DateTime.Today)
                return;

            string maNV = Session["MaNV"]?.ToString();
            if (string.IsNullOrEmpty(maNV)) return;

            // ❌ Đã chấm rồi → không hiện
            if (DaChamCong(maNV, ngayDangChon))
                return;

            // ✅ Đủ điều kiện
            btnUserChamCong.Visible = true;
        }
        private bool DaChamCong(string maNV, DateTime ngay)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"SELECT COUNT(*) 
                       FROM ChamCong
                       WHERE MaNV = @MaNV 
                         AND Ngay >= @Ngay AND Ngay < DATEADD(DAY,1,@Ngay)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Ngay", ngay.Date);

                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        protected void rptNhanVien_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int ngayChon = 0;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptNgayCong = (Repeater)e.Item.FindControl("rptNgayCong");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string maNV = drv["MaNV"].ToString();

                int thang = int.Parse(ddlThang.SelectedValue);
                int nam = int.Parse(ddlNam.SelectedValue);
                SoNgayTrongThang = DateTime.DaysInMonth(nam, thang);

                DataTable dtNgay = new DataTable();
                dtNgay.Columns.Add("Ngay", typeof(int));
                dtNgay.Columns.Add("GiaTriCong");
                dtNgay.Columns.Add("CssClassName");

                for (int i = 1; i <= SoNgayTrongThang; i++)
                {
                    DateTime ngayHienTai = new DateTime(nam, thang, i);

                    // Logic xác định Class dựa trên loại ngày
                    string currentClass = "bg-normal";

                    if (IsNgayLe(ngayHienTai)) // Hàm kiểm tra ngày lễ của bạn
                    {
                        currentClass = "bg-holiday";
                    }
                    else if (ngayHienTai.DayOfWeek == DayOfWeek.Sunday)
                    {
                        currentClass = "bg-sunday";
                    }

                    // Lấy giá trị công từ DB
                    string cong = TimGiaTriCongTuDatabase(maNV, ngayHienTai);

                    dtNgay.Rows.Add(i, cong, currentClass);

                }


                rptNgayCong.DataSource = dtNgay;
                rptNgayCong.DataBind();

               
                int.TryParse(ddlNgay.SelectedValue, out ngayChon);

                foreach (RepeaterItem itemNgay in rptNgayCong.Items)
                {
                    TextBox txtCong = (TextBox)itemNgay.FindControl("txtCong");

                    if (ngayChon > 0)
                    {
                        int ngay = itemNgay.ItemIndex + 1;

                        if (ngay != ngayChon)
                        {
                            txtCong.Enabled = false;
                            txtCong.CssClass += " disabled-day";
                        }
                        else
                        {
                            txtCong.Enabled = true;
                            txtCong.CssClass = txtCong.CssClass.Replace("disabled-day", "");
                        }
                    }
                }


            }

        }

        private string TimGiaTriCongTuDatabase(string maNV, DateTime ngay)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Tên bảng đúng là Bangcong_nhanvien_chitiet
                // Cột lưu giá trị công là Cong
                string sql = "SELECT Cong FROM Bangcong_nhanvien_chitiet WHERE MaNV = @MaNV AND Ngay = @Ngay AND TrangThai = 1";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Ngay", ngay.Date); // Chỉ lấy phần Ngày để so khớp

                conn.Open();
                object result = cmd.ExecuteScalar();

                // Trả về giá trị công, nếu trống thì trả về 0
                return (result != null && result != DBNull.Value) ? result.ToString() : "0";
            }
        }
        protected void ddlPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 1. Lấy ID phòng ban được chọn
            int idBP = int.Parse(ddlPhongBan.SelectedValue);

            // 2. Lấy thông tin tháng năm hiện tại để vẽ lại Header
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);

            // 3. QUAN TRỌNG: Vẽ lại tiêu đề ngày trước
            LoadHeaderNgay(thang, nam);

            LoadNhanVienVaoDropDownList(idBP);
            ThucThiLocDuLieu();
            // 4. Lọc danh sách nhân viên theo phòng ban
            DataTable dt = LayDanhSachNhanVien(idBP);
            rptNhanVien.DataSource = dt;
            rptNhanVien.DataBind();
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
        protected void btnUserChamCong_Click(object sender, EventArgs e)
        {
            string maNV = Session["MaNV"].ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                // ❗ Chỉ chặn chấm trùng ngày
                SqlCommand check = new SqlCommand(@"
SELECT 1
FROM Bangcong_nhanvien_chitiet
WHERE MaNV = @MaNV
  AND CAST(Ngay AS DATE) = CAST(GETDATE() AS DATE)", conn);

                check.Parameters.AddWithValue("@MaNV", maNV);

                if (check.ExecuteScalar() != null)
                    return;


                SqlCommand cmd = new SqlCommand(@"
INSERT INTO Bangcong_nhanvien_chitiet
(MaNV, Ngay, Cong, TrangThai, GhiChu)
VALUES
(@MaNV, GETDATE(), 1, 0, N'Nhân viên chấm công')", conn);

                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.ExecuteNonQuery();
            }
        }


//        private void CapNhatTrangThaiNutChamCong()
//        {
//            if (!IsUser()) return;

//            string maNV = Session["MaNV"].ToString();

//            int ngay = int.Parse(ddlNgay.SelectedValue);
//            int thang = int.Parse(ddlThang.SelectedValue);
//            int nam = int.Parse(ddlNam.SelectedValue);

//            DateTime ngayDangXem = new DateTime(nam, thang, ngay);

//            // ❌ Không phải hôm nay → không cho chấm
//            if (ngayDangXem.Date != DateTime.Today)
//            {
//                btnUserChamCong.Visible = false;
//                return;
//            }

//            using (SqlConnection conn = new SqlConnection(connStr))
//            {
//                conn.Open();

//                SqlCommand cmd = new SqlCommand(@"
//SELECT 1
//FROM Bangcong_nhanvien_chitiet
//WHERE MaNV = @MaNV
//  AND CAST(Ngay AS DATE) = CAST(GETDATE() AS DATE)", conn);

//                cmd.Parameters.AddWithValue("@MaNV", maNV);

//                object rs = cmd.ExecuteScalar();

//                // 🔴 CHƯA CHẤM
//                if ((int)rs == 0)
//                {
//                    btnUserChamCong.Visible = true;
//                    btnUserChamCong.Text = "🟢 Chấm công hôm nay";
//                    btnUserChamCong.Enabled = true;
//                }
//                // 🟢 ĐÃ CHẤM (duyệt hay chưa kệ admin)
//                else
//                {
//                    btnUserChamCong.Visible = true;
//                    btnUserChamCong.Text = "⏳ Đã chấm công";
//                    btnUserChamCong.Enabled = false;
//                }
//            }
//        }


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
                notify.Visible = false;
                ddlPhongBan.Enabled = false;
                ddlNhanVien.Enabled = false;
                
            }
        }



        void LoadNgay(int thang, int nam)
        {
            ddlNgay.Items.Clear();
            int soNgay = DateTime.DaysInMonth(nam, thang);

            for (int d = 1; d <= soNgay; d++)
            {
                ddlNgay.Items.Add(new ListItem("Ngày " + d, d.ToString()));
            }

            // Cập nhật biến toàn cục để Header Repeater hiển thị đúng số cột
            SoNgayTrongThang = soNgay;

            // Tránh lỗi khi chọn ngày: Ưu tiên chọn ngày 1 nếu ngày hiện tại không hợp lệ
            int ngayHT = DateTime.Now.Day;
            if (ngayHT <= soNgay)
                ddlNgay.SelectedValue = ngayHT.ToString();
            else
                ddlNgay.SelectedValue = "1";
         
        }

    
        protected void btnChamCongAll_Click(object sender, EventArgs e)
        {
            try
            {
                int thang = int.Parse(ddlThang.SelectedValue);
                int nam = int.Parse(ddlNam.SelectedValue);
                int ngay = int.Parse(ddlNgay.SelectedValue);
                DateTime ngayCham = new DateTime(nam, thang, ngay);
                SoNgayTrongThang = DateTime.DaysInMonth(nam, thang);

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
                            int loaiNgay = (ngayCham.DayOfWeek == DayOfWeek.Sunday) ? 2 : (IsNgayLe(ngayCham) ? 3 : 1);

                            // Truyền đủ tham số theo thứ tự: (conn, maNV, ngay, cong, ghiChu, idLoaiCa, tran)
                            int trangThai = IsUser() ? 0 : 1;
                            SaveChamCong(conn, maNV, ngayCham, cong, "Đi làm", loaiNgay, tran, trangThai);

                        }

                        tran.Commit(); // Xác nhận hoàn tất

                    
                        // 2. Gọi hàm nạp dữ liệu (Ví dụ tên hàm của bạn là ThucThiLocDuLieu hoặc LoadData)
                        ThucThiLocDuLieu();

                        // 3. Tính toán lại tổng công để hiển thị ngay con số vừa chấm
                        TinhTongCongTungNhanVien();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback(); // Hủy bỏ nếu có bất kỳ lỗi nào xảy ra
                        throw ex;
                    }
                }

                // 🔥 RESET GIAO DIỆN
                //if (!IsUser())
                //{
                //    ddlNhanVien.SelectedIndex = 0;
                //    rptNhanVien.DataSource = null;
                //    rptNhanVien.DataBind();
                //    lblTongCong.Text = "0";
                //}

                ScriptManager.RegisterStartupScript(this, GetType(), "ok",
                    $"alert('Đã chấm {cong} công cho nhân viên ngày {ngay:00}/{thang:00}/{nam}!');", true);
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
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    foreach (RepeaterItem itemNV in rptNhanVien.Items)
                    {
                        // Lấy MaNV từ HiddenField đã thêm vào .aspx
                        string maNV = ((HiddenField)itemNV.FindControl("hfMaNV")).Value;
                        Repeater rptNgayCong = (Repeater)itemNV.FindControl("rptNgayCong");
                        TextBox txtGhiChu = (TextBox)itemNV.FindControl("txtGhiChu");

                        double tongCongNhanVien = 0; // Biến tạm để tính tổng

                        for (int i = 0; i < rptNgayCong.Items.Count; i++)
                        {
                            RepeaterItem itemNgay = rptNgayCong.Items[i];
                            TextBox txtCong = (TextBox)itemNgay.FindControl("txtCong");

                            int ngay = i + 1;
                            DateTime ngayFull = new DateTime(nam, thang, ngay);

                            // Nếu ô trống thì mặc định là 0 để không lỗi
                            string giaTriNhap = string.IsNullOrEmpty(txtCong.Text) ? "0" : txtCong.Text;

                            if (double.TryParse(giaTriNhap, NumberStyles.Any, CultureInfo.InvariantCulture, out double giaTriCong))
                            {
                                // Lưu từng ngày vào DB
                                SaveChamCong(conn, maNV, ngayFull, giaTriCong, txtGhiChu.Text, 1, transaction);
                                tongCongNhanVien += giaTriCong;
                            }
                        }

                        // Cập nhật hiển thị Tổng công tạm thời trên giao diện
                        Label lblTong = (Label)itemNV.FindControl("lblTong");
                        if (lblTong != null) lblTong.Text = tongCongNhanVien.ToString();
                    }

                    transaction.Commit();

                    // QUAN TRỌNG: Gọi lại hàm nạp dữ liệu để bảng không bị lệch và cập nhật số liệu mới nhất
                    ThucThiLocDuLieu();

                    ScriptManager.RegisterStartupScript(this, GetType(), "success", "alert('Lưu bảng công thành công!');", true);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ScriptManager.RegisterStartupScript(this, GetType(), "error", $"alert('Lỗi: {ex.Message}');", true);
                }
            }
        }
        private void LoadNotifyCount()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Đếm tổng số bản ghi có TrangThai = 1 trong tháng hiện tại
                string sql = @"
SELECT COUNT(*)
FROM Bangcong_nhanvien_chitiet
WHERE TrangThai = 0
  AND MONTH(Ngay) = MONTH(GETDATE())
  AND YEAR(Ngay) = YEAR(GETDATE())";

                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                ltrCount.Text = cmd.ExecuteScalar().ToString();
            }
        }
        private void LoadThongBao()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
        SELECT bc.MaNV, nv.HoTen, bc.Ngay, bc.GhiChu
        FROM Bangcong_nhanvien_chitiet bc
        JOIN Nhan_vien nv ON bc.MaNV = nv.MaNV
        WHERE bc.TrangThai = 0
        ORDER BY bc.Ngay DESC";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvThongBao.DataSource = dt;
                gvThongBao.DataBind();
            }
        }
        protected void gvThongBao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] data = e.CommandArgument.ToString().Split('|');
            string maNV = data[0];
            DateTime ngay = DateTime.Parse(data[1]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                if (e.CommandName == "DUYET")
                {
                    SqlCommand cmd = new SqlCommand(@"
UPDATE Bangcong_nhanvien_chitiet
SET 
    Cong = 1,
    TrangThai = 1,
    GhiChu = N'Admin duyệt'
WHERE MaNV = @MaNV
  AND Ngay >= @Ngay
  AND Ngay < DATEADD(DAY, 1, @Ngay)", conn);

                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.Add("@Ngay", SqlDbType.Date).Value = ngay.Date;
                    cmd.ExecuteNonQuery();
                }

                if (e.CommandName == "TUCHOI")
                {
                    SqlCommand cmd = new SqlCommand(@"
UPDATE Bangcong_nhanvien_chitiet
SET 
    TrangThai = 0,
    Cong = 0,
    GhiChu = N'Admin từ chối'
WHERE MaNV = @MaNV
  AND Ngay >= @Ngay
  AND Ngay < DATEADD(DAY, 1, @Ngay)", conn);

                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.Add("@Ngay", SqlDbType.Date).Value = ngay.Date;
                    cmd.ExecuteNonQuery();
                }
            }

            // ✅ Load lại TẤT CẢ ở đây
            LoadThongBao();
            LoadNotifyCount();
            ExecuteLoad();
        }

        protected void btnDuyet_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
UPDATE Bangcong_nhanvien_chitiet
SET 
    TrangThai = 1,
    Cong = CASE 
        WHEN DATEPART(WEEKDAY, Ngay) = 1 THEN 2      -- Chủ nhật
        WHEN EXISTS (
            SELECT 1 
            FROM Danh_muc_ngay_le 
            WHERE NgayLe = CAST(Ngay AS DATE)
        ) THEN 3                                     -- Ngày lễ
        ELSE 1                                       -- Ngày thường
    END,
    IDLoaiCa = CASE 
        WHEN DATEPART(WEEKDAY, Ngay) = 1 THEN 2
        WHEN EXISTS (
            SELECT 1 
            FROM Danh_muc_ngay_le 
            WHERE NgayLe = CAST(Ngay AS DATE)
        ) THEN 3
        ELSE 1
    END,
    GhiChu = N'Admin duyệt công'
WHERE TrangThai = 0
  AND MONTH(Ngay) = @Thang
  AND YEAR(Ngay) = @Nam";

                // Có thể thêm điều kiện lọc theo bộ phận tại đây
                SqlCommand cmd = new SqlCommand(sql, conn);
            
                
                cmd.Parameters.AddWithValue("@Thang", int.Parse(ddlThang.SelectedValue));
                cmd.Parameters.AddWithValue("@Nam", int.Parse(ddlNam.SelectedValue));
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            LoadNotifyCount(); // Cập nhật lại số chuông
            ExecuteLoad();     // Tải lại bảng dữ liệu
        }

        private void UpdateLabelTongCongThang(string maNV, int thang, int nam)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT SUM(Cong) FROM Bangcong_nhanvien_chitiet\r\nWHERE TrangThai = 1\r\n  AND MONTH(Ngay) = @Thang\r\n  AND YEAR(Ngay) = @Nam";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);
                conn.Open();
                object result = cmd.ExecuteScalar();
              
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
        private void SaveChamCong(SqlConnection conn, string maNV, DateTime ngay, double cong, string ghiChu, int idLoaiCa, SqlTransaction tran = null, int trangThai = 1)
        {
            // Thêm TrangThai vào câu lệnh SQL
            string sql = @"
IF EXISTS (SELECT 1 FROM Bangcong_nhanvien_chitiet WHERE MaNV = @MaNV AND CAST(Ngay AS DATE) = @Ngay)
BEGIN
    UPDATE Bangcong_nhanvien_chitiet 
    SET Cong = @Cong, GhiChu = @GhiChu, IDLoaiCa = @IDLoaiCa, TrangThai = @TrangThai
    WHERE MaNV = @MaNV AND CAST(Ngay AS DATE) = @Ngay
END
ELSE
BEGIN
    INSERT INTO Bangcong_nhanvien_chitiet (MaNV, Ngay, Cong, GhiChu, IDLoaiCa, TrangThai) 
    VALUES (@MaNV, @Ngay, @Cong, @GhiChu, @IDLoaiCa, @TrangThai)
END";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (tran != null) cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Ngay", ngay.Date);
                cmd.Parameters.AddWithValue("@Cong", cong);
                cmd.Parameters.AddWithValue("@GhiChu", ghiChu ?? "");
                cmd.Parameters.AddWithValue("@IDLoaiCa", idLoaiCa);
                cmd.Parameters.AddWithValue("@TrangThai", trangThai); // Lưu trạng thái duyệt
                cmd.ExecuteNonQuery();
            }
        }
        private void TinhTongCongTungNhanVien()
        {
            foreach (RepeaterItem itemNV in rptNhanVien.Items)
            {
                string maNV = ((HiddenField)itemNV.FindControl("hfMaNV")).Value;
                Label lblTong = (Label)itemNV.FindControl("lblTong");

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = @"
                SELECT ISNULL(SUM(Cong),0)
                FROM Bangcong_nhanvien_chitiet
                WHERE MaNV = @MaNV
                  AND TrangThai = 1
                  AND MONTH(Ngay) = @Thang
                  AND YEAR(Ngay) = @Nam";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.AddWithValue("@Thang", int.Parse(ddlThang.SelectedValue));
                    cmd.Parameters.AddWithValue("@Nam", int.Parse(ddlNam.SelectedValue));

                    conn.Open();
                    lblTong.Text = cmd.ExecuteScalar().ToString();
                }
            }
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

           
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // SQL phải lấy đủ HoTen và TenChucVu như trong file .aspx yêu cầu
                string sql = @"
            SELECT nv.MaNV, nv.HoTen, cv.TenCV as TenChucVu 
            FROM Nhan_vien nv
            LEFT JOIN Chuc_vu cv ON nv.IDCV = cv.IDCV";

                if (maNV > 0) sql += " WHERE nv.MaNV = @MaNV";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                if (maNV > 0) da.SelectCommand.Parameters.AddWithValue("@MaNV", maNV);

                DataTable dtNhanVien = new DataTable();
                da.Fill(dtNhanVien);

                // Gán vào Repeater cha
                rptNhanVien.DataSource = dtNhanVien;
                rptNhanVien.DataBind();
            }
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
               
            }
        }

        private void LoadChamCongCaThang(int thang, int nam, int maNV)
        {
            // 1. Tạo cấu trúc bảng
            DataTable dt = new DataTable();
            dt.Columns.Add("MaNV", typeof(int));
            dt.Columns.Add("HoTen", typeof(string));
            dt.Columns.Add("Ngay", typeof(DateTime));
            dt.Columns.Add("Thu", typeof(string));
            dt.Columns.Add("Cong", typeof(double));
            dt.Columns.Add("GhiChu", typeof(string));
            dt.Columns.Add("SoGio", typeof(double));

            // 2. Lấy tên nhân viên an toàn
            string hoTen = "N/A";
            if (IsUser())
                hoTen = Session["HoTen"]?.ToString() ?? "Nhân viên";
            else if (ddlNhanVien.SelectedIndex > 0)
                hoTen = ddlNhanVien.SelectedItem.Text;

            // 3. Khởi tạo danh sách ngày trong tháng (Dữ liệu mẫu)
            int soNgay = DateTime.DaysInMonth(nam, thang);
            for (int d = 1; d <= soNgay; d++)
            {
                DateTime ngay = new DateTime(nam, thang, d);
                dt.Rows.Add(maNV, hoTen, ngay, ngay.ToString("dddd"), 0, "", 0);
            }

            // 4. Lấy dữ liệu thực từ SQL
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Sử dụng LEFT JOIN để lấy cả tăng ca trong 1 lần truy vấn
                string sql = @"
            SELECT bc.Ngay, bc.Cong, bc.GhiChu, tc.SoGio
            FROM Bangcong_nhanvien_chitiet bc
            LEFT JOIN Tang_ca tc ON tc.MaNV = bc.MaNV AND CAST(tc.Ngay AS INT) = DAY(bc.Ngay) 
                AND tc.Thang = MONTH(bc.Ngay) AND tc.Nam = YEAR(bc.Ngay)
            WHERE bc.MaNV = @MaNV AND MONTH(bc.Ngay) = @Thang AND YEAR(bc.Ngay) = @Nam";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime ngayDB = Convert.ToDateTime(reader["Ngay"]);
                        // Dùng LINQ tìm dòng nhanh hơn foreach truyền thống
                        DataRow dr = dt.AsEnumerable()
                                       .FirstOrDefault(r => r.Field<DateTime>("Ngay").Date == ngayDB.Date);

                        if (dr != null)
                        {
                            dr["Cong"] = reader["Cong"] ?? 0;
                            dr["GhiChu"] = reader["GhiChu"]?.ToString() ?? "";
                            dr["SoGio"] = reader["SoGio"] != DBNull.Value ? reader["SoGio"] : 0;
                        }
                    }
                }
            }

            // 5. Ràng buộc dữ liệu
            rptNhanVien.DataSource = dt;
            rptNhanVien.DataBind();

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
            // 1. Kiểm tra tháng năm
            if (!int.TryParse(ddlThang.SelectedValue, out int thang) ||
                !int.TryParse(ddlNam.SelectedValue, out int nam))
                return;

            // 2. Luôn vẽ lại header ngày
            SoNgayTrongThang = DateTime.DaysInMonth(nam, thang);
            LoadHeaderNgay(thang, nam);

            DataTable dt = null;

            // ===============================
            // 3. PHÂN LUỒNG LOAD DỮ LIỆU
            // ===============================

            if (IsUser())
            {
                // 🔒 USER: chỉ xem chính mình
                if (Session["MaNV"] == null)
                {
                    Response.Redirect("DangNhap.aspx");
                    return;
                }

                string maNV = Session["MaNV"].ToString();
                dt = LayDanhSachNhanVien(0, maNV);
            }
            else
            {
                // 🔓 ADMIN
                string maNVChon = ddlNhanVien.SelectedValue;

                if (!string.IsNullOrEmpty(maNVChon) && maNVChon != "0")
                {
                    // 👉 ADMIN chọn 1 nhân viên
                    dt = LayDanhSachMotNhanVien(maNVChon);
                }
                else
                {
                    // 👉 ADMIN chưa chọn nhân viên → lọc theo phòng ban
                    int.TryParse(ddlPhongBan.SelectedValue, out int idBP);
                    dt = LayDanhSachNhanVien(idBP);
                }
            }

            // ===============================
            // 4. ĐỔ DỮ LIỆU
            // ===============================
            rptNhanVien.DataSource = dt;
            rptNhanVien.DataBind();

            // 5. Tính tổng công
            TinhTongCongTungNhanVien();

            // 6. Quyền lưu
            btnSave.Visible = !IsUser();
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            ExecuteLoad();
        }
        protected void ddlThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            CapNhatGiaoDienTheoThangNam();
        }

        protected void ddlNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            CapNhatGiaoDienTheoThangNam();
        }
        private void CapNhatGiaoDienTheoThangNam()
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);

            LoadNgay(thang, nam);
            // 1. Vẽ lại tiêu đề 28, 29, 30 hoặc 31 ngày
            LoadHeaderNgay(thang, nam);

            // 2. Nạp lại danh sách ngày vào ddlNgay (Nếu cần chọn theo ngày)
            LoadDanhSachNgayTrongThang(thang, nam);

            // 3. Tải lại dữ liệu chấm công
            ExecuteLoad();
        }
        private void LoadDanhSachNgayTrongThang(int thang, int nam)
        {
            ddlNgay.Items.Clear();
            int soNgay = DateTime.DaysInMonth(nam, thang);
            for (int i = 1; i <= soNgay; i++)
            {
                ddlNgay.Items.Add(new ListItem("Ngày " + i, i.ToString()));
            }
        }
        protected void ddlNgay_SelectedIndexChanged(object sender, EventArgs e)
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);

            // 1. LUÔN giữ đủ số ngày
            SoNgayTrongThang = DateTime.DaysInMonth(nam, thang);
            LoadHeaderNgay(thang, nam);

            // 2. Reload dữ liệu
            ExecuteLoad();
        }

        private void LoadNhanVienVaoDropDownList(int idPhongBan = 0)
        {
            // 1. Xóa dữ liệu cũ và thêm dòng mặc định
            ddlNhanVien.Items.Clear();
            ddlNhanVien.Items.Add(new ListItem("-- Chọn Nhân Viên --", "0"));

            // 2. Lấy dữ liệu từ hàm LayDanhSachNhanVien bạn đã viết
            DataTable dt = LayDanhSachNhanVien(idPhongBan);

            // 3. Duyệt bảng dữ liệu để đưa tên vào DropDownList
            foreach (DataRow dr in dt.Rows)
            {
                string hoTen = dr["HoTen"].ToString();
                string maNV = dr["MaNV"].ToString();

                // ListItem(Text, Value) -> Text hiện tên, Value giữ mã số
                ddlNhanVien.Items.Add(new ListItem(hoTen, maNV));
            }
        }
        private void LoadNhanVienVaoDdl(int idBP = 0)
        {
            // 1. Lấy dữ liệu (Sử dụng hàm LayDanhSachNhanVien đã sửa JOIN trước đó)
            DataTable dt = LayDanhSachNhanVien(idBP);

            ddlNhanVien.Items.Clear();
            ddlNhanVien.Items.Add(new ListItem("-- Chọn Nhân Viên --", "0"));

            // 2. Điền dữ liệu vào DropDownList
            ddlNhanVien.DataSource = dt;
            ddlNhanVien.DataTextField = "HoTen"; // Tên hiển thị cho người dùng chọn
            ddlNhanVien.DataValueField = "MaNV"; // Giá trị ẩn bên dưới để code xử lý (ID)
            ddlNhanVien.DataBind();
        }
        protected void ddlNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maNV = ddlNhanVien.SelectedValue;

            if (maNV == "0" || string.IsNullOrEmpty(maNV))
            {
                // Nếu không chọn ai, hiện lại tất cả hoặc xóa trắng tùy bạn
                ExecuteLoad();
            }
            else
            {
                int thang = int.Parse(ddlThang.SelectedValue);
                int nam = int.Parse(ddlNam.SelectedValue);

                // 1. QUAN TRỌNG: Vẽ lại tiêu đề ngày (Header) để bảng không bị trắng
                LoadHeaderNgay(thang, nam);

                // 2. Nạp dữ liệu của RIÊNG nhân viên này vào Repeater
                // Bạn cần sửa hàm LayDanhSachNhanVien để nhận thêm MaNV nếu cần lọc sâu
                DataTable dtMotNhanVien = LayDanhSachMotNhanVien(maNV);
                rptNhanVien.DataSource = dtMotNhanVien;
                rptNhanVien.DataBind();

                btnSave.Visible = true;
            }
        }
        private DataTable LayDanhSachMotNhanVien(string maNV)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Bạn phải đảm bảo SELECT có đầy đủ: MaNV, HoTen, TenChucVu và GhiChu
                // Nếu bảng Nhan_vien chưa có GhiChu, bạn có thể SELECT '' as GhiChu
                string sql = @"SELECT nv.MaNV, nv.HoTen, cv.TenCV as TenChucVu, 
                       '' as GhiChu 
                       FROM Nhan_vien nv 
                       LEFT JOIN Chuc_vu cv ON nv.IDCV = cv.IDCV 
                       WHERE nv.MaNV = @MaNV";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        private void ThucThiLocDuLieu()
        {
            if (int.TryParse(ddlThang.SelectedValue, out int thang) &&
                int.TryParse(ddlNam.SelectedValue, out int nam))
            {
             
                // Vẽ lại Header ngày
                LoadHeaderNgay(thang, nam);

                // Lấy ID phòng ban từ dropdown và gọi hàm bạn vừa viết
                int idBP = int.Parse(ddlPhongBan.SelectedValue);
                DataTable dt = LayDanhSachNhanVien(idBP);

                rptNhanVien.DataSource = dt;
                rptNhanVien.DataBind();

                // Tính tổng công hiển thị
                TinhTongCongTungNhanVien();
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }


        private void TinhTongCongNhanVien()
        {
            foreach (RepeaterItem itemNV in rptNhanVien.Items)
            {
                double tongNhanVien = 0;

                // Tìm Repeater con chứa danh sách các ngày công của nhân viên này
                Repeater rptNgayCong = (Repeater)itemNV.FindControl("rptNgayCong");
                Label lblTong = (Label)itemNV.FindControl("lblTong");

                if (rptNgayCong != null)
                {
                    foreach (RepeaterItem itemNgay in rptNgayCong.Items)
                    {
                        TextBox txtCong = (TextBox)itemNgay.FindControl("txtCong");
                        if (txtCong != null && !string.IsNullOrEmpty(txtCong.Text))
                        {
                            if (double.TryParse(txtCong.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double giaTri))
                            {
                                tongNhanVien += giaTri;
                            }
                        }
                    }
                }

                // Gán giá trị vào cột "Tổng công" của nhân viên đó
                if (lblTong != null)
                {
                    lblTong.Text = tongNhanVien.ToString("0.##");
                }
            }
        }



        protected void btnTongHopCong_Click(object sender, EventArgs e)
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);
            bool isSuccess = false; // Biến đánh dấu thành công

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Xóa dữ liệu tổng hợp cũ
                        SqlCommand del = new SqlCommand("DELETE FROM BangCongThang WHERE Thang = @Thang AND Nam = @Nam", conn, trans);
                        del.Parameters.AddWithValue("@Thang", thang);
                        del.Parameters.AddWithValue("@Nam", nam);
                        del.ExecuteNonQuery();

                        // 2. Chèn dữ liệu tổng hợp mới
                        string sqlInsert = @"
                  INSERT INTO BangCongThang
(MaNV, Thang, Nam, CongNgayThuong, CongChuNhat, CongLe,NgayNghi, TongCong)
SELECT
    bc.MaNV,
    @Thang,
    @Nam,

    -- Công ngày thường
    SUM(CASE
        WHEN nl.NgayLe IS NULL
             AND DATEPART(WEEKDAY, bc.Ngay) <> 1
        THEN bc.Cong ELSE 0 END),

    -- Công chủ nhật
    SUM(CASE
        WHEN nl.NgayLe IS NULL
             AND DATEPART(WEEKDAY, bc.Ngay) = 1
        THEN bc.Cong ELSE 0 END),

    -- Công ngày lễ
    SUM(CASE
        WHEN nl.NgayLe IS NOT NULL
        THEN bc.Cong ELSE 0 END),

 SUM(CASE
        WHEN bc.Cong = 0
        THEN 1 ELSE 0 END),

    -- Tổng công
    SUM(bc.Cong)

FROM Bangcong_nhanvien_chitiet bc
INNER JOIN Hop_dong hd ON bc.MaNV = hd.MaNV
LEFT JOIN Danh_muc_ngay_le nl ON bc.Ngay = nl.NgayLe
WHERE
    MONTH(bc.Ngay) = @Thang
    AND YEAR(bc.Ngay) = @Nam
    AND bc.TrangThai = 1
    AND hd.NgayBatDau <= bc.Ngay
    AND hd.NgayKetThuc >= bc.Ngay
GROUP BY bc.MaNV

                    ";

                        SqlCommand cmd = new SqlCommand(sqlInsert, conn, trans);
                        cmd.Parameters.AddWithValue("@Thang", thang);
                        cmd.Parameters.AddWithValue("@Nam", nam);
                        cmd.ExecuteNonQuery();

                        trans.Commit();
                        isSuccess = true; // Đánh dấu đã xong việc
                    }
                    catch (Exception ex)
                    {
                        // Chỉ Rollback nếu transaction chưa completed
                        if (trans.Connection != null)
                        {
                            trans.Rollback();
                        }
                        ScriptManager.RegisterStartupScript(this, GetType(), "err", $"alert('Lỗi: {ex.Message}');", true);
                    }
                }
            }

            // GỌI CHUYỂN HƯỚNG Ở ĐÂY (NGOÀI USING)
            if (isSuccess)
            {
                Response.Redirect($"TongHopCongThang.aspx?thang={thang}&nam={nam}");
            }
        }
        protected void btnResetCongThang_Click(object sender, EventArgs e)
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

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

                SqlCommand del = new SqlCommand(@"
            DELETE FROM BangCongThang
            WHERE Thang = @Thang AND Nam = @Nam", conn);

                del.Parameters.AddWithValue("@Thang", thang);
                del.Parameters.AddWithValue("@Nam", nam);
                del.ExecuteNonQuery();
            }

            LoadChamCong();

            ScriptManager.RegisterStartupScript(
                this, GetType(), "ok",
                "alert('Đã reset công tháng. Có thể tổng hợp lại!');",
                true);
        }


    }
}
