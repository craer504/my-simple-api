using SimpleRegisterLoginLogout.Classes;

namespace SimpleRegisterLoginLogout.Utility
{
    public static class Session
    {
        private static User? _user;

        public static User? User { get => _user; set => _user = value; }

        public static bool UserLogout()
        {
            if (_user != null)
            {
                _user = null;
                return true;
            }

            return false;
        }
    }
}
