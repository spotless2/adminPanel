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
    public partial class formAdd : Form
    {
        string username, password, country;

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            username = txtUsername.Text;
        }
        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            password = txtPassword.Text;
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            if (!Utilities.ValidateTextBoxes(this))
            {
                Utilities.ShowErrorMessage();
            }
            else
            {
                SqlConnection conn = Utilities.OpenDbConnection();
                string query = "INSERT into users (user_name, user_pass," +
                    "user_register_date, user_country_code)" +
                    " VALUES(@username, @password, @regDate, @country); ";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@regdate",
                    dtpRegistrationDate.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@country", country);
                Utilities.WriteDataToDb(conn, cmd);
                Utilities.ClearTextBoxes(this);
                this.Close();
            }
        }

        private void txtCountryCode_TextChanged(object sender, EventArgs e)
        {
            country = txtCountryCode.Text;
        }
        public formAdd()
        {
            InitializeComponent();
        }
    }
}
