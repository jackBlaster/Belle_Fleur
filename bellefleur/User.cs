using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bellefleur
{
    internal class User
    {
        private static Dictionary<string, string> users = Create_users();

        public static Dictionary<string, string> Users => users;

        private static Dictionary<string, string> Create_users()
        {
            Dictionary<string, string> users = new Dictionary<string, string>();
            users.Add("root", "root");
            users.Add("bozo", "bozo");
            return users;
        }
    }
}
