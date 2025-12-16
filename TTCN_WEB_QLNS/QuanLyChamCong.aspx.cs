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
            if (Session["UserName"] == null || Session["IDROLE"] == null || Session["MaNV"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }
            //lblWelcome.Text = "Xin chào: " + Session["UserName"].ToString();

            if (!IsPostBack)
            {
                LoadDropdown();
                string role = Session["IDROLE"].ToString();

                if (role == "10")
                {

                }
                //else
                //{
                //    menuThongTinNV.Visible = false;
                //}    

            }
        }
        // ====================================
        // LOAD DROPDOWN (Tháng, năm, nhân viên)
        // ====================================
        private void LoadDropdown()
        {
            // Tháng
            for (int i = 1; i <= 12; i++)
                ddlThang.Items.Add(i.ToString());

            // Năm
            for (int y = DateTime.Now.Year - 5; y <= DateTime.Now.Year + 1; y++)
                ddlNam.Items.Add(y.ToString());

            // Nhân viên
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT MaNV, HoTen FROM Nhan_vien", conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlNhanVien.DataSource = dt;
                ddlNhanVien.DataTextField = "HoTen";
                ddlNhanVien.DataValueField = "MaNV";
                ddlNhanVien.DataBind();
            }
        }

        // ====================================
        // SAVE 1 DÒNG CHẤM CÔNG
        // ====================================
        private void SaveRow(SqlConnection conn, int maKyCong, int maNV, DateTime ngay, string thu,
            string gioVao, string gioRa, double cong, string ghiChu)
        {
            string sqlCheck = @"
                SELECT COUNT(*) 
                FROM Bangcong_nhanvien_chitiet
                WHERE MaKyCong = @MaKyCong AND MaNV = @MaNV AND Ngay = @Ngay";

            using (SqlCommand check = new SqlCommand(sqlCheck, conn))
            {
                BuildParam(check, maKyCong, maNV, ngay, thu, gioVao, gioRa, cong, ghiChu);

                int count = (int)check.ExecuteScalar();
                if (count > 0) return; // đã có → bỏ qua
            }

            string sqlInsert = @"
                INSERT INTO Bangcong_nhanvien_chitiet
                (MaKyCong, MaNV, Ngay, Thu, GioVao, GioRa, CongNgayLe, GhiChu)
                VALUES
                (@MaKyCong, @MaNV, @Ngay, @Thu, @GioVao, @GioRa, @CongNgay, @GhiChu)";

            using (SqlCommand cmd = new SqlCommand(sqlInsert, conn))
            {
                BuildParam(cmd, maKyCong, maNV, ngay, thu, gioVao, gioRa, cong, ghiChu);
                cmd.ExecuteNonQuery();
            }
        }

        private void BuildParam(SqlCommand cmd, int maKyCong, int maNV, DateTime ngay, string thu,
            string gioVao, string gioRa, double cong, string ghiChu)
        {
            cmd.Parameters.AddWithValue("@MaKyCong", maKyCong);
            cmd.Parameters.AddWithValue("@MaNV", maNV);
            cmd.Parameters.AddWithValue("@Ngay", ngay);
            cmd.Parameters.AddWithValue("@Thu", thu);
            cmd.Parameters.AddWithValue("@GioVao", gioVao);
            cmd.Parameters.AddWithValue("@GioRa", gioRa);
            cmd.Parameters.AddWithValue("@CongNgay", cong);
            cmd.Parameters.AddWithValue("@GhiChu", ghiChu);
        }

        // ====================================
        // LƯU CHẤM CÔNG
        // ====================================
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    int maKyCong = int.Parse(ddlThang.SelectedValue);
                    int maNV = int.Parse(ddlNhanVien.SelectedValue);

                    foreach (GridViewRow row in gvChamCong.Rows)
                    {
                        string strNgay = row.Cells[0].Text + "/" + ddlNam.SelectedValue;
                        DateTime ngay = DateTime.ParseExact(strNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        TextBox txtVao = row.FindControl("txtGioVao") as TextBox;
                        TextBox txtRa = row.FindControl("txtGioRa") as TextBox;
                        TextBox txtNote = row.FindControl("txtGhiChu") as TextBox;

                        double congNgay = double.Parse(row.Cells[4].Text.Trim());

                        SaveRow(conn, maKyCong, maNV, ngay,
                            row.Cells[1].Text,
                            txtVao.Text.Trim(),
                            txtRa.Text.Trim(),
                            congNgay,
                            txtNote.Text.Trim());
                    }
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "ok",
                    "alert('✔ Chấm công đã được lưu!');", true);
            }
            catch (Exception ex)
            {
                Response.Write("Lỗi: " + ex.Message);
            }
        }

        // ====================================
        // TÍNH CÔNG
        // ====================================
        protected void btnTinhCong_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvChamCong.Rows)
            {
                TextBox vao = row.FindControl("txtGioVao") as TextBox;
                TextBox ra = row.FindControl("txtGioRa") as TextBox;

                row.Cells[4].Text = TinhCongNgay(vao.Text, ra.Text).ToString();
            }
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
        // TẠO BẢNG CHẤM CÔNG
        // ====================================
        private DataTable TaoBangChamCong(int thang, int nam)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Ngay", typeof(string));
            dt.Columns.Add("Thu", typeof(string));
            dt.Columns.Add("GioVao", typeof(string));
            dt.Columns.Add("GioRa", typeof(string));
            dt.Columns.Add("CongNgay", typeof(string));
            dt.Columns.Add("GhiChu", typeof(string));

            int days = DateTime.DaysInMonth(nam, thang);

            for (int d = 1; d <= days; d++)
            {
                DateTime ngay = new DateTime(nam, thang, d);
                dt.Rows.Add(
                    ngay.ToString("dd/MM"),
                    ngay.ToString("dddd", new CultureInfo("vi-VN")),
                    "",
                    "",
                    "0",
                    ""
                );
            }

            return dt;
        }

        // ====================================
        // LOAD GRID
        // ====================================
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            gvChamCong.DataSource = TaoBangChamCong(
                int.Parse(ddlThang.SelectedValue),
                int.Parse(ddlNam.SelectedValue));
            gvChamCong.DataBind();
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

        protected void gvChamCong_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("DangNhap.aspx");
        }
    }
}
