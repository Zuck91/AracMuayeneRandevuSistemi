using MySql.Data.MySqlClient;

namespace AracMuayeneRandevuSistemi.DAL
{
    public class Baglanti
    {
        public static MySqlConnection Getir()
        {
            string baglantiCumlesi = "Server=localhost;Database=arac_muayene_randevu;Uid=root;Pwd=;Charset=utf8mb4;";
            return new MySqlConnection(baglantiCumlesi);
        }
    }
}
