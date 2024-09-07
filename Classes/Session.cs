namespace SimpleRegisterLoginLogout.Classes
{
    public static class Session
    {
        private static User? _user;

        public static User? User { get => _user; set => _user = value; }
    }
}
