using SimpleRegisterLoginLogout.Classes;
using SimpleRegisterLoginLogout.Interfaces;
using System.Text.RegularExpressions;

namespace SimpleRegisterLoginLogout.Factories
{
    public class PostFactory : IStringParseObjectCreatable
    {
        public object ParseStringCreateObject(string data)
        {
            if (data == null)
                return null;

            string pattern = @"^\d+ - ";
            string cleaned = Regex.Replace(data, pattern, string.Empty);
            string[] parts = cleaned.Split(" && ");

            if (parts[0] != string.Empty)
            {
                var post = new Post(Guid.Parse(parts[0]), parts[1]);

                _ = parts[2] != null ? post.MediaURL = parts[2] : "";

                return post;
            }

            return null;
        }
    }
}
