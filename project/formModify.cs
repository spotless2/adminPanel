using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace project
{
    public partial class formModify : Form
    {
        string username, password, country;
        int userId;

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            username = txtUsername.Text;
        }
        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            password = txtPassword.Text;
        }

        private void cmdModify_Click(object sender, EventArgs e)
        {
            if (!Utilities.ValidateTextBoxes(this))
            {
                Utilities.ShowErrorMessage();
            }
            else
            {
                SqlConnection conn = Utilities.OpenDbConnection();
                string query = "UPDATE users " +
                    "SET user_name = @username, user_pass=@password," +
                    "user_register_date = @regDate, user_country_code = @country " +
                    "WHERE user_id=@userId ";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@regDate",
                dtpRegistrationDate.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@country", country);
                cmd.Parameters.AddWithValue("@userId", userId);
                Utilities.WriteDataToDb(conn, cmd);
                FillCmbUsers(cmbUser);
                Utilities.ClearTextBoxes(this);
            }
        }
        public void FillCmbUsers(ComboBox cmbTemp)
        {
            try
            {
                SqlConnection conn = Utilities.OpenDbConnection();
                String query = "SELECT user_id, user_name from users order by " +
                    "user_name";
                SqlCommand cmd = new SqlCommand(query, conn);
                DataTable dt = Utilities.GetDataFromDb(conn, cmd);
                cmbTemp.DataSource = dt;
                cmbTemp.DisplayMember = "user_name";
                cmbTemp.ValueMember = "user_id";
                cmbTemp.SelectedIndex = -1;
                cmbTemp.Text = "Selecteaza un utilizator";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void cmbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            int userId = -1;
            SqlConnection conn = Utilities.OpenDbConnection();
            String query = "SELECT * FROM users WHERE user_id = @userId";
            if (cmbUser.SelectedIndex > -1)
            {
                string user = cmbUser.SelectedValue.ToString();
                try
                {
                    userId = Convert.ToInt32(user);

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    DataTable dt = Utilities.GetDataFromDb(conn, cmd);
                    txtUsername.Text = Convert.ToString(dt.Rows[0]["user_name"]);
                    txtPassword.Text = Convert.ToString(dt.Rows[0]["user_pass"]);
                    dtpRegistrationDate.Value =
                    Convert.ToDateTime(dt.Rows[0]["user_register_date"]);
                    txtCountryCode.Text =
                    Convert.ToString(dt.Rows[0]["user_country_code"]);
                    txtUserId.Text = Convert.ToString(dt.Rows[0]["user_id"]);

                }
                catch (Exception convertE)
                {
                    Console.WriteLine(convertE.Message);
                }
            }
        }

        private void txtUserId_TextChanged(object sender, EventArgs e)
        {
            try
            {
                userId = Convert.ToInt32(txtUserId.Text);
            }
            catch (Exception convertE)
            {
                Console.WriteLine(convertE.Message);
            }
        }

        private void txtCountryCode_TextChanged(object sender, EventArgs e)
        {
            country = txtCountryCode.Text;
        }
        public formModify()
        {
            InitializeComponent();
            FillCmbUsers(cmbUser);
        }
    }
}
