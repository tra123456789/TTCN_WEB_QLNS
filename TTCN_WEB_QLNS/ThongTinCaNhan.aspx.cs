using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace TTCN_WEB_QLNS
{
    public partial class ThongTinCaNhan : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadChucVu();
                LoadBoPhan();
                LoadTrinhDo();
                string maNV = "";
                string role = Session["IDROLE"]?.ToString();

                // 1. Kiểm tra nếu có MaNV trên URL (Dành cho Admin hoặc HR xem nhân viên khác)
                if (Request.QueryString["MaNV"] != null)
                {
                    maNV = Request.QueryString["MaNV"];
                }
                // 2. Nếu không có URL, lấy mã của chính mình (Tự xem hồ sơ cá nhân)
                else if (Session["MaNV"] != null)
                {
                    maNV = Session["MaNV"].ToString();
                }

                // 3. Bảo mật: Nếu là User (10) hoặc Kế toán (12) mà cố tình nhập MaNV khác trên URL
                if (Request.QueryString["MaNV"] != null && (role == "10" || role == "12"))
                {
                    // Nếu không phải Admin/HR mà đòi xem người khác -> Đẩy về xem chính mình
                    maNV = Session["MaNV"].ToString();
                }

                if (!string.IsNullOrEmpty(maNV))
                {
                    lblWelcome.Text = (role == "1") ? "Quản trị viên" : "Xin chào, " + Session["UserName"];
                    LoadThongTinNhanVien(maNV);
                    SetupPermission();
                }
            }
        }

        void LoadChucVu()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT IDCV, TenCV FROM Chuc_vu", conn);
                conn.Open();
                ddlChucVu.DataSource = cmd.ExecuteReader();
                ddlChucVu.DataTextField = "TenCV";
                ddlChucVu.DataValueField = "IDCV";
                ddlChucVu.DataBind();
            }
        }

        void LoadTrinhDo()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT IDTD, TenTD FROM Trinh_do", conn);
                conn.Open();
                ddlTrinhDo.DataSource = cmd.ExecuteReader();
                ddlTrinhDo.DataTextField = "TenTD";
                ddlTrinhDo.DataValueField = "IDTD";
                ddlTrinhDo.DataBind();
            }
        }
        void LoadBoPhan()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT IDBP, TenBP FROM Bo_phan", conn);
                conn.Open();
                ddlBoPhan.DataSource = cmd.ExecuteReader();
                ddlBoPhan.DataTextField = "TenBP";
                ddlBoPhan.DataValueField = "IDBP";
                ddlBoPhan.DataBind();
            }
        }
        bool IsHR() => Session["IDROLE"]?.ToString() == "11";
        bool IsKeToan() => Session["IDROLE"]?.ToString() == "12";
        bool IsUser() => Session["IDROLE"]?.ToString() == "10";

        bool IsAdmin()
        {
            return Session["IDROLE"]?.ToString() == "1";
        }

        // ================= LOAD THÔNG TIN =================
        void LoadThongTinNhanVien(string maNV)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // 1️⃣ Thêm nv.IDTD vào câu lệnh SELECT
                string sql = @"
            SELECT 
                nv.HoTen, nv.NgaySinh, nv.GioiTinh, nv.DiaChi, 
                nv.TrangThai, nv.IDCV, nv.IDBP, nv.IDTD, nv.HinhAnh
            FROM Nhan_vien nv
            WHERE nv.MaNV = @ma";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", maNV);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // --- GÁN DỮ LIỆU CƠ BẢN ---
                    txtHoTen.Text = dr["HoTen"].ToString();
                    txtDiaChi.Text = dr["DiaChi"].ToString();

                    // Xử lý Ngày sinh (Tránh lỗi null)
                    txtNgaySinh.Text = dr["NgaySinh"] == DBNull.Value
                        ? ""
                        : Convert.ToDateTime(dr["NgaySinh"]).ToString("yyyy-MM-dd");

                    // Xử lý Giới tính
                    ddlGioiTinh.SelectedValue = dr["GioiTinh"] == DBNull.Value
                        ? "true"
                        : (Convert.ToBoolean(dr["GioiTinh"]) ? "true" : "false");

                    ddlTrangThai.SelectedValue = dr["TrangThai"].ToString();

                    // Xử lý Hình ảnh
                    string hinhAnh = dr["HinhAnh"].ToString();
                    avatarBox.Style["background-image"] = !string.IsNullOrEmpty(hinhAnh)
                        ? $"url('{hinhAnh}')"
                        : "url('Images/default-avatar.png')";

                    // --- GÁN CÁC DROP DOWN LIST (Chức vụ, Bộ phận, Trình độ) ---
                    ddlChucVu.SelectedValue = dr["IDCV"].ToString();
                    ddlBoPhan.SelectedValue = dr["IDBP"].ToString();

                    // 2️⃣ Gán giá trị Trình độ
                    if (dr["IDTD"] != DBNull.Value)
                    {
                        ddlTrinhDo.SelectedValue = dr["IDTD"].ToString();
                    }

                    // --- QUAN TRỌNG: ĐÓNG dr TRƯỚC KHI MỞ drPC ---
                    dr.Close();

                    // --- TRUY VẤN PHỤ CẤP ---
                    string sqlPC = "SELECT IDPC, SoTien FROM NhanVien_PhuCap WHERE MaNV = @ma";
                    SqlCommand cmdPC = new SqlCommand(sqlPC, conn);
                    cmdPC.Parameters.AddWithValue("@ma", maNV);

                    txtPCTrachNhiem.Text = txtPCDocHai.Text = txtPCKhac.Text = "0";

                    SqlDataReader drPC = cmdPC.ExecuteReader();
                    while (drPC.Read())
                    {
                        int id = Convert.ToInt32(drPC["IDPC"]);
                        string tien = drPC["SoTien"].ToString();

                        if (id == 1) txtPCTrachNhiem.Text = tien;
                        else if (id == 2) txtPCDocHai.Text = tien;
                        else if (id == 3) txtPCKhac.Text = tien;
                    }
                    drPC.Close();
                }
                else
                {
                    dr.Close();
                }
            }
        }
        private void ThucThiLuuPhuCap(SqlConnection conn, string maNV, int idpc, string giaTri)
        {
            double soTien = 0;
            if (double.TryParse(giaTri, out soTien) && soTien >= 0)
            {
                // Kiểm tra xem đã có dòng này chưa, nếu có thì Update, chưa thì Insert
                string sql = @"IF EXISTS (SELECT 1 FROM NhanVien_PhuCap WHERE MaNV=@MaNV AND IDPC=@IDPC)
                       UPDATE NhanVien_PhuCap SET SoTien=@SoTien, Ngay=@Ngay WHERE MaNV=@MaNV AND IDPC=@IDPC
                       ELSE
                       INSERT INTO NhanVien_PhuCap (MaNV, IDPC, Ngay, NoiDung, SoTien) 
                       VALUES (@MaNV, @IDPC, @Ngay, @NoiDung, @SoTien)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.AddWithValue("@IDPC", idpc);
                    cmd.Parameters.AddWithValue("@SoTien", soTien); // Kiểu float trong DB tương ứng double/float C#
                    cmd.Parameters.AddWithValue("@Ngay", DateTime.Now.Day); // Lưu ngày hiện tại (kiểu int)
                    cmd.Parameters.AddWithValue("@NoiDung", "Cập nhật từ hồ sơ");
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // ================= PHÂN QUYỀN =================
        void SetupPermission()
        {
            string role = Session["IDROLE"]?.ToString();

            // --- BƯỚC 1: MẶC ĐỊNH KHÓA TẤT CẢ ---
            txtHoTen.ReadOnly = txtNgaySinh.ReadOnly = txtDiaChi.ReadOnly = true;
            txtPCTrachNhiem.ReadOnly = txtPCDocHai.ReadOnly = txtPCKhac.ReadOnly = true;
           
            ddlGioiTinh.Enabled = ddlChucVu.Enabled = ddlBoPhan.Enabled = ddlTrangThai.Enabled = ddlTrinhDo.Enabled = false;
           btnPreview.Visible= fuAvatar.Visible= btnEditUser.Visible = btnSave.Visible =  false;

            // --- BƯỚC 2: PHÂN QUYỀN HIỂN THỊ PHỤ CẤP ---
            // Giả sử Admin (1) và Kế toán (12) được xem bảng phụ cấp
            if (role == "1" || role == "12")
            {
                divPhuCap.Visible = true;
                
            }
            else
            {
                divPhuCap.Visible = false; // Nhân viên (10) và HR (11) không thấy tiền
                btnPreview.Visible = false;
                fuAvatar.Visible = false;
            }

            // --- BƯỚC 3: PHÂN QUYỀN CHỈNH SỬA (Nút Edit) ---
            // Admin (1) và HR (11) được quyền sửa hồ sơ
            if (role == "1" || role == "11")
            {
                btnEditUser.Visible = true;
            }
        }



        bool CoQuyenChinhSua()
        {
            bool isAdmin = Session["IDROLE"]?.ToString() == "1";
            if (isAdmin) return true;

            return DuocPhepChinhSua(); // admin mở thời gian
        }

        // ================= SAVE =================
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CoQuyenChinhSua())
            {
                Response.Write("<script>alert('Đã hết thời gian cho phép chỉnh sửa');</script>");
                return;
            }

            string maNV = Request.QueryString["MaNV"] ?? Session["MaNV"]?.ToString();
            if (string.IsNullOrEmpty(maNV)) return;

            // Xử lý ảnh
            bool coAnhMoi = fuAvatar.HasFile;
            string imageUrl = null;

            if (coAnhMoi)
            {
                // Trường hợp 1: Người dùng chọn file rồi nhấn Lưu luôn (không qua Preview)
                string ext = Path.GetExtension(fuAvatar.FileName);
                string fileName = Guid.NewGuid() + ext;
                string folderPath = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                fuAvatar.SaveAs(Path.Combine(folderPath, fileName));
                imageUrl = "Uploads/" + fileName;
            }
            else if (ViewState["TempImagePath"] != null)
            {
                // Trường hợp 2: Người dùng đã nhấn "Xem trước", lấy ảnh từ ViewState
                imageUrl = ViewState["TempImagePath"].ToString();
                coAnhMoi = true; // Đánh dấu là có ảnh để SQL thêm vào câu lệnh Update
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // BƯỚC 1: MỞ KẾT NỐI DUY NHẤT 1 LẦN TẠI ĐÂY
                conn.Open();

                // BƯỚC 2: GỌI LƯU PHỤ CẤP (Vì hàm này dùng chung conn đang Open)
                ThucThiLuuPhuCap(conn, maNV, 1, txtPCTrachNhiem.Text);
                ThucThiLuuPhuCap(conn, maNV, 2, txtPCDocHai.Text);
                ThucThiLuuPhuCap(conn, maNV, 3, txtPCKhac.Text);

                // BƯỚC 3: CẬP NHẬT THÔNG TIN NHÂN VIÊN
                string sql = @"UPDATE Nhan_vien SET 
                        HoTen = @HoTen,
                        NgaySinh = @NgaySinh,
                        GioiTinh = @GioiTinh,
                        DiaChi = @DiaChi,
                        IDCV = @IDCV,
                        IDBP = @IDBP,
                        IDTD = @IDTD,
                        TrangThai = @TrangThai"
                                + (coAnhMoi ? ", HinhAnh = @HinhAnh" : "") +
                                " WHERE MaNV = @MaNV";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
                cmd.Parameters.AddWithValue("@NgaySinh", string.IsNullOrEmpty(txtNgaySinh.Text) ? (object)DBNull.Value : DateTime.Parse(txtNgaySinh.Text));
                cmd.Parameters.AddWithValue("@GioiTinh", ddlGioiTinh.SelectedValue == "true");
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@IDCV", ddlChucVu.SelectedValue);
                cmd.Parameters.AddWithValue("@IDBP", ddlBoPhan.SelectedValue);
                cmd.Parameters.AddWithValue("@IDTD", ddlTrinhDo.SelectedValue);
                cmd.Parameters.AddWithValue("@TrangThai", ddlTrangThai.SelectedValue);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                if (coAnhMoi) cmd.Parameters.AddWithValue("@HinhAnh", imageUrl);

                // KHÔNG gọi conn.Open() ở đây nữa vì đã mở ở Bước 1
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    Response.Write("<script>alert('Cập nhật thông tin và phụ cấp thành công!');</script>");
                }

                // Khóa lại giao diện sau khi lưu
                SetupPermission();
                btnSave.Visible = false;
                btnEditUser.Visible = true;
            }
        }

        protected void btnEditUser_Click(object sender, EventArgs e)
        {
            // ❌ Không có quyền chỉnh sửa
            if (!(IsAdmin() || IsHR()))
            {
                Response.Write("<script>alert('Bạn không có quyền chỉnh sửa thông tin này');</script>");
                return;
            }

            // ✅ HR + Admin đều sửa được thông tin cơ bản
            txtHoTen.ReadOnly = false;
            txtNgaySinh.ReadOnly = false;
            txtDiaChi.ReadOnly = false;
            ddlGioiTinh.Enabled = true;
            

            // ❌ Mặc định KHÔNG cho đổi chức vụ
            ddlChucVu.Enabled = false;
            ddlBoPhan.Enabled = false;
            ddlTrangThai.Enabled = false;
            ddlTrinhDo.Enabled = false;

            // 👑 Chỉ ADMIN mới được đổi chức vụ / bộ phận / trạng thái
            if (!(IsAdmin() || IsHR()))
            {
                Response.Write("<script>alert('Bạn không có quyền chỉnh sửa thông tin này');</script>");
                return;
            }

            // Mở khóa thông tin cơ bản
            txtHoTen.ReadOnly = false;
            txtNgaySinh.ReadOnly = false;
            txtDiaChi.ReadOnly = false;
            ddlGioiTinh.Enabled = true;
            

            // --- THÊM 3 DÒNG NÀY ĐỂ SỬA ĐƯỢC PHỤ CẤP ---
            txtPCTrachNhiem.ReadOnly = false;
            txtPCDocHai.ReadOnly = false;
            txtPCKhac.ReadOnly = false;
            // ------------------------------------------

            ddlTrinhDo.Enabled = ddlChucVu.Enabled = ddlBoPhan.Enabled = ddlTrangThai.Enabled = IsAdmin();

            btnEditUser.Visible = false;
            btnSave.Visible = true;
            btnPreview.Visible = true;
            fuAvatar.Visible = true;
        }


        protected void btnPreview_Click(object sender, EventArgs e)
        {
            if (fuAvatar.HasFile)
            {
                try
                {
                    string ext = Path.GetExtension(fuAvatar.FileName);
                    string fileName = Guid.NewGuid() + ext;
                    string folderPath = Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                    string filePath = folderPath + fileName;
                    fuAvatar.SaveAs(filePath);

                    // Cập nhật giao diện
                    string imageUrl = "Uploads/" + fileName;
                    avatarBox.Style["background-image"] = $"url('{imageUrl}')";

                    // Lưu đường dẫn vào ViewState để btnSave có thể sử dụng lại nếu không chọn file mới
                    ViewState["TempImagePath"] = imageUrl;
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Lỗi xem trước: " + ex.Message + "');</script>");
                }
            }
        }
        bool DuocPhepChinhSua()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT ThoiGianMo, ThoiGianDong FROM CauHinhSuaThongTin";
                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    DateTime mo = Convert.ToDateTime(dr["ThoiGianMo"]);
                    DateTime dong = Convert.ToDateTime(dr["ThoiGianDong"]);
                    DateTime now = DateTime.Now;

                    return now >= mo && now <= dong;
                }
            }
            return false;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (IsAdmin())
            {
                Response.Redirect("QuanLyUser.aspx"); // admin
            }
            else
            {
                Response.Redirect("UserHome.aspx"); // user thường
            }
        }


    }
}
