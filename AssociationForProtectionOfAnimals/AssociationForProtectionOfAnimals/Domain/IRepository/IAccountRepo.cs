using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;

namespace AssociationForProtectionOfAnimals.Domain.IRepository
{
    public interface IAccountRepo
    {
        Account AddAccount(Account account);
        Account Update(Account account);
        Account GetAccountById(int id);
        void Subscribe(IObserver observer);
    }
}
