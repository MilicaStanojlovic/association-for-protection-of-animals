using AssociationForProtectionOfAnimals.Domain.IUtility;
using AssociationForProtectionOfAnimals.Domain.Model;

namespace AssociationForProtectionOfAnimals.Domain.Utility
{
    public class SortByPostStatus : ISortStrategy
    {
        public IEnumerable<Post> Sort(IEnumerable<Post> posts)
        {
            return posts.OrderBy(x => x.PostStatus);
        }
    }
}
