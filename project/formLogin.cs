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
    public partial class formLogin : Form
    {
        public formLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConn = new SqlConnection(@"Data Source=localhost;Initial Catalog=online_tv;Integrated Security=True");
            string query = "Select * from tbl_Login Where username = '" + txbUser.Text.Trim() + "' and password = '" + txbPass.Text.Trim() + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, sqlConn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                formMain objFormMain = new formMain();
                this.Hide();
                objFormMain.Show();
            }
            else
            {
                MessageBox.Show("Check your username and password");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
