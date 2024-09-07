namespace SimpleRegisterLoginLogout.Classes
{
    public class Post
    {
        private readonly Guid _id;
        private string _title;
        private string? _mediaURL;

        public Post(string title)
        {
            _id = Guid.NewGuid();
            _title = title;
        }

        public void SetMedia(string mediaURL)
        {
            _mediaURL = mediaURL;
        }
    }
}
