using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL_Server
{
    public class Account
    {
        public int id;
        public string username;
        public int gold;
        public int level;
        public int exp;


        public Account()
        {
            id = -1;
            username = "";
            gold = -1;
            level = -1;
            exp = -1;
        }

        public void InitializeAccount(int _id, string _username, int _gold, int _level, int _exp)
        {
            id = _id;
            username = _username;
            gold = _gold;
            level = _level;
            exp = _exp;
        }

        public override string ToString()
        {
            //Serializer.WriteInt(id);
            //Serializer.WriteString(username);
            //Serializer.WriteInt(gold);
            //Serializer.WriteInt(level);
            //Serializer.WriteInt(exp);
            string account = id + "_" + username + "_" + gold + "_" + level + "_" + exp;
            return account;
        }
    }
}
