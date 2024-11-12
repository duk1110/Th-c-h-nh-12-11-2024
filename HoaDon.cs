using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace WinFormsApp9
{
    public partial class HoaDon : Form
    {
        private string connectionString = "Server=localhost;Database=nhasach;Uid=root;Pwd=771099;";

        public HoaDon()
        {
            InitializeComponent();
        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            LoadHoaDonData();    // Hiển thị danh sách hóa đơn
            LoadKhachHangData();  // Tải danh sách khách hàng vào comboBox1
            LoadSachData();       // Tải danh sách sách vào comboBox2
        }

        private void LoadHoaDonData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"SELECT hd.MaHoaDon, hd.NgayLap, kh.TenKhachHang 
                                 FROM HoaDon hd 
                                 JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
        }

        private void LoadKhachHangData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT MaKhachHang, TenKhachHang FROM KhachHang";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                comboBox1.DisplayMember = "TenKhachHang";
                comboBox1.ValueMember = "MaKhachHang";
                comboBox1.DataSource = dataTable;
            }
        }

        private void LoadSachData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT MaSach, TenSach FROM Sach";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                comboBox2.DisplayMember = "TenSach";
                comboBox2.ValueMember = "MaSach";
                comboBox2.DataSource = dataTable;
            }
        }

        // Thêm hóa đơn
        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO HoaDon (NgayLap, MaKhachHang) VALUES (@NgayLap, @MaKhachHang)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NgayLap", DateTime.Parse(textBox2.Text));
                cmd.Parameters.AddWithValue("@MaKhachHang", comboBox1.SelectedValue);

                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm hóa đơn thành công!");
                LoadHoaDonData();
            }
        }

        // Sửa hóa đơn
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string maHoaDon = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "UPDATE HoaDon SET NgayLap = @NgayLap, MaKhachHang = @MaKhachHang WHERE MaHoaDon = @MaHoaDon";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NgayLap", DateTime.Parse(textBox2.Text));
                    cmd.Parameters.AddWithValue("@MaKhachHang", comboBox1.SelectedValue);
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sửa hóa đơn thành công!");
                    LoadHoaDonData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để sửa.");
            }
        }

        // Xóa hóa đơn
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string maHoaDon = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "DELETE FROM HoaDon WHERE MaHoaDon = @MaHoaDon";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa hóa đơn thành công!");
                    LoadHoaDonData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để xóa.");
            }
        }

        // Tìm kiếm hóa đơn theo tên khách hàng
        private void button4_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"SELECT hd.MaHoaDon, hd.NgayLap, kh.TenKhachHang 
                                 FROM HoaDon hd 
                                 JOIN KhachHang kh ON hd.MaKhachHang = kh.MaKhachHang 
                                 WHERE kh.TenKhachHang LIKE @TenKhachHang";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenKhachHang", "%" + textBox1.Text + "%");

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
        }
    }
}
