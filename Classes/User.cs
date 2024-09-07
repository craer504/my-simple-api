namespace SimpleRegisterLoginLogout.Classes
{
    public class User
    {
        private readonly Guid _id;
        private string _username;
        private string _password;

        public string Username { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }
        public Guid Id { get => _id; }

        public User(string username, string password)
        {
            _id = Guid.NewGuid();
            _username = username;
            _password = password;
        }

        public override string ToString()
        {
            return $"{_id} - {_username}";
        }
    }
}
