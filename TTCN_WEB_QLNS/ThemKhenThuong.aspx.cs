using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace TTCN_WEB_QLNS
{
    public partial class ThemKhenThuong : Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IDROLE"] == null || Session["IDROLE"].ToString() == "10")
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadNhanVien();
            }
        }

        void LoadNhanVien()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT MaNV, HoTen FROM Nhan_vien WHERE TrangThai = 1";
                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                ddlNhanVien.DataSource = cmd.ExecuteReader();
                ddlNhanVien.DataTextField = "HoTen";
                ddlNhanVien.DataValueField = "MaNV";
                ddlNhanVien.DataBind();
            }
        }

        protected void btnLuu_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
INSERT INTO KhenThuong_KyLuat
(MaNV, Loai, SoKTKL, NoiDung, Ngay)
VALUES
(@MaNV, @Loai, @SoTien, @LyDo, @Ngay)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", ddlNhanVien.SelectedValue);
                cmd.Parameters.AddWithValue("@Loai", ddlLoai.SelectedValue);
                cmd.Parameters.AddWithValue("@SoTien", txtSoTien.Text);
                cmd.Parameters.AddWithValue("@LyDo", txtLyDo.Text);
                cmd.Parameters.AddWithValue("@Ngay", txtNgay.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("KhenThuong.aspx");
        }
    }
}
