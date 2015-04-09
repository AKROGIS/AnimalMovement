using System;
using System.Data.SqlClient;
using System.IO;

namespace WachExport
{
    class Program
    {
        static void Main()
        {
            const String connectionString = "Data Source=INPAKROVMAIS;Initial Catalog=Animal_Movement;Integrated Security=True";
            const String root = "/tmp/AM/WACH Export/";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                ExportFiles(root, "TPF Files", "tpf", connection, "wach_pfile_data");
                ExportFiles(root, "CSV Files", "csv", connection, "wach_cfile_data");
            }
        }

        private static void ExportFiles(string root, string folder, string ext, SqlConnection connection, string table)
        {
            string dir = Path.Combine(root, folder);
            Directory.CreateDirectory(dir);
            string sql = "SELECT [FileId], [Contents] FROM [dbo].[" + table + "]";
            using (var command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader results = command.ExecuteReader())
                {
                    while (results.Read())
                    {
                        string filename = Path.ChangeExtension(results.GetInt32(0).ToString(), ext);
                        filename = Path.Combine(dir, filename);
                        Byte[] bytes = results.GetSqlBytes(1).Buffer;
                        File.WriteAllBytes(filename, bytes);
                    }
                }
            }
        }
    }
}
