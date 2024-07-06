using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Storage;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.IUtility;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;

namespace AssociationForProtectionOfAnimals.Repository
{
    public class AdminRepo : Subject, IAdminRepo
    {
        private readonly List<Volunteer> _volunteers;
        private readonly List<Administrator> _admins;
        private readonly Storage<Volunteer> _storageVolunteers;
        private readonly Storage<Administrator> _storageAdmins;
        /*
            private readonly List<Animal> _animals;
            private readonly Storage<Animal> _animalsStorage;
        */

        public AdminRepo()
        {
            _storageVolunteers = new Storage<Volunteer>("volunteers.csv");
            _storageAdmins = new Storage<Administrator>("admins.csv");
            _volunteers = _storageVolunteers.Load();
            _admins = _storageAdmins.Load();
        }

        public Administrator? GetAdmin()
        {
            return _admins.Find(d => d.Id == 0);
        }

        private int GenerateId()
        {
            if (_volunteers.Count == 0) return 0;
            return _volunteers.Last().Id + 1;
        }
        public Administrator? UpdateAdministrator(Administrator? Administrator)
        {
            Administrator? oldAdministrator = GetAdmin();
            if (oldAdministrator == null) return null;

            oldAdministrator.FirstName = Administrator.FirstName;
            oldAdministrator.LastName = Administrator.LastName;
            oldAdministrator.Gender = Administrator.Gender;
            oldAdministrator.DateOfBirth = Administrator.DateOfBirth;
            oldAdministrator.PhoneNumber = Administrator.PhoneNumber;
            oldAdministrator.HomeAddress = Administrator.HomeAddress;
            oldAdministrator.IdNumber = Administrator.IdNumber;
            oldAdministrator.Account.Username = Administrator.Account.Username;
            oldAdministrator.Account.Password = Administrator.Account.Password;
            oldAdministrator.Account.Type = Administrator.Account.Type;

            _storageAdmins.Save(_admins);
            NotifyObservers();
            return oldAdministrator;
        }
        public Volunteer Add(Volunteer Volunteer)
        {
            Volunteer.Id = GenerateId();
            _volunteers.Add(Volunteer);
            _storageVolunteers.Save(_volunteers);
            NotifyObservers();
            return Volunteer;
        }

        public Volunteer? Update(Volunteer? Volunteer)
        {
            Volunteer? oldVolunteer = GetById(Volunteer.Id);
            if (oldVolunteer == null) return null;

            oldVolunteer.FirstName = Volunteer.FirstName;
            oldVolunteer.LastName = Volunteer.LastName;
            oldVolunteer.Gender = Volunteer.Gender;
            oldVolunteer.DateOfBirth = Volunteer.DateOfBirth;
            oldVolunteer.PhoneNumber = Volunteer.PhoneNumber;
            oldVolunteer.HomeAddress = Volunteer.HomeAddress;
            oldVolunteer.IdNumber = Volunteer.IdNumber;
            oldVolunteer.Account.Username = Volunteer.Account.Username;
            oldVolunteer.Account.Password = Volunteer.Account.Password;
            oldVolunteer.Account.Type = Volunteer.Account.Type;

            _storageVolunteers.Save(_volunteers);
            NotifyObservers();
            return oldVolunteer;
        }

        public Volunteer? Remove(int id)
        {
            Volunteer? Volunteer = GetById(id);
            if (Volunteer == null) return null;

            _volunteers.Remove(Volunteer);
            _storageVolunteers.Save(_volunteers);
            NotifyObservers();
            return Volunteer;
        }


        public Volunteer? GetById(int id)
        {
            return _volunteers.Find(t => t.Id == id);
        }

        public List<Volunteer> GetAll()
        {
            return _volunteers;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public List<Volunteer> GetAllVolunteers(int page, int pageSize, IUserSortStrategy sortStrategy, List<Volunteer> VolunteersToPaginate)
        {
            IEnumerable<Volunteer> Volunteers = sortStrategy.Sort(VolunteersToPaginate);
            Volunteers = Volunteers.Skip((page - 1) * pageSize).Take(pageSize);
            return Volunteers.ToList();
        }
    }
}
