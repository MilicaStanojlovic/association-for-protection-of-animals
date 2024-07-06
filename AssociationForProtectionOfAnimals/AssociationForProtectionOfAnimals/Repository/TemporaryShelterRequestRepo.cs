using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AssociationForProtectionOfAnimals.Repository
{
    public class TemporaryShelterRequestRepo : Subject, ITemporaryShelterRequestRepo
    {
        private readonly List<TemporaryShelterRequest> _requests;
        private readonly Storage<TemporaryShelterRequest> _storage;

        public TemporaryShelterRequestRepo()
        {
            _storage = new Storage<TemporaryShelterRequest>("temporaryShelterRequests.csv");
            _requests = _storage.Load();
        }
        private int GenerateId()
        {
            if (_requests.Count == 0) return 0;
            return _requests.Last().Id + 1;
        }
        public TemporaryShelterRequest AddRequest(TemporaryShelterRequest request)
        {
            request.Id = GenerateId();
            _requests.Add(request);
            _storage.Save(_requests);
            NotifyObservers();
            return request;
        }
        public TemporaryShelterRequest? UpdateRequest(TemporaryShelterRequest request)
        {
            TemporaryShelterRequest? oldRequest = GetRequestById(request.Id);
            if (oldRequest == null) return null;

            oldRequest.RegisteredUserId = request.RegisteredUserId;
            oldRequest.VolunteerId = request.VolunteerId;
            oldRequest.PostId = request.PostId;
            oldRequest.RequestSubmissionDate = request.RequestSubmissionDate;
            oldRequest.RequestStatus = request.RequestStatus;
            oldRequest.AccommodationDate = request.AccommodationDate;
            oldRequest.ReturnDate = request.ReturnDate;

            _storage.Save(_requests);
            NotifyObservers();
            return oldRequest;
        }

        public TemporaryShelterRequest? RemoveRequest(int id)
        {
            TemporaryShelterRequest? request = GetRequestById(id);
            if (request == null) return null;

            _requests.Remove(request);
            _storage.Save(_requests);
            NotifyObservers();
            return request;
        }
        public TemporaryShelterRequest? GetRequestById(int id)
        {
            return _requests.Find(v => v.Id == id);
        }
        public List<TemporaryShelterRequest> GetAllRequests()
        {
            return _requests;
        }
    }
}
