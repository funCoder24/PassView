using System.Data;
using System.IO;
using System.Text;

namespace PassView
{
    class PassView
    {
        static void Main(string[] args)
        {
            byte[] entropy = null; // DPAPI class does not use entropy but requires this parameter
            string description;    // I could not understand the purpose of a this mandatory parameter
                                   // Output always is Null
            string filename = "my_chrome_passwords.html";
            StreamWriter Writer = new StreamWriter(filename, false, Encoding.UTF8);

            DataTable DB = Chrome.GetPassData(true);

            int rows = DB.Rows.Count;
            Writer.Write("<table><thead><tr><th>S.No</th><th>Site URL</th><th>User</th><th>Password</th></tr></thead><tbody>");
            for (int i = 0; i < rows; i++)
            {
                byte[] byteArray = (byte[])DB.Rows[i][5];
                byte[] decrypted = EncoderDecoder.Decrypt(byteArray, entropy, out description);
                string password = new UTF8Encoding(true).GetString(decrypted);

                Writer.Write($"<tr><td>{i + 1}</td><td>{DB.Rows[i][1]}</td><td>{DB.Rows[i][3]}</td><td>{password}</td></tr>");
            }
            Writer.Write("</tbody></table>");
            Writer.Close();
        }
    }
}
