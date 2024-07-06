using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;

namespace AssociationForProtectionOfAnimals.Controller
{
    public class RegisteredUserController : Subject
    {
        private readonly IRegisteredUserRepo _users;
        private readonly IAnimalRepo _animals;
        private readonly IAccountRepo _account;
        private readonly IPlaceRepo _place;
        private readonly IRequestRepo _request;
        private readonly IPostRepository _posts;

        public RegisteredUserController()
        {
            _users = Injector.CreateInstance<IRegisteredUserRepo>();
            _account = Injector.CreateInstance<IAccountRepo>();
            _place = Injector.CreateInstance<IPlaceRepo>();
            _animals = Injector.CreateInstance<IAnimalRepo>();
            _request = Injector.CreateInstance<IRequestRepo>();
            _posts = Injector.CreateInstance<IPostRepository>();
        }

        public void Add(RegisteredUser user)
        {
            Place place = _place.GetPlaceByNameAndPostalCode(user.Place);
            int placeId;
            if (place == null)
                placeId = _place.AddPlace(user.Place).Id;
            else
                placeId = place.Id;
            user.Place.Id = placeId;
            user.Account.Status = AccountStatus.WaitingForActivation;
            Account acc = _account.AddAccount(user.Account);
            user.Account = acc;
            _users.AddRegisteredUser(user);
            NotifyObservers();
        }
        public void Delete(int userId)
        {
            _users.RemoveRegisteredUser(userId);
            NotifyObservers();
        }
        public void Update(RegisteredUser user)
        {
            _users.UpdateRegisteredUser(user);
        }
        public void Subscribe(IObserver observer)
        {
            _users.Subscribe(observer);
            _account.Subscribe(observer);
        }
        public RegisteredUser? GetRegisteredUserById(int id)
        {
            return _users.GetRegisteredUserById(id);
        }
        public List<RegisteredUser> GetAllRegisteredUsers()
        {
            return _users.GetAllRegisteredUsers();
        }
        public List<RegisteredUser> GetEveryUser()
        {
            return _users.GetEveryRegisteredUser();
        }

        public Account GetAccountById(int id)
        {
            return _account.GetAccountById(id);
        }

        public RegisteredUser GetRegisteredUserByEmail(string email)
        {
            return _users.GetUserByEmail(email);
        }

        public bool IsUsernameUnique(string username)
        {
            foreach (RegisteredUser user in _users.GetAllRegisteredUsers())
                if (user.Account.Username.Equals(username)) return false;

            return true;
        }
        public Post? AddAnimal(Animal animal, int userId)
        {
            Animal newAnimal = _animals.AddAnimal(animal);
            if (newAnimal == null) return null;

            RegisteredUser user = _users.GetRegisteredUserById(userId);
            Post post = new Post(DateTime.Now, DateTime.Now, PostStatus.Unpublished, false, newAnimal.Id, user.Account.Username, "");
            return _posts.Add(post);
        }
        public Animal? UpdateAnimal(Animal animal)
        {
            return _animals.UpdateAnimal(animal);
        }
        public Animal? RemoveAnimal(int id)
        {
            return _animals.RemoveAnimal(id);
        }
    }
}
