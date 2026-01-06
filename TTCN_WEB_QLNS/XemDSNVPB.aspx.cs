using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class XemDSNVPB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["IDPB"] == null)
                {
                    Response.Redirect("PhongBan.aspx");
                    return;
                }

                string idPB = Request.QueryString["IDPB"];
                LoadNhanVienTheoPhongBan(idPB);
            }
        }
        void LoadNhanVienTheoPhongBan(string idPB)
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            // Truy vấn này sẽ lấy hợp đồng có ngày bắt đầu muộn nhất của mỗi nhân viên
            string sql = @"
        WITH ContractRanking AS (
            SELECT 
                MaNV, 
                LuongCoBan, 
                HeSoLuong,
                NgayBatDau,
                -- Sắp xếp giảm dần theo ngày bắt đầu để lấy hợp đồng mới nhất
                ROW_NUMBER() OVER (PARTITION BY MaNV ORDER BY NgayBatDau DESC) as RankNum
            FROM Hop_dong
        )
        SELECT 
            nv.MaNV,
            nv.HoTen,
            bp.TenBP,
            pb.TenPB,
            cr.LuongCoBan,
            cr.HeSoLuong,
            (cr.LuongCoBan * ISNULL(cr.HeSoLuong, 1)) AS LuongThucLinh
        FROM Phong_ban pb
        JOIN Bo_phan bp ON bp.IDPB = pb.IDPB
        JOIN Nhan_vien nv ON nv.IDBP = bp.IDBP
        -- Chỉ Join với những hợp đồng có RankNum = 1 (mới nhất)
        JOIN ContractRanking cr ON cr.MaNV = nv.MaNV AND cr.RankNum = 1
        WHERE pb.IDPB = @IDPB
    ";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@IDPB", idPB);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    lblPhongBan.Text = "Phòng ban: " + dt.Rows[0]["TenPB"].ToString();
                }

                gvNhanVien.DataSource = dt;
                gvNhanVien.DataBind();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("PhongBan.aspx");
        }
    }
}
