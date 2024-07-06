using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Storage;

namespace AssociationForProtectionOfAnimals.Repository
{
    internal class AccountRepo : Subject, IAccountRepo
    {
        private readonly List<Account> _accounts;
        private readonly Storage<Account> _storage;

        public AccountRepo()
        {
            _storage = new Storage<Account>("accounts.csv");
            _accounts = _storage.Load();
        }

        private int GenerateId()
        {
            if (_accounts.Count == 0) return 0;
            return _accounts.Last().Id + 1;
        }
        public Account GetAccountById(int id)
        {
            return _accounts.Find(v => v.Id == id);
        }

        public Account AddAccount(Account account)
        {
            account.Id = GenerateId();
            _accounts.Add(account);
            _storage.Save(_accounts);
            NotifyObservers();
            return account;
        }
        public Account? Update(Account? account)
        {
            Account? oldAccount = GetAccountById(account.Id);
            if (oldAccount == null) return null;

            oldAccount.Username = account.Username;
            oldAccount.Password = account.Password;
            oldAccount.Type = account.Type;
            oldAccount.Status = account.Status;

            _storage.Save(_accounts);
            NotifyObservers();
            return oldAccount;
        }
    }
}
