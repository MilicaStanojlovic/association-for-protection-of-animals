using AssociationForProtectionOfAnimals.Observer;
using System;
using System.Collections.Generic;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using System.Linq;
using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.IUtility;

namespace AssociationForProtectionOfAnimals.Controller
{
    public class AdministratorController
    {
        private readonly IAdminRepo _admins;
        private readonly IVolunteerRepo? _volunteers;
        private readonly IRegisteredUserRepo? _users;
        private readonly IAnimalRepo? _animals;
        private readonly IAccountRepo _accounts;
        private readonly IPlaceRepo _places;

        public AdministratorController()
        {
            _admins = Injector.CreateInstance<IAdminRepo>();
            _volunteers = Injector.CreateInstance<IVolunteerRepo>();
            _users = Injector.CreateInstance<IRegisteredUserRepo>();
            _animals = Injector.CreateInstance<IAnimalRepo>();
            _accounts = Injector.CreateInstance<IAccountRepo>();
            _places = Injector.CreateInstance<IPlaceRepo>();
        }

        public Administrator? GetAdministrator()
        {
            return _admins.GetAdmin();
        }

        public Volunteer? GetById(int volunteerId)
        {
            return _admins.GetById(volunteerId);
        }

        public List<Volunteer> GetAllVolunteers()
        {
            return _admins.GetAll();
        }

        public Volunteer Add(Volunteer volunteer)
        {
            Place place = _places.GetPlaceByNameAndPostalCode(volunteer.Place);
            int placeId;
            if (place == null)
                placeId = _places.AddPlace(volunteer.Place).Id;
            else
                placeId = place.Id;
            volunteer.Place.Id = placeId;
            Account acc = _accounts.AddAccount(volunteer.Account);
            volunteer.Account = acc;
            Volunteer ret = _admins.Add(volunteer);
            return ret;
        }

        public Volunteer Update(Volunteer volunteer)
        {
            return _admins.Update(volunteer);
        }

        public void UpdateAdministrator(Administrator director)
        {
            _admins.UpdateAdministrator(director);
        }

        public void Delete(Volunteer volunteer)
        {
            _admins.Remove(volunteer.Id);
        }

        public void Subscribe(IObserver observer)
        {
            _admins.Subscribe(observer);
            _volunteers.Subscribe(observer);
            _accounts.Subscribe(observer);
        }
        public Animal AddAnimal(Animal animal)
        {
            return _animals.AddAnimal(animal);
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

        public List<RegisteredUser> GetAllRegisteredUsers()
        {
            return _volunteers.GetAllRegisteredUsers();
        }
        public RegisteredUser? GetUserById(int id)
        {
            return _volunteers.GetById(id);
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
            RegisteredUser ret = _volunteers.AddUser(user);
            _accounts.AddAccount(ret.Account);
            return ret;
        }

        public RegisteredUser? UpdateUser(RegisteredUser? user)
        {
            return _volunteers.UpdateUser(user);
        }

        public RegisteredUser? RemoveUser(int id)
        {
            RegisteredUser? user = GetUserById(id);
            if (user == null) return null;
            _volunteers.RemoveUser(id);
            return user;
        }

        public Volunteer? GetVolunteerByUsername(string username)
        {
            foreach (Volunteer volunteer in GetAllVolunteers())
                if (volunteer.Account.Username == username)
                    return volunteer;
            return null;
        }
        public List<Volunteer> GetAllVolunteers(int page, int pageSize, IUserSortStrategy sortStrategy, List<Volunteer> RegisteredUsers)
        {
            return _admins.GetAllVolunteers(page, pageSize, sortStrategy, RegisteredUsers);
        }
        public List<Volunteer> FindVolunteersByCriteria(string firstName, string lastName, Place? place, DateTime? datetime)
        {
            List<Volunteer> volunteers = GetAllVolunteers();

            var filteredVolunteers = volunteers.Where(volunteer =>
                (string.IsNullOrEmpty(firstName) || volunteer.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(lastName) || volunteer.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)) &&
                (place == null || volunteer.Place?.Id == place.Id) &&
                (!datetime.HasValue || volunteer.DateOfBirth.Date == datetime.Value.Date)
            ).ToList();

            return filteredVolunteers;
        }
        public List<RegisteredUser> GetAllRegistrationRequests()
        {
            return _volunteers.GetAllRegistrationRequests();
        }

        public RegisteredUser? AcceptRegistration(RegisteredUser user)
        {
            return _volunteers.AcceptRegistration(user);
        }

        public RegisteredUser? DenyRegistration(RegisteredUser user)
        {
            return _volunteers.DenyRegistration(user);
        }
    }
}
