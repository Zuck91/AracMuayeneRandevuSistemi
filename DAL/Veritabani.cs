using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace AracMuayeneRandevuSistemi.DAL
{
    public class Veritabani
    {
        public DataTable Listele(string procedureAdi)
        {
            MySqlConnection baglanti = Baglanti.Getir();
            MySqlCommand komut = new MySqlCommand(procedureAdi, baglanti);
            komut.CommandType = CommandType.StoredProcedure;

            MySqlDataAdapter adapter = new MySqlDataAdapter(komut);
            DataTable tablo = new DataTable();
            adapter.Fill(tablo);
            return tablo;
        }

        public void Calistir(string procedureAdi, Dictionary<string, object> parametreler)
        {
            MySqlConnection baglanti = Baglanti.Getir();
            MySqlCommand komut = new MySqlCommand(procedureAdi, baglanti);
            komut.CommandType = CommandType.StoredProcedure;

            foreach (var item in parametreler)
            {
                komut.Parameters.AddWithValue(item.Key, item.Value);
            }

            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
        }
    }
}
