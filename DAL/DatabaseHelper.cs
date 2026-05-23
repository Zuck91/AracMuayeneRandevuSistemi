using MySql.Data.MySqlClient;
using System.Data;

namespace AracMuayeneRandevuSistemi.DAL
{
    public class DatabaseHelper
    {
        public DataTable Listele(string procedureName)
        {
            using MySqlConnection baglanti = Db.GetConnection();
            using MySqlCommand komut = new MySqlCommand(procedureName, baglanti);
            komut.CommandType = CommandType.StoredProcedure;

            using MySqlDataAdapter da = new MySqlDataAdapter(komut);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            return tablo;
        }

        public void Calistir(string procedureName, Dictionary<string, object> parametreler)
        {
            using MySqlConnection baglanti = Db.GetConnection();
            using MySqlCommand komut = new MySqlCommand(procedureName, baglanti);
            komut.CommandType = CommandType.StoredProcedure;

            foreach (var p in parametreler)
            {
                komut.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
            }

            baglanti.Open();
            komut.ExecuteNonQuery();
        }
    }
}
