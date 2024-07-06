using AssociationForProtectionOfAnimals.Domain.IUtility;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.Observer;

namespace AssociationForProtectionOfAnimals.Domain.IRepository
{
    public interface IVolunteerRepo 
    {
        RegisteredUser? GetById(int id);
        List<RegisteredUser> GetAllRegisteredUsers();
        int GenerateId();
        RegisteredUser AddUser(RegisteredUser user);
        RegisteredUser? UpdateUser(RegisteredUser? user);
        RegisteredUser? RemoveUser(int id);
        RegisteredUser GetUserByEmail(string email);
        void Subscribe(IObserver observer);
        List<RegisteredUser> GetAllRegisteredUsers(int page, int pageSize, string sortCriteria, List<RegisteredUser> RegisteredUsers);
        List<RegisteredUser> GetAllRegisteredUsers(int page, int pageSize, IUserSortStrategy sortStrategy, List<RegisteredUser> RegisteredUsers);
        List<RegisteredUser> GetAllRegistrationRequests();
        RegisteredUser? AcceptRegistration(RegisteredUser user);
        RegisteredUser? DenyRegistration(RegisteredUser user);
    }
}
