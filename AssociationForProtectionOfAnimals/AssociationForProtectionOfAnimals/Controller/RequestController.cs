using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AssociationForProtectionOfAnimals.Controller
{
    public class RequestController
    {
        private readonly IRequestRepo _requests;
        private readonly IAdoptionRequestRepo _adoptionRequests;
        private readonly ITemporaryShelterRequestRepo _temporaryShelterRequests;
      
        public RequestController()
        {
            _requests = Injector.CreateInstance<IRequestRepo>();
            _adoptionRequests = Injector.CreateInstance<IAdoptionRequestRepo>();    
            _temporaryShelterRequests = Injector.CreateInstance<ITemporaryShelterRequestRepo>();
        }

        public void Send(Request request)
        {
            _requests.AddRequest(request);
        }

        public void SendAdoptionRequest(AdoptionRequest request)
        {
            _adoptionRequests.AddRequest(request);
        }
        public void SendTemporaryShelterRequest(TemporaryShelterRequest request)
        {
            _temporaryShelterRequests.AddRequest(request);
        }
        public void Update(Request request)
        {
            _requests.UpdateRequest(request);
        }
        public void Update(AdoptionRequest request)
        {
            _adoptionRequests.UpdateRequest(request);
        }
        public void Update(TemporaryShelterRequest request)
        {
            _temporaryShelterRequests.UpdateRequest(request);
        }

        public void Subscribe(IObserver observer)
        {
            _requests.Subscribe(observer);
            _adoptionRequests.Subscribe(observer);
            _temporaryShelterRequests.Subscribe(observer);
        }

        public Request? GetRequestById(int id)
        {
            return _requests.GetRequestById(id);
        }
        public AdoptionRequest? GetAdoptionRequestById(int id)
        {
            return _adoptionRequests.GetRequestById(id);
        }
        public Request? GetTemporaryShelterRequestById(int id)
        {
            return _temporaryShelterRequests.GetRequestById(id);
        }

        public List<Request> GetAllRequests()
        {
            return _requests.GetAllRequests();
        }
        public List<AdoptionRequest> GetAllAdoptionRequests()
        {
            return _adoptionRequests.GetAllRequests();
        }
        public List<TemporaryShelterRequest> GetAllTemporaryShelterRequests()
        {
            return _temporaryShelterRequests.GetAllRequests();
        }



    }
}
