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

            string sql = @"
        SELECT 
            nv.MaNV,
            nv.HoTen,
            bp.TenBP,
            pb.TenPB,
            pb.LuongCoBan
        FROM Phong_ban pb
        JOIN Bo_phan bp ON bp.IDPB = pb.IDPB
        JOIN Nhan_vien nv ON nv.IDBP = bp.IDBP
        WHERE pb.IDPB = @IDPB
        ORDER BY bp.TenBP, nv.HoTen";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@IDPB", idPB);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    lblPhongBan.Text =
                        "Phòng ban: " + dt.Rows[0]["TenPB"]
                        + " | Lương cơ bản: "
                        + string.Format("{0:N0}", dt.Rows[0]["LuongCoBan"]);
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
