using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssociationForProtectionOfAnimals.Repository
{
    public class AdoptionRequestRepo : Subject, IAdoptionRequestRepo
    {
        private readonly List<AdoptionRequest> _requests;
        private readonly Storage<AdoptionRequest> _storage;
        public AdoptionRequestRepo()
        {
            _storage = new Storage<AdoptionRequest>("adoptionRequests.csv");
            _requests = _storage.Load();
        }
        private int GenerateId()
        {
            if (_requests.Count == 0) return 0;
            return _requests.Last().Id + 1;
        }
        public AdoptionRequest AddRequest(AdoptionRequest request)
        {
            request.Id = GenerateId();
            _requests.Add(request);
            _storage.Save(_requests);
            NotifyObservers();
            return request;
        }

        public List<AdoptionRequest> GetAllRequests()
        {
            return _requests;
        }

        public AdoptionRequest? GetRequestById(int id)
        {
            return _requests.Find(v => v.Id == id);
        }

        public AdoptionRequest RemoveRequest(int id)
        {
            AdoptionRequest? request = GetRequestById(id);
            if (request == null) return null;

            _requests.Remove(request);
            _storage.Save(_requests);
            NotifyObservers();
            return request;
        }

        public AdoptionRequest UpdateRequest(AdoptionRequest request)
        {
            AdoptionRequest? oldRequest = GetRequestById(request.Id);
            if (oldRequest == null) return null;

            oldRequest.RegisteredUserId = request.RegisteredUserId;
            oldRequest.VolunteerId = request.VolunteerId;
            oldRequest.PostId = request.PostId; 
            oldRequest.RequestSubmissionDate = request.RequestSubmissionDate;
            oldRequest.RequestStatus = request.RequestStatus;
            oldRequest.AdoptionDate = request.AdoptionDate;

            _storage.Save(_requests);
            NotifyObservers();
            return oldRequest;
        }
    }
}
