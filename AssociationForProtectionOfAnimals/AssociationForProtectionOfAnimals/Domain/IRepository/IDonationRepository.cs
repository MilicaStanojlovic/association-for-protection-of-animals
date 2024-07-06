using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;

namespace AssociationForProtectionOfAnimals.Domain.IRepository
{
    public interface IDonationRepository : IObserver
    {
        Donation Add(Donation donation);
        Donation? GetById(int id);
        List<Donation> GetAllDonations();
        void Subscribe(IObserver observer);
    }
}
