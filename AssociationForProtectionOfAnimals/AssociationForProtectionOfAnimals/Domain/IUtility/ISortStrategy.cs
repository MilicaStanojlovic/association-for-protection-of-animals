using AssociationForProtectionOfAnimals.Domain.Model;

namespace AssociationForProtectionOfAnimals.Domain.IUtility
{
    public interface ISortStrategy
    {
        IEnumerable<Post> Sort(IEnumerable<Post> posts);
    }
}
