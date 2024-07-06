using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.Domain.IUtility;
using System.IO;

namespace AssociationForProtectionOfAnimals.Controller
{
    public class VolunteerController
    {
        private readonly IVolunteerRepo _volunteers;
        private readonly IRegisteredUserRepo _users;
        private readonly IAdminRepo _admin;
        private readonly IAnimalRepo _animals;
        private readonly IAccountRepo _accounts;
        private readonly IPlaceRepo _places;
        private readonly IPostRepository _posts;

        public VolunteerController()
        {
            _volunteers = Injector.CreateInstance<IVolunteerRepo>();
            _users = Injector.CreateInstance<IRegisteredUserRepo>();
            _admin = Injector.CreateInstance<IAdminRepo>();
            _animals = Injector.CreateInstance<IAnimalRepo>();
            _accounts = Injector.CreateInstance<IAccountRepo>();
            _places = Injector.CreateInstance<IPlaceRepo>();
            _posts = Injector.CreateInstance<IPostRepository>();
        }

        public RegisteredUser? GetById(int id)
        {
            return _volunteers.GetById(id);
        }
        public List<RegisteredUser> GetAllRegisteredUsers()
        {
            return _volunteers.GetAllRegisteredUsers();
        }
        public RegisteredUser AddUser(RegisteredUser user)
        {
            Place place = _places.GetPlaceByNameAndPostalCode(user.Place);
            int placeId;
            if (place == null)
                placeId = _places.AddPlace(user.Place).Id;
            else
                placeId = place.Id;
            user.Place.Id = placeId;
            Account acc = _accounts.AddAccount(user.Account);
            user.Account = acc;
            RegisteredUser ret = _volunteers.AddUser(user);
            return ret;
        }

        public RegisteredUser? UpdateUser(RegisteredUser? user)
        {
            Place place = _places.GetPlaceByNameAndPostalCode(user.Place);
            int placeId;
            if (place == null)
                placeId = _places.AddPlace(user.Place).Id;
            else
                placeId = place.Id;
            user.Place.Id = placeId;
            Account acc = _accounts.Update(user.Account);
            user.Account = acc;
            RegisteredUser ret = _volunteers.UpdateUser(user);
            return ret;
        }

        public RegisteredUser? RemoveUser(int id)
        {
            RegisteredUser? user = GetById(id);
            if (user == null) return null;
            _volunteers.RemoveUser(id);
            return user;
        }

        public Post? AddAnimal(Animal animal, int userId)
        {
            Animal newAnimal = _animals.AddAnimal(animal);
            if (newAnimal == null) return null;

            Volunteer user = _admin.GetById(userId);
            Post post = new Post(DateTime.Now, DateTime.Now, PostStatus.ForAdoption, false, newAnimal.Id, user.Account.Username, "");
            return _posts.Add(post);
        }
        public Post? AcceptPostRequest(Post post)
        {
            post.PostStatus = PostStatus.ForAdoption;
            return _posts.Update(post);
        }
        public bool RejectPostRequest(Post post)
        {
            return _posts.Remove(post.Id) != null && _animals.RemoveAnimal(post.AnimalId) != null;
        }

        public Animal? UpdateAnimal(Animal animal)
        {
            return _animals.UpdateAnimal(animal);
        }

        public Animal? RemoveAnimal(int id)
        {
            return _animals.RemoveAnimal(id);
        }

        public Animal? GetAnimalById(int id)
        {
            return _animals.GetAnimalById(id);
        }

        public List<Animal> GetAllAnimals()
        {
            return _animals.GetAllAnimals();
        }


        public void Subscribe(IObserver observer)
        {
            _volunteers.Subscribe(observer);
            _users.Subscribe(observer);
            _accounts.Subscribe(observer);
        }

        public RegisteredUser GetVolunteerByEmail(string email)
        {
            return _volunteers.GetUserByEmail(email);
        }
        public List<RegisteredUser> GetAllRegisteredUsers(int page, int pageSize, string sortCriteria, List<RegisteredUser> RegisteredUsers)
        {
            return _volunteers.GetAllRegisteredUsers(page, pageSize, sortCriteria, RegisteredUsers);
        }
        public List<RegisteredUser> GetAllRegisteredUsers(int page, int pageSize, IUserSortStrategy sortStrategy, List<RegisteredUser> RegisteredUsers)
        {
            return _volunteers.GetAllRegisteredUsers(page, pageSize, sortStrategy, RegisteredUsers);
        }
        public List<RegisteredUser> FindRegisteredUsersByCriteria(string firstName, string lastName, Place? place, DateTime? datetime)
        {
            List<RegisteredUser> registeredUsers = GetAllRegisteredUsers();

            var filteredRegisteredUsers = registeredUsers.Where(registeredUser =>
                (string.IsNullOrEmpty(firstName) || registeredUser.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(lastName) || registeredUser.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)) &&
                (place == null || registeredUser.Place?.Id == place.Id) &&
                (!datetime.HasValue || registeredUser.DateOfBirth.Date == datetime.Value.Date)
            ).ToList();

            return filteredRegisteredUsers;
        }

        public List<RegisteredUser> GetAllRegistrationRequests()
        {
            return _volunteers.GetAllRegistrationRequests();
        }

        public RegisteredUser? AcceptRegistration(RegisteredUser user)
        {
            user.Account.Status = AccountStatus.Active;
            _accounts.Update(user.Account);
            return _volunteers.AcceptRegistration(user);
        }

        public RegisteredUser? DenyRegistration(RegisteredUser user)
        {
            user.Account.Status = AccountStatus.Denied;
            _accounts.Update(user.Account);
            return _volunteers.DenyRegistration(user);
        }
    }
}