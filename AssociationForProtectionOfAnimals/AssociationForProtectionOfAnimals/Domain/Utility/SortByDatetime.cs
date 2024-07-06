using AssociationForProtectionOfAnimals.Domain.IUtility;
using AssociationForProtectionOfAnimals.Domain.Model;
namespace AssociationForProtectionOfAnimals.Domain.Utility
{
    public class SortByDatetime : ISortStrategy, IUserSortStrategy
    {
        public IEnumerable<Post> Sort(IEnumerable<Post> posts)
        {
            return posts.OrderBy(x => x.DateOfPosting);
        }
        public IEnumerable<RegisteredUser> Sort(IEnumerable<RegisteredUser> registeredUsers)
        {
            return registeredUsers.OrderBy(x => x.DateOfBirth);
        }
        public IEnumerable<Volunteer> Sort(IEnumerable<Volunteer> volunteers)
        {
            return volunteers.OrderBy(x => x.DateOfBirth);

        }
    }
}
