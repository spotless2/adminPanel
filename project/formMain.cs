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
    public partial class formMain : Form
    {
        DataTable dt = new DataTable();
        public void LoadData()
        {
            // connection info
            String connectionString = "Data Source=localhost;Initial Catalog=online_tv;Integrated Security=True";
            String query = "Select * FROM users";

            // connection, SQL and adapter initialization
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            // loading data
            con.Open();
            adapter.Fill(dt);
            con.Close(); 

            // cleaning unused allocated memory of the objects
            adapter.Dispose();
            cmd.Dispose();
            con.Dispose();
        }
        public class MyUser
        {
            public int id;
            public string name;
            public override string ToString()
            {
                return name;
            }
        }
        public void ShowUsers()
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                MyUser user = new MyUser();
                user.id = Convert.ToInt32(dt.Rows[i]["user_Id"]);
                user.name = Convert.ToString(dt.Rows[i]["user_Name"]);

                lbUsers.Items.Add(user);
            }
        }
        public formMain()
        {
            InitializeComponent();
        }

        private void lbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnModify.Visible = true;
            btnDelete.Visible = true;
            
            int index = lbUsers.SelectedIndex;

            string info = "";
            int i;
            if (dt.Rows.Count > 0 && index >= 0)
            {
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    info += dt.Columns[i].ColumnName + ": " +
                        dt.Rows[index][dt.Columns[i].ColumnName] + "\r\n";
                }
            }
            tbDetails.Text = info;
        }
        public void ClearAll()
        {
            dt = new DataTable();
            lbUsers.Items.Clear();
            tbDetails.Text = "";
        }
        public void RefreshData()
        {
            ClearAll();
            LoadData();
            ShowUsers();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadData();
            ShowUsers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int i = lbUsers.SelectedIndex;
            if (i < 0) return;
            else
            {
                String userId = dt.Rows[i][dt.Columns[0].ColumnName].ToString();
                // connection info
                String connectionString = "Data Source=localhost;Initial Catalog=online_tv;Integrated Security=True";
                String query = "DELETE FROM users WHERE user_id=@userId";
                // connection, SQL and adapter initialization
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@userId", userId);
                // loading data
                con.Open();
                cmd.ExecuteReader();
                con.Close();
                // cleaning unused allocated memory of the objects
                cmd.Dispose();
                con.Dispose();
                
                RefreshData();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            formAdd form2 = new formAdd();
            form2.ShowDialog();
            RefreshData();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            formModify form3 = new formModify();
            form3.ShowDialog();
            RefreshData();
        }
    }
}
