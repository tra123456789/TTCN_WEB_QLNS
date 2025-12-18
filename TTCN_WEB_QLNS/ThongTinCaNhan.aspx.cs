using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace TTCN_WEB_QLNS
{
    public partial class ThongTinCaNhan : System.Web.UI.Page
    {
        private readonly string connStr =  ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                bool isAdmin = Session["IDROLE"]?.ToString() == "1";
                string maNV;

                LoadChucVu();
                LoadBoPhan();

                if (isAdmin)
                {
                    if (Request.QueryString["MaNV"] == null)
                    {
                        Response.Redirect("QuanLyUser.aspx");
                        return;
                    }

                    maNV = Request.QueryString["MaNV"];
                    lblWelcome.Text = "Quản trị viên";
                }
                else
                {
                    if (Session["MaNV"] == null)
                    {
                        Response.Redirect("DangNhap.aspx");
                        return;
                    }

                    maNV = Session["MaNV"].ToString();
                    lblWelcome.Text = "Xin chào, " + Session["UserName"];
                }

                LoadThongTinNhanVien(maNV);
                SetupPermission(isAdmin);
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
        bool IsAdmin()
        {
            return Session["IDROLE"]?.ToString() == "1";
        }

        // ================= LOAD THÔNG TIN =================
        void LoadThongTinNhanVien(string maNV)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                string sql = @"
SELECT 
    nv.HoTen,
    nv.NgaySinh,
    nv.GioiTinh,
    nv.DiaChi,
    nv.TrangThai,
    nv.IDCV,
    nv.IDBP,
    nv.HinhAnh
FROM Nhan_vien nv
WHERE nv.MaNV = @ma";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", maNV);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtHoTen.Text = dr["HoTen"].ToString();
                    txtDiaChi.Text = dr["DiaChi"].ToString();
                    txtNgaySinh.Text = dr["NgaySinh"] == DBNull.Value
                        ? ""
                        : Convert.ToDateTime(dr["NgaySinh"]).ToString("yyyy-MM-dd");

                    ddlGioiTinh.SelectedValue = dr["GioiTinh"] == DBNull.Value
                        ? "true"
                        : (Convert.ToBoolean(dr["GioiTinh"]) ? "true" : "false");


                    ddlTrangThai.SelectedValue = dr["TrangThai"].ToString();
                    // Đọc ảnh từ DB và hiển thị vào div
                    string hinhAnh = dr["HinhAnh"].ToString();
                    if (!string.IsNullOrEmpty(hinhAnh))
                    {
                        avatarBox.Style["background-image"] = $"url('{hinhAnh}')";
                    }
                    else
                    {
                        avatarBox.Style["background-image"] = "url('Images/default-avatar.png')"; // Ảnh mặc định
                    }
                    ddlChucVu.SelectedValue = dr["IDCV"].ToString();
                    ddlBoPhan.SelectedValue = dr["IDBP"].ToString();
                }
            }
        }


        // ================= PHÂN QUYỀN =================
        void SetupPermission(bool isAdmin)
        {
            // khóa toàn bộ
            txtHoTen.ReadOnly = true;
            txtNgaySinh.ReadOnly = true;
            txtDiaChi.ReadOnly = true;
            ddlGioiTinh.Enabled = false;

            ddlChucVu.Enabled = false;
            ddlBoPhan.Enabled = false;
            ddlTrangThai.Enabled = false;

            // hiển thị nút chỉnh sửa
            btnEditUser.Visible = true;
            btnSave.Visible = false;

            // admin mới thấy trạng thái / chức vụ
            if (isAdmin)
            {
                ddlTrangThai.Enabled = false; // chỉ mở khi bấm Edit
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
            string maNV = (Session["IDROLE"]?.ToString() == "1")
                          ? Request.QueryString["MaNV"]
                          : Session["MaNV"].ToString();

            string imagePath = ViewState["TempImagePath"]?.ToString();


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"UPDATE Nhan_vien SET 
                        HoTen = @HoTen, 
                        NgaySinh = @NgaySinh, 
                        GioiTinh = @GioiTinh, 
                        DiaChi = @DiaChi, 
                        IDCV = @IDCV, 
                        IDBP = @IDBP, 
                        TrangThai = @TrangThai";

                // Chỉ update cột HinhAnh nếu người dùng có chọn ảnh mới
                if (!string.IsNullOrEmpty(imagePath))
                {
                    sql += ", HinhAnh = @HinhAnh";
                }

                sql += " WHERE MaNV = @MaNV";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
                cmd.Parameters.AddWithValue("@NgaySinh",
    string.IsNullOrEmpty(txtNgaySinh.Text)
        ? (object)DBNull.Value
        : DateTime.Parse(txtNgaySinh.Text));

                cmd.Parameters.AddWithValue("@GioiTinh", ddlGioiTinh.SelectedValue == "true");
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@IDCV", ddlChucVu.SelectedValue);
                cmd.Parameters.AddWithValue("@IDBP", ddlBoPhan.SelectedValue);
                cmd.Parameters.AddWithValue("@TrangThai", ddlTrangThai.SelectedValue);
                cmd.Parameters.AddWithValue("@MaNV", maNV);

                if (!string.IsNullOrEmpty(imagePath))
                {
                    cmd.Parameters.AddWithValue("@HinhAnh", imagePath);
                }

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    Response.Write("<script>alert('Cập nhật thông tin thành công!');</script>");
                }


            }
        }

        protected void btnEditUser_Click(object sender, EventArgs e)
        {
           
            if (!CoQuyenChinhSua())
            {
                Response.Write("<script>alert('Hiện không được phép chỉnh sửa');</script>");
                return;
            }

            txtHoTen.ReadOnly = false;
            txtNgaySinh.ReadOnly = false;
            txtDiaChi.ReadOnly = false;
            ddlGioiTinh.Enabled = true;


            if (IsAdmin())
            {
                ddlChucVu.Enabled = true;
                ddlBoPhan.Enabled = true;
                ddlTrangThai.Enabled = true;
            }

            btnEditUser.Visible = false;
            btnSave.Visible = true;
    
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
