namespace SimpleRegisterLoginLogout.Utility
{
    public static class DBSaver
    {
        private static string? _targetURL = string.Empty;

        public static void SetTargetURL(string targetURL)
        {
            _targetURL = targetURL;
        }
    }
}
