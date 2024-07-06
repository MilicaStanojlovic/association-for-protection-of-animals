using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssociationForProtectionOfAnimals.Domain.IRepository
{
    public interface IAdoptionRequestRepo
    {
        AdoptionRequest? GetRequestById(int id);
        AdoptionRequest AddRequest(AdoptionRequest request);
        AdoptionRequest UpdateRequest(AdoptionRequest request);
        AdoptionRequest RemoveRequest(int id);
        List<AdoptionRequest> GetAllRequests();
        void Subscribe(IObserver observer);
    }
}
