using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace BBL_Server
{
    class Database
    {
        private static bool UserExist(string username)
        {
            string query = "SELECT username FROM account WHERE username='" + username + "'";
            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }
        private static bool IsValidUsername(string userName)
        {
            Regex UsernameRegex = new Regex(@"^[a-z0-9_-]{3,15}$");
            return UsernameRegex.IsMatch(userName);
        }
        private static bool IsValidPassword(string password)
        {
            // Password (UpperCase, LowerCase, Number/SpecialChar and min 8 Chars)
            Regex PasswordRegex = new Regex(@"(?=^.{8,}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$");
            return PasswordRegex.IsMatch(password);
        }
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private static string HashPassword(string stringToHash)
        {
            using (var sha1 = new SHA1Managed())
            {
                return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(stringToHash)));
            }
        }
        private static bool IsPassordsMatch(string username, string password)
        {
            string query = "SELECT password FROM account WHERE username ='" + username + "'";
            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            string tempPassword = string.Empty;

            while (reader.Read())
            {
                tempPassword = reader["password"] + "";
            }
            reader.Close();
            return password == tempPassword;
        }
        private static void NewAccount(string username, string password)
        {
            string query = "INSERT INTO account(username,password,gold,level,exp) VALUES('" +
                username + "','" +
                password + "','" +
                Constants.playerGoldCreation + "','" +
                Constants.playerLevelCreation + "','" +
                Constants.playerExpCreation + "')";

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }

           
        }
        public static Account GetAccountData(string username)
        {
            string query = "SELECT * FROM account WHERE username='" + username + "'";
            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            int id =0;
            int level = 0;
            int gold = 0;
            int exp = 0;
            while (reader.Read())
            {
                id += reader.GetInt32("id");
                level += reader.GetInt32("level");
                gold += reader.GetInt32("gold");
                exp += reader.GetInt32("exp");
            }
            reader.Close();

            Account account = new Account();
            account.InitializeAccount(id, username, gold, level, exp);
            return account;
        }

        public static bool LoginAccount(string username, string password, out string error)
        {
            username = username.ToLower();
            if (!IsValidUsername(username)) {
                error = "The Username is not valid.";
                return false;
            }
            if (!UserExist(username)) {
                error = "Account does not exist.";
                return false;
            }
            if (!IsValidPassword(password)) {
                error = "The password have to respect these rules: UpperCase, LowerCase, Number/SpecialChar and min 8 Chars.";
                return false;
            }

            password = HashPassword(password);
            bool succes = IsPassordsMatch(username, password);
            if (!succes) {
                error = "Password is incorrect.";
                return false;
            }
            error = null;
            return true;
        }
        public static void RegisterAccount(string username, string password, int connectionID)
        {
            username = username.ToLower();
            if (!IsValidUsername(username)) { ServerTCP.PACKET_AlertMsg(connectionID, "The Username is not valid."); return; }
            if (UserExist(username)) { ServerTCP.PACKET_AlertMsg(connectionID, "The Username is already taken."); return; }
            if (!IsValidPassword(password)) { ServerTCP.PACKET_AlertMsg(connectionID, "The password have to respect these rules:" +
            " UpperCase, LowerCase, Number/SpecialChar and min 8 Chars."); return; }

            password = HashPassword(password);
            NewAccount(username, password);
            Console.WriteLine("id: {0} - New account has been created : '{1}'", connectionID, username);
            ServerTCP.PACKET_AlertMsg(connectionID, "Your account has been created");
        }
    }
}
