using SimpleRegisterLoginLogout.Classes;
using SimpleRegisterLoginLogout.Interfaces;
using System.Text.RegularExpressions;

namespace SimpleRegisterLoginLogout.Factories
{
    public class UserPostFactory : IStringParseObjectCreatable
    {
        public object ParseStringCreateObject(string data)
        {
            if (data == null)
                return null;

            string pattern = @"^\d+ - ";
            string cleaned = Regex.Replace(data, pattern, string.Empty);
            string[] parts = cleaned.Split(" && ");

            // If username is not null
            if (parts[0] != null)
            {
                string[] posts = parts[1].Split(", ");

                return new UserPost(Guid.Parse(parts[0]), posts.Select(p => Guid.Parse(p)).ToList());
            }

            return null;
        }
    }
}
