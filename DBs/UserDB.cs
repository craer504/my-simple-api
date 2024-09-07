using SimpleRegisterLoginLogout.Classes;
using System.Text;

namespace SimpleRegisterLoginLogout.DBs
{
    public static class UserDB
    {
        private static List<User> _userDB = new List<User>();

        public static void AddUser(User user)
        {
            _userDB.Add(user);
        }

        public static User? GetUserByCredentials(string username, string password)
        {
            return _userDB.Find(u => u.Username == username && u.Password == password);
        }

        public static User? GetUserById(Guid id)
        {
            return _userDB.Find(u => u.Id == id);
        }

        public static bool IsUserRegistered(string username, string password)
        {
            return _userDB.Exists(u => u.Username == username && u.Password == password);
        }

        public static bool IsUsernameRegistered(string username)
        {
            return _userDB.Exists(u => u.Username == username);
        }

        public static string GetAllUsers()
        {
            StringBuilder sb = new StringBuilder();
            var count = 1;

            foreach (var u in _userDB)
            {
                sb.AppendLine(count++ + "-" + u.Username);
            }

            return sb.ToString();
        }
    }
}
