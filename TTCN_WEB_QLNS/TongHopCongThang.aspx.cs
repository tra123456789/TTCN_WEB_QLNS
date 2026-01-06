using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TTCN_WEB_QLNS
{
    public partial class TongHopCongThang : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadThangNam();
            }
        }

        private void LoadThangNam()
        {
            for (int i = 1; i <= 12; i++)
                ddlThang.Items.Add(i.ToString());

            for (int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year + 1; i++)
                ddlNam.Items.Add(i.ToString());

            ddlThang.SelectedValue = DateTime.Now.Month.ToString();
            ddlNam.SelectedValue = DateTime.Now.Year.ToString();
        }

        protected void btnXem_Click(object sender, EventArgs e)
        {
            int thang = int.Parse(ddlThang.SelectedValue);
            int nam = int.Parse(ddlNam.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        bc.MaNV,
                        nv.HoTen,
                        bc.Thang,
                        bc.Nam,
                        bc.TongCong
                    FROM BangCongThang bc
                    JOIN Nhan_vien nv ON bc.MaNV = nv.MaNV
                    WHERE bc.Thang = @Thang AND bc.Nam = @Nam
                    ORDER BY nv.HoTen", conn);

                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    lblThongBao.Text = "Chưa có dữ liệu tổng hợp công cho tháng này.";
                    gvTongHopCong.DataSource = null;
                    gvTongHopCong.DataBind();
                }
                else
                {
                    lblThongBao.Text = "";
                    gvTongHopCong.DataSource = dt;
                    gvTongHopCong.DataBind();
                }
            }
        }


    }
}
