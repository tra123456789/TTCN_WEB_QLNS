using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class KhenThuong : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDataKhenThuong();
            }
        }

        private void LoadDataKhenThuong()
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM KhenThuong_KyLuat", conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvKhenThuong.DataSource = dt;
                gvKhenThuong.DataBind();
            }
        }

        protected void gvKhenThuong_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvKhenThuong.PageIndex = e.NewPageIndex;
            LoadDataKhenThuong();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvKhenThuong.PageSize = int.Parse(ddlPageSize.SelectedValue);
            LoadDataKhenThuong();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Khen_thuong WHERE MA_NV LIKE @search OR LyDo LIKE @search",
                    conn);

                da.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvKhenThuong.DataSource = dt;
                gvKhenThuong.DataBind();
            }
        }

        protected void gvKhenThuong_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
