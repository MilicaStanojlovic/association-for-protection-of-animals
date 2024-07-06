using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssociationForProtectionOfAnimals.Domain.IRepository
{
    public interface ITemporaryShelterRequestRepo
    {
        TemporaryShelterRequest? GetRequestById(int id);
        TemporaryShelterRequest AddRequest(TemporaryShelterRequest request);
        TemporaryShelterRequest UpdateRequest(TemporaryShelterRequest request);
        TemporaryShelterRequest RemoveRequest(int id);
        List<TemporaryShelterRequest> GetAllRequests();
        void Subscribe(IObserver observer);
    }
}
