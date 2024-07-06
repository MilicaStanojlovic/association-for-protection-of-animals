using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Storage;

namespace AssociationForProtectionOfAnimals.Repository
{
    public class RequestRepo : Subject, IRequestRepo
    {
        private readonly List<Request> _requests;
        private readonly Storage<Request> _storage;

        public RequestRepo()
        {
            _storage = new Storage<Request>("requests.csv");
            _requests = _storage.Load();
        }
        private int GenerateId()
        {
            if (_requests.Count == 0) return 0;
            return _requests.Last().Id + 1;
        }
        public Request AddRequest(Request request)
        {
            request.Id = GenerateId();
            _requests.Add(request);
            _storage.Save(_requests);
            NotifyObservers();
            return request;
        }
        public Request? UpdateRequest(Request request)
        {
            Request? oldRequest = GetRequestById(request.Id);
            if (oldRequest == null) return null;

            oldRequest.RegisteredUserId = request.RegisteredUserId;
            oldRequest.VolunteerId = request.VolunteerId;
            oldRequest.RequestSubmissionDate = request.RequestSubmissionDate;
            oldRequest.RequestStatus = request.RequestStatus;

            _storage.Save(_requests);
            NotifyObservers();
            return oldRequest;
        }

        public Request? RemoveRequest(int id)
        {
            Request? request = GetRequestById(id);
            if (request == null) return null;

            _requests.Remove(request);
            _storage.Save(_requests);
            NotifyObservers();
            return request;
        }
        public Request? GetRequestById(int id)
        {
            return _requests.Find(v => v.Id == id);
        }
        public List<Request> GetAllRequests()
        {
            return _requests;
        }
    }
}
