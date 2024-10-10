namespace SimpleRegisterLoginLogout.Classes
{
    public class UserPost
    {
        private Guid _userId;
        private List<Guid> _postIds;

        public Guid UserId { get => _userId; set => _userId = value; }
        public List<Guid> PostIds { get => _postIds; set => _postIds = value; }

        public UserPost(Guid userId, List<Guid> postIds = null)
        {
            _userId = userId;
            _postIds = postIds ?? new List<Guid>();
        }

        public override string ToString()
        {
            return $"{_userId} && {string.Join(", ", _postIds)}";
        }
    }
}
