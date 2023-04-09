using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisconnectedMimari
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=OGUZHANPC\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True");
        private void Form1_Load(object sender, EventArgs e)
        {
            NewMethod();
        }
        private void NewMethod()
        {
            SqlDataAdapter adp = new SqlDataAdapter("select * from Urunler", baglanti);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["UrunID"].Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string adi = textBox1.Text;
            decimal fiyat = numericUpDown1.Value;
            decimal stok = numericUpDown2.Value;
            if (adi == "" || fiyat == 0 || stok == 0)
            {
                MessageBox.Show("Tüm alanları eksiksiz doldurun!!");

            }
            SqlCommand komut = new SqlCommand();
            komut.CommandText = string.Format
                ("insert Urunler(UrunAdi,Fiyat,Stok) values('{0}',{1},{2})", adi, fiyat, stok);
            komut.Connection = baglanti;
            baglanti.Open();
            int etkilenen = komut.ExecuteNonQuery();
            if (etkilenen > 0)
            {
                MessageBox.Show("Kayıt eklendi!!");
                NewMethod();

            }
            else
            {
                MessageBox.Show("Kaydınız Eklenemedi!!");
            }
            baglanti.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = string.Format("update Urunler" + "set UrunAdi='{0}',Fiyat={1},Stok={2}" + "where UrunID={3}", textBox1.Text, numericUpDown1.Value, numericUpDown2.Value, textBox1.Tag);
            command.Connection = baglanti;
            baglanti.Open();
            try
            {
                int etk = command.ExecuteNonQuery();
                baglanti.Close();
                if (etk > 0)
                {
                    MessageBox.Show("Kayıt Güncellendi.");
                    NewMethod();
                }
                else
                {
                    MessageBox.Show("Kayıt güncellenirken hata oluştu.");
                }
            }
            catch (Exception ex)
            {
                baglanti.Close();
                MessageBox.Show("Bağlantı oluşturulurken hata oluştu." + ex.Message);

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells
                ["UrunAdi"].Value.ToString();
            textBox1.Tag = dataGridView1.CurrentRow.Cells["UrunID"].Value;
            numericUpDown1.Value = (decimal)dataGridView1.CurrentRow.Cells["Fiyat"].Value;
            numericUpDown2.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Stok"].Value);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            KategoriForm ktgr = new KategoriForm();
            ktgr.Show();
        }
    }
}
