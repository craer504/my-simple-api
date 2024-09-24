namespace SimpleRegisterLoginLogout.Classes
{
    public class Post
    {
        private Guid _id;
        private string _title;
        private string? _mediaURL;

        public Guid Id { get => _id; set => _id = value; }
        public string Title { get => _title; set => _title = value; }
        public string? MediaURL { get => _mediaURL; set => _mediaURL = value; }

        public Post(Guid id, string title, string? mediaURL = "")
        {
            _id = id;
            _title = title;
            _mediaURL = mediaURL;
        }

        public override string ToString()
        {
            return $"{_id} && {_title} && {_mediaURL}";
        }
    }
}
