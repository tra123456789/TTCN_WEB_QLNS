using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTCN_WEB_QLNS
{
    public partial class ThemBoPhan : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["QLNS"].ConnectionString;

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
                ViewState["IDPB"] = idPB;

                LoadTenPhongBan(idPB);
                LoadBoPhan(idPB);
            }
        }

        void LoadTenPhongBan(string idPB)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT TenPB FROM Phong_ban WHERE IDPB = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", idPB);

                conn.Open();
                lblPhongBan.Text = "Phòng ban: " + cmd.ExecuteScalar().ToString();
            }
        }

        void LoadBoPhan(string idPB)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT IDBP, TenBP FROM Bo_phan WHERE IDPB = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", idPB);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvBoPhan.DataSource = dt;
                gvBoPhan.DataBind();
            }
        }

        protected void btnAddBP_Click(object sender, EventArgs e)
        {
            string tenBP = txtTenBP.Text.Trim();
            string idPB = ViewState["IDPB"].ToString();

            if (string.IsNullOrEmpty(tenBP))
                return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "INSERT INTO Bo_phan(TenBP, IDPB) VALUES(@ten, @id)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", tenBP);
                cmd.Parameters.AddWithValue("@id", idPB);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            txtTenBP.Text = "";
            LoadBoPhan(idPB);
        }

        protected void gvBoPhan_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvBoPhan.EditIndex = e.NewEditIndex;
            LoadBoPhan(ViewState["IDPB"].ToString());
        }

        protected void gvBoPhan_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string idBP = gvBoPhan.DataKeys[e.RowIndex].Value.ToString();
            GridViewRow row = gvBoPhan.Rows[e.RowIndex];

            string tenBP = ((TextBox)row.FindControl("txtTenBP_Edit")).Text;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "UPDATE Bo_phan SET TenBP = @ten WHERE IDBP = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ten", tenBP);
                cmd.Parameters.AddWithValue("@id", idBP);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvBoPhan.EditIndex = -1;
            LoadBoPhan(ViewState["IDPB"].ToString());
        }

        protected void gvBoPhan_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvBoPhan.EditIndex = -1;
            LoadBoPhan(ViewState["IDPB"].ToString());
        }

        protected void gvBoPhan_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string idBP = gvBoPhan.DataKeys[e.RowIndex].Value.ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // ❌ Không cho xóa nếu còn nhân viên
                string checkNV = "SELECT COUNT(*) FROM Nhan_vien WHERE IDBP = @id";
                SqlCommand checkCmd = new SqlCommand(checkNV, conn);
                checkCmd.Parameters.AddWithValue("@id", idBP);

                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "alert",
                        "alert('Không thể xóa bộ phận vì vẫn còn nhân viên!');",
                        true);
                    return;
                }

                // ✅ Cho xóa
                string sql = "DELETE FROM Bo_phan WHERE IDBP = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", idBP);
                cmd.ExecuteNonQuery();
            }

            LoadBoPhan(ViewState["IDPB"].ToString());
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("PhongBan.aspx");
        }
    }
}
