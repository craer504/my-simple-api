using SimpleRegisterLoginLogout.Classes;
using SimpleRegisterLoginLogout.Interfaces;
using System.Text.RegularExpressions;

namespace SimpleRegisterLoginLogout.Factories
{
    public class UserFactory : IStringParseObjectCreatable
    {
        public object ParseStringCreateObject(string data)
        {
            if (data == string.Empty)
                return null;

            // Remove number and hypen at the beginning:
            string pattern = @"^\d+ - ";
            string cleaned = Regex.Replace(data, pattern, string.Empty);
            string[] parts = cleaned.Split(" && ");

            if (parts.Length != 3)
                return null;
            else
                return new User(Guid.Parse(parts[0]), parts[1], parts[2]);
        }
    }
}
