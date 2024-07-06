using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Storage;
using AssociationForProtectionOfAnimals.Domain.IUtility;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;

namespace AssociationForProtectionOfAnimals.Repository
{
    public class VolunteerRepo : Subject, IVolunteerRepo
    {
        private readonly List<RegisteredUser> _users;
        private readonly Storage<RegisteredUser> _usersStorage;
        /*
            private readonly List<Animal> _animals;
            private readonly Storage<Animal> _animalsStorage;
        */

        public VolunteerRepo()
        {
            _usersStorage = new Storage<RegisteredUser>("registeredUsers.csv");
            _users = _usersStorage.Load();
        }
        public RegisteredUser? GetById(int id)
        {
            return _users.Find(v => v.Id == id);
        }
        public List<RegisteredUser> GetAllRegisteredUsers()
        {
            return _users.Where(user => user.Account.Status == AccountStatus.Active).ToList();
        }

        public List<RegisteredUser> GetAllRegistrationRequests()
        {
            return _users.Where(user => user.Account.Status == AccountStatus.WaitingForActivation).ToList();
        }

        public RegisteredUser? AcceptRegistration(RegisteredUser user)
        {
            user.Account.Status = AccountStatus.Active;
            return UpdateUser(user);
        }

        public RegisteredUser? DenyRegistration(RegisteredUser user)
        {
            user.Account.Status = AccountStatus.Denied;
            return UpdateUser(user);
        }

        public int GenerateId()
        {
            if (_users.Count == 0) return 0;
            return _users.Last().Id + 1;
        }
        public RegisteredUser AddUser(RegisteredUser user)
        {
            user.Id = GenerateId();
            _users.Add(user);
            _usersStorage.Save(_users);
            NotifyObservers();
            return user;
        }

        public RegisteredUser? UpdateUser(RegisteredUser? user)
        {
            RegisteredUser? oldRegisteredUser = GetById(user.Id);
            if (oldRegisteredUser == null) return null;

            oldRegisteredUser.FirstName = user.FirstName;
            oldRegisteredUser.LastName = user.LastName;
            oldRegisteredUser.Gender = user.Gender;
            oldRegisteredUser.DateOfBirth = user.DateOfBirth;
            oldRegisteredUser.PhoneNumber = user.PhoneNumber;
            oldRegisteredUser.HomeAddress = user.HomeAddress;
            oldRegisteredUser.IdNumber = user.IdNumber;
            oldRegisteredUser.Account.Username = user.Account.Username;
            oldRegisteredUser.Account.Password = user.Account.Password;
            oldRegisteredUser.Account.Type = user.Account.Type;

            _usersStorage.Save(_users);
            NotifyObservers();
            return oldRegisteredUser;
        }

        public RegisteredUser? RemoveUser(int id)
        {
            RegisteredUser? user = GetById(id);
            if (user == null) return null;

            _users.Remove(user);
            _usersStorage.Save(_users);
            NotifyObservers();
            return user;
        }

        public RegisteredUser GetUserByEmail(string email)
        {
            return _users.Find(v => v.Account.Username == email);
        }
        public List<RegisteredUser> GetAllRegisteredUsers(int page, int pageSize, string sortCriteria, List<RegisteredUser> registeredUsersToPaginate)
        {
            IEnumerable<RegisteredUser> registeredUsers = registeredUsersToPaginate;

            switch (sortCriteria)
            {
                case "DateOfBirth":
                    registeredUsers = registeredUsers.OrderBy(x => x.DateOfBirth);
                    break;
            }

            registeredUsers = registeredUsers.Skip((page - 1) * pageSize).Take(pageSize);

            return registeredUsers.ToList();
        }
        public List<RegisteredUser> GetAllRegisteredUsers(int page, int pageSize, IUserSortStrategy sortStrategy, List<RegisteredUser> registeredUsersToPaginate)
        {
            IEnumerable<RegisteredUser> RegisteredUsers = sortStrategy.Sort(registeredUsersToPaginate);
            RegisteredUsers = RegisteredUsers.Skip((page - 1) * pageSize).Take(pageSize);
            return RegisteredUsers.ToList();
        }
    }
}
