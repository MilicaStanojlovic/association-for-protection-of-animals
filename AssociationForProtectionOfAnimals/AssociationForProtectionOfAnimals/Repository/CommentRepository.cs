using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Storage;

namespace AssociationForProtectionOfAnimals.Repository
{
    public class CommentRepository : Subject, ICommentRepository
    {
        private readonly List<Comment> _comments;
        private readonly Storage<Comment> _storage;

        public CommentRepository()
        {
            _storage = new Storage<Comment>("comment.csv");
            _comments = _storage.Load();
        }

        private int GenerateId()
        {
            if (_comments.Count == 0) return 0;
            return _comments.Last().Id + 1;
        }

        public Comment Add(Comment comment)
        {
            comment.Id = GenerateId();
            _comments.Add(comment);
            _storage.Save(_comments);
            NotifyObservers();
            return comment;
        }

        public Comment? GetById(int id)
        {
            return _comments.Find(v => v.Id == id);
        }

        public List<Comment> GetAllComments()
        {
            return _comments;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public List<Comment> GetCommentsByPost(int postId)
        {
            return _comments.Where(v => v.PostId == postId).ToList();
        }
    }
}
