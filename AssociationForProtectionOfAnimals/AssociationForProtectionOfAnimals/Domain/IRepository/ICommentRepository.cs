using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;

namespace AssociationForProtectionOfAnimals.Domain.IRepository
{
    public interface ICommentRepository : IObserver
    {
        Comment Add(Comment comment);
        Comment? GetById(int id);
        List<Comment> GetAllComments();
        List<Comment> GetCommentsByPost(int postId);
        void Subscribe(IObserver observer);

    }
}
