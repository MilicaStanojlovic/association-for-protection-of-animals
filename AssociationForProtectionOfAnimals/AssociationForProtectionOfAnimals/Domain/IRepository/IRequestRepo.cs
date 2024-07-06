using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssociationForProtectionOfAnimals.Domain.IRepository
{
    public interface IRequestRepo
    {
        Request? GetRequestById(int id);
        Request AddRequest(Request request);
        Request UpdateRequest(Request request);
        Request RemoveRequest(int id);
        List<Request> GetAllRequests();
        void Subscribe(IObserver observer);
    }
}
