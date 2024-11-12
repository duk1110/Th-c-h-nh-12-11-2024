using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace WinFormsApp9
{
    public partial class Sach : Form
    {
        private string connectionString = "Server=localhost;Database=nhasach;Uid=root;Pwd=771099;";

        public Sach()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT s.MaSach, s.TenSach, s.TacGia, ls.TenLoaiSach FROM Sach s JOIN LoaiSach ls ON s.MaLoaiSach = ls.MaLoaiSach";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
        }

        private void LoadLoaiSach()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT MaLoaiSach, TenLoaiSach FROM LoaiSach";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                comboBox1.DisplayMember = "TenLoaiSach";  
                comboBox1.ValueMember = "MaLoaiSach";   
                comboBox1.DataSource = dataTable;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO Sach (TenSach, TacGia, MaLoaiSach) VALUES (@TenSach, @TacGia, @MaLoaiSach)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenSach", textBox2.Text);
                cmd.Parameters.AddWithValue("@TacGia", textBox3.Text);
                cmd.Parameters.AddWithValue("@MaLoaiSach", comboBox1.SelectedValue);

                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm sách thành công!");
                LoadData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string maSach = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "UPDATE Sach SET TenSach = @TenSach, TacGia = @TacGia, MaLoaiSach = @MaLoaiSach WHERE MaSach = @MaSach";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TenSach", textBox2.Text);
                    cmd.Parameters.AddWithValue("@TacGia", textBox3.Text);
                    cmd.Parameters.AddWithValue("@MaLoaiSach", comboBox1.SelectedValue);
                    cmd.Parameters.AddWithValue("@MaSach", maSach);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sửa sách thành công!");
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sách để sửa.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string maSach = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "DELETE FROM Sach WHERE MaSach = @MaSach";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaSach", maSach);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa sách thành công!");
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sách để xóa.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT s.MaSach, s.TenSach, s.TacGia, ls.TenLoaiSach FROM Sach s JOIN LoaiSach ls ON s.MaLoaiSach = ls.MaLoaiSach WHERE ls.TenLoaiSach = @TenLoaiSach";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenLoaiSach", comboBox1.Text);

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
        }


        private void Sach_Load(object sender, EventArgs e)
        {
            LoadData();    
            LoadLoaiSach();   
        }
    }
}
