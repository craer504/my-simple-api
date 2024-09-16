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

        public User(string username, string password)
        {
            _id = Guid.NewGuid();
            _username = username;
            _password = password;
        }

        public override string ToString()
        {
            return $"{_id} && {_username} && {_password}";
        }
    }

    public class UserFactory : IStringParseObjectCreatable
    {
        public object ParseStringCreateObject(string data)
        {
            if (data == string.Empty)
                return null;

            // Remove number + hypen at the beginning:
            string pattern = @"^\d+ - ";
            string cleaned = Regex.Replace(data, pattern, string.Empty);
            string[] parts = cleaned.Split(" && ");

            if (parts.Length != 3)
                return null;
            else
                return new User(parts[1], parts[2]) { Id = Guid.Parse(parts[0]) };
        }
    }
}
