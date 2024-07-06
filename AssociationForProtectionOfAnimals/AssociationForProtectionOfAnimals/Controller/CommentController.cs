using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;

namespace AssociationForProtectionOfAnimals.Controller
{
    public class CommentController
    {
        private readonly ICommentRepository _comments;
        private readonly IAnimalRepo _animalRepository;

        public CommentController()
        {
            _comments = Injector.CreateInstance<ICommentRepository>();
        }

        public List<Comment> GetAllComments()
        {
            return _comments.GetAllComments();
        }

        public Comment? GetById(int postId)
        {
            return _comments.GetById(postId);
        }

        public Comment Add(Comment post)
        {
            return _comments.Add(post);
        }

        public void Subscribe(IObserver observer)
        {
            _comments.Subscribe(observer);
        }

        public List<Comment> GetCommentsByPost(int postId)
        {
            return _comments.GetCommentsByPost(postId);
        }
    }
}
