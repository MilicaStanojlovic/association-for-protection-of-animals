using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Storage;

namespace AssociationForProtectionOfAnimals.Repository
{
    public class DonationRepository : Subject, IDonationRepository
    {
        private readonly List<Donation> _donations;
        private readonly Storage<Donation> _storage;

        public DonationRepository()
        {
            _storage = new Storage<Donation>("donations.csv");
            _donations = _storage.Load();
        }

        private int GenerateId()
        {
            if (_donations.Count == 0) return 0;
            return _donations.Last().Id + 1;
        }

        public Donation Add(Donation donation)
        {
            donation.Id = GenerateId();
            _donations.Add(donation);
            _storage.Save(_donations);
            NotifyObservers();
            return donation;
        }

        public Donation? GetById(int id)
        {
            return _donations.Find(v => v.Id == id);
        }

        public List<Donation> GetAllDonations()
        {
            return _donations;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
