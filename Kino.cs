using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CinemaBooking
{
    public partial class MainForm : Form
    {
        private Button Kava_btn;
        private Button OstaPilet_btn;
        private PictureBox pictureBox1;
        private Label Nimi_lbl;
        private Button SignIn_btn;
        private Button SignUp_btn;
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Kino2;Integrated Security=True";

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string role = GetUserRole(txtEmail.Text, txtPassword.Text);

            if (role == "Admin")
            {
                AdminForm adminForm = new AdminForm(connectionString);
                adminForm.Show();
            }
            else if (role == "User")
            {
                UserForm userForm = new UserForm(connectionString, txtEmail.Text);
                userForm.Show();
            }
            else
            {
                MessageBox.Show("Invalid login credentials.");
            }
        }

        private string GetUserRole(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Role FROM Konto WHERE Email = @Email AND Parool = @Password", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                object result = cmd.ExecuteScalar();

                return result != null ? result.ToString() : null;
            }
        }

        private void InitializeComponent()
        {
            this.Kava_btn = new System.Windows.Forms.Button();
            this.OstaPilet_btn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Nimi_lbl = new System.Windows.Forms.Label();
            this.SignIn_btn = new System.Windows.Forms.Button();
            this.SignUp_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Kava_btn
            // 
            this.Kava_btn.BackColor = System.Drawing.Color.Crimson;
            this.Kava_btn.ForeColor = System.Drawing.Color.Black;
            this.Kava_btn.Location = new System.Drawing.Point(49, 67);
            this.Kava_btn.Name = "Kava_btn";
            this.Kava_btn.Size = new System.Drawing.Size(165, 91);
            this.Kava_btn.TabIndex = 0;
            this.Kava_btn.Text = "Kava";
            this.Kava_btn.UseVisualStyleBackColor = false;
            // 
            // OstaPilet_btn
            // 
            this.OstaPilet_btn.BackColor = System.Drawing.Color.Crimson;
            this.OstaPilet_btn.Location = new System.Drawing.Point(49, 280);
            this.OstaPilet_btn.Name = "OstaPilet_btn";
            this.OstaPilet_btn.Size = new System.Drawing.Size(165, 91);
            this.OstaPilet_btn.TabIndex = 1;
            this.OstaPilet_btn.Text = "Osta pilet";
            this.OstaPilet_btn.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Crimson;
            this.pictureBox1.Location = new System.Drawing.Point(294, 77);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(226, 274);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // Nimi_lbl
            // 
            this.Nimi_lbl.AutoSize = true;
            this.Nimi_lbl.Location = new System.Drawing.Point(46, 9);
            this.Nimi_lbl.Name = "Nimi_lbl";
            this.Nimi_lbl.Size = new System.Drawing.Size(0, 13);
            this.Nimi_lbl.TabIndex = 4;
            // 
            // SignIn_btn
            // 
            this.SignIn_btn.BackColor = System.Drawing.Color.Crimson;
            this.SignIn_btn.ForeColor = System.Drawing.Color.Black;
            this.SignIn_btn.Location = new System.Drawing.Point(505, 12);
            this.SignIn_btn.Name = "SignIn_btn";
            this.SignIn_btn.Size = new System.Drawing.Size(71, 38);
            this.SignIn_btn.TabIndex = 5;
            this.SignIn_btn.Text = "Sign in";
            this.SignIn_btn.UseVisualStyleBackColor = false;
            // 
            // SignUp_btn
            // 
            this.SignUp_btn.BackColor = System.Drawing.Color.Crimson;
            this.SignUp_btn.ForeColor = System.Drawing.Color.Black;
            this.SignUp_btn.Location = new System.Drawing.Point(597, 12);
            this.SignUp_btn.Name = "SignUp_btn";
            this.SignUp_btn.Size = new System.Drawing.Size(71, 38);
            this.SignUp_btn.TabIndex = 6;
            this.SignUp_btn.Text = "Sign up";
            this.SignUp_btn.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.BackgroundImage = global::CinemaBooking.Properties.Resources.Kino;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(695, 422);
            this.Controls.Add(this.SignUp_btn);
            this.Controls.Add(this.SignIn_btn);
            this.Controls.Add(this.Nimi_lbl);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.OstaPilet_btn);
            this.Controls.Add(this.Kava_btn);
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }

    public partial class AdminForm : Form
    {
        private string connectionString;
        private DataGridView dgvAdmin;

        public AdminForm(string connectionString)
        {
            this.connectionString = connectionString;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.dgvAdmin = new DataGridView();
            this.Controls.Add(this.dgvAdmin);
            this.dgvAdmin.Dock = DockStyle.Fill;
        }

        private void LoadTable(string tableName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {tableName}", conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvAdmin.DataSource = table;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

                adapter.Update((DataTable)dgvAdmin.DataSource);
                MessageBox.Show("Changes saved to database.");
            }
        }
    }

    public partial class UserForm : Form
    {
        private string connectionString;
        private string userEmail;
        private ListBox lstMovies;
        private ListBox lstSessions;
        private DataGridView dgvHall;

        public UserForm(string connectionString, string userEmail)
        {
            this.connectionString = connectionString;
            this.userEmail = userEmail;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lstMovies = new ListBox();
            this.lstSessions = new ListBox();
            this.dgvHall = new DataGridView();
            this.Controls.Add(this.lstMovies);
            this.Controls.Add(this.lstSessions);
            this.Controls.Add(this.dgvHall);
            this.lstMovies.Dock = DockStyle.Top;
            this.lstSessions.Dock = DockStyle.Top;
            this.dgvHall.Dock = DockStyle.Fill;
        }

        private void LoadMovies()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID, FilmiNimi, Poster FROM Film", conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                lstMovies.DataSource = table;
                lstMovies.DisplayMember = "FilmiNimi";
                lstMovies.ValueMember = "ID";
            }
        }

        private void lstMovies_SelectedIndexChanged(object sender, EventArgs e)
        {
            int movieId = (int)lstMovies.SelectedValue;
            LoadSessions(movieId);
        }

        private void LoadSessions(int movieId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID, Aeg FROM Seanss WHERE FilmID = @MovieId", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@MovieId", movieId);

                DataTable table = new DataTable();
                adapter.Fill(table);
                lstSessions.DataSource = table;
                lstSessions.DisplayMember = "Aeg";
                lstSessions.ValueMember = "ID";
            }
        }

        private void btnLoadHall_Click(object sender, EventArgs e)
        {
            int sessionId = (int)lstSessions.SelectedValue;
            LoadHallPlan(sessionId);
        }

        private void LoadHallPlan(int sessionId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT Koht.ID, Koht.Rida, Koht.Veerus, CASE WHEN Pilet.ID IS NOT NULL THEN 'Occupied' ELSE 'Free' END AS Status FROM Koht LEFT JOIN Pilet ON Koht.ID = Pilet.KohtID AND Pilet.SeanssID = @SessionId", conn);
                adapter.SelectCommand.Parameters.AddWithValue("@SessionId", sessionId);

                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvHall.DataSource = table;
            }
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            int sessionId = (int)lstSessions.SelectedValue;
            int seatId = (int)dgvHall.SelectedRows[0].Cells["ID"].Value;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Pilet (KohtID, SeanssID, KontoID) VALUES (@SeatId, @SessionId, (SELECT ID FROM Konto WHERE Email = @UserEmail))", conn);
                cmd.Parameters.AddWithValue("@SeatId", seatId);
                cmd.Parameters.AddWithValue("@SessionId", sessionId);
                cmd.Parameters.AddWithValue("@UserEmail", userEmail);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Seat booked successfully!");

                GenerateTicketPDF(sessionId, seatId);
                LoadHallPlan(sessionId);
            }
        }

        private void GenerateTicketPDF(int sessionId, int seatId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT Film.FilmiNimi, Seanss.Aeg, Koht.Rida, Koht.Veerus FROM Pilet INNER JOIN Seanss ON Pilet.SeanssID = Seanss.ID INNER JOIN Film ON Seanss.FilmID = Film.ID INNER JOIN Koht ON Pilet.KohtID = Koht.ID WHERE Pilet.SeanssID = @SessionId AND Pilet.KohtID = @SeatId", conn);
                cmd.Parameters.AddWithValue("@SessionId", sessionId);
                cmd.Parameters.AddWithValue("@SeatId", seatId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string movieName = reader.GetString(0);
                    DateTime sessionTime = reader.GetDateTime(1);
                    int row = reader.GetInt32(2);
                    int column = reader.GetInt32(3);

                    Document pdfDoc = new Document();
                    PdfWriter.GetInstance(pdfDoc, new FileStream($"Ticket_{movieName}_{row}_{column}.pdf", FileMode.Create));
                    pdfDoc.Open();
                    pdfDoc.Add(new Paragraph($"Movie: {movieName}"));
                    pdfDoc.Add(new Paragraph($"Date & Time: {sessionTime}"));
                    pdfDoc.Add(new Paragraph($"Seat: Row {row}, Column {column}"));
                    pdfDoc.Close();

                    MessageBox.Show("Ticket PDF generated!");
                }
            }
        }
    }
}
