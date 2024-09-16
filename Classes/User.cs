using SimpleRegisterLoginLogout.Interfaces;
using SimpleRegisterLoginLogout.Utility;
using System.Text.RegularExpressions;

namespace SimpleRegisterLoginLogout.Classes
{
    public class User
    {
        private Guid _id;
        private string _username;
        private string _password;

        public string Username { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }
        public Guid Id { get => _id; set => _id = value; }

        public User(Guid id, string username, string password)
        {
            _id = id;
            _username = username;
            _password = password;
        }

        public override string ToString()
        {
            return $"{_id} && {_username} && {_password}";
        }
    }
}
