using Microsoft.Extensions.Primitives;
using SimpleRegisterLoginLogout.Classes;

namespace SimpleRegisterLoginLogout.DBs
{
    public static class UserPostDB
    {
        private static List<UserPost> _userpostDB = new();
        public static string DBFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DBs", "userpost_db");

        public static List<UserPost> GetUserPostDB()
        {
            return _userpostDB;
        }

        public static void AddUserPost(User user, Post post)
        {
            if (user != null && post != null)
            {
                UserPost? up = _userpostDB.FirstOrDefault(up => up.UserId == user.Id);

                if (up != null)
                {
                    up.PostIds.Add(post.Id);
                }
                else
                {
                    _userpostDB.Add(new UserPost(user.Id, new List<Guid> { post.Id }));
                }
            }
        }
    }
}
