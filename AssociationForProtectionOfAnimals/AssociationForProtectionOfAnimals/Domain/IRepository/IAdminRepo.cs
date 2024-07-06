using AssociationForProtectionOfAnimals.Observer;
using System.Collections.Generic;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.IUtility;

namespace AssociationForProtectionOfAnimals.Domain.IRepository
{
    public interface IAdminRepo : IObserver
    {
        Administrator? GetAdmin();
        Administrator? UpdateAdministrator(Administrator? Administrator);
        Volunteer Add(Volunteer Volunteer);
        Volunteer? Update(Volunteer? Volunteer);
        Volunteer? Remove(int id);
        Volunteer? GetById(int id);
        List<Volunteer> GetAll();
        List<Volunteer> GetAllVolunteers(int page, int pageSize, IUserSortStrategy sortStrategy, List<Volunteer> VolunteersToPaginate);
        void Subscribe(IObserver observer);
    }
}
