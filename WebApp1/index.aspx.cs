using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspTask1_a_
{
    public partial class index : System.Web.UI.Page
    {
        //DataTable dt = new DataTable();

        //DataAccessLayer.DataAccessLayer dal = new DataAccessLayer.DataAccessLayer();
        SqlConnection SqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["aspcrud"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnDelete.Enabled = false;
                fillGridView();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        public void Clear()
        {
            hfContactID.Value = "";
            txtName.Text = "";
            txtMobile.Text = "";
            txtAddress.Text = "";
            btnSave.Text = "Save";
            lblSuccessMessage.Text = lblErrorMessage.Text = "";
            btnDelete.Enabled = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SqlConnection.State == ConnectionState.Closed)
                SqlConnection.Open();
            SqlCommand cmd = new SqlCommand("ContactCreateOrUpdate", SqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@ContactID", hfContactID.Value == "" ? 0 : Convert.ToInt32(hfContactID.Value));
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
            cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
            cmd.ExecuteNonQuery();
            SqlConnection.Close();
            Clear();
            string ContactID = "hfContactID.Value";
            if (ContactID == "")
                lblSuccessMessage.Text = "Saved Successfully";
            else
                lblErrorMessage.Text = "Updated Successfully";

            fillGridView();
        }

        void fillGridView()
        {
            if (SqlConnection.State == ConnectionState.Closed)
                SqlConnection.Open();
            SqlCommand cmd = new SqlCommand("ContactViewAll", SqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            SqlConnection.Close();
            gvContact.DataSource = dt;
            gvContact.DataBind();
        }

        protected void lnk_OnClick(object sender, EventArgs e)
        {
            int ContactID = Convert.ToInt32((sender as LinkButton).CommandArgument);
            if (SqlConnection.State == ConnectionState.Closed)
                SqlConnection.Open();
            SqlCommand cmd = new SqlCommand("ContactViewByID", SqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ContactID", ContactID);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            SqlConnection.Close();
            hfContactID.Value = ContactID.ToString();
            txtName.Text = dt.Rows[0]["Name"].ToString();
            txtMobile.Text = dt.Rows[0]["Mobile"].ToString();
            txtAddress.Text = dt.Rows[0]["Address"].ToString();
            btnSave.Text = "Update";
            btnDelete.Enabled = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (SqlConnection.State == ConnectionState.Closed)
                SqlConnection.Open();
            SqlCommand cmd = new SqlCommand("ContactDeleteByID", SqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ContactID", Convert.ToInt32(hfContactID.Value));
            cmd.ExecuteNonQuery();
            SqlConnection.Close();
            Clear();
            fillGridView();
            lblSuccessMessage.Text = "Deleted Successfully";
        }
    }
}