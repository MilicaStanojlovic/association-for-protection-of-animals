using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;

namespace AssociationForProtectionOfAnimals.Controller
{
    public class DonationController 
    { 
        private readonly IDonationRepository _donations;

        public DonationController()
        {
            _donations = Injector.CreateInstance<IDonationRepository>();
        }

        public List<Donation> GetAllDonations()
        {
            return _donations.GetAllDonations();
        }

        public Donation? GetById(int donationId)
        {
            return _donations.GetById(donationId);
        }

        public Donation Add(Donation donation)
        {
            return _donations.Add(donation);
        }

        public void Subscribe(IObserver observer)
        {
            _donations.Subscribe(observer);
        }
    }
}
