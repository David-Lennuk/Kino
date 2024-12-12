using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CinemaBooking
{
    public partial class MainForm : Form
    {
        private string connectionString = "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;";

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
    }

    public partial class AdminForm : Form
    {
        private string connectionString;

        public AdminForm(string connectionString)
        {
            this.connectionString = connectionString;
            InitializeComponent();
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

        public UserForm(string connectionString, string userEmail)
        {
            this.connectionString = connectionString;
            this.userEmail = userEmail;
            InitializeComponent();
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
