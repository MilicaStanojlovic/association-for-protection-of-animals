using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using System.Windows.Input;

namespace AssociationForProtectionOfAnimals.Domain.IRepository
{
    public interface IPostRepository : IObserver
    {
        Post Add(Post post);
        Post? Update(Post post);
        Post? Remove(int id);
        Post? GetById(int id);
        List<Post>? GetPostByPersonPosted(string personPostedEmail);
        List<Post> GetAllPosts();
        List<Post> GetAllPosts(int page, int pageSize, string sortCriteria, List<Post> posts);
        void Subscribe(IObserver observer);

    }
}