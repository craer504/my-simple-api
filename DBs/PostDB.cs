using SimpleRegisterLoginLogout.Classes;

namespace SimpleRegisterLoginLogout.DBs
{
    public class PostDB
    {
        private static List<Post> _postDB = new();
        public static string DBFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DBs", "post_db");

        public static List<Post> GetPostDB()
        {
            return _postDB;
        }

        public static void AddPost(Post post)
        {
            if (post != null)
                _postDB.Add(post);
        }

        public static Post? GetPostByID(Guid id)
        {
            return _postDB.Find(p => p.Id == id);
        }

        public static Post? GetPostByTitle(string title)
        {
            return _postDB.Find(p => p.Title == title);
        }
    }
}
