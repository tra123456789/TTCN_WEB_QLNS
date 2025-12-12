using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class ThongTinCaNhan : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                // Lấy MaNV từ Session khi user đang đăng nhập
                if (Session["MaNV"] == null)
                {
                    Response.Redirect("DangNhap.aspx");
                    return;
                }

                string maNV = Session["MaNV"].ToString();

                lblWelcome.Text = "Xin chào, " + Session["UserName"].ToString();

                LoadThongTinNhanVien(maNV);
            }
        }

        void LoadThongTinNhanVien(string maNV)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
            SELECT nv.MaNV, nv.HoTen, nv.NgaySinh, nv.GioiTinh, nv.DiaChi,
                   cv.IDCV, pb.IDPB, bh.SoBH, bh.TuThang, bh.DenThang
            FROM Nhan_vien nv
            LEFT JOIN Chuc_vu cv ON nv.IDCV = cv.IDCV
            LEFT JOIN Phong_ban pb ON nv.IDPB = pb.IDPB
            LEFT JOIN Bao_hiem bh ON nv.MaNV = bh.MaNV
            WHERE nv.MaNV = @MaNV";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblMaNV.Text = dr["MaNV"].ToString();
                    lblHoTen.Text = dr["HoTen"].ToString();
                    lblNgaySinh.Text = dr["NgaySinh"].ToString();
                    lblGioiTinh.Text = dr["GioiTinh"].ToString();
                    lblDiaChi.Text = dr["DiaChi"].ToString();
                    lblChucVu.Text = dr["IDCV"].ToString();
                    lblPhongBan.Text = dr["IDPB"].ToString();
                    lblSoBH.Text = dr["SoBH"].ToString();
                }
            }
        }

    }
}