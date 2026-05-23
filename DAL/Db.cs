using MySql.Data.MySqlClient;

namespace AracMuayeneRandevuSistemi.DAL
{
    public static class Db
    {
        public static string ConnectionString =
            "Server=localhost;Database=arac_muayene_randevu;Uid=root;Pwd=;Charset=utf8mb4;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
