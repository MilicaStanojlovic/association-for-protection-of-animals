using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.Storage.Serialization;
using System.Windows.Documents;
using System;
using System.Globalization;

namespace AssociationForProtectionOfAnimals.Domain.Model
{
    public class Request : ISerializable
    {
        protected int id;
        protected int volunteerId;
        protected int postId;
        protected int registeredUserId;
        protected RequestStatus requestStatus;
        protected DateTime requestSubmissionDate;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int VolunteerId
        {
            get { return volunteerId; }
            set { volunteerId = value; }
        }
        
        public int RegisteredUserId
        {
            get { return registeredUserId; }
            set { registeredUserId = value; }
        }
        public int PostId
        {
            get { return postId; }
            set { postId = value; }
        }

        public DateTime RequestSubmissionDate
        {
            get { return requestSubmissionDate; }
            set { requestSubmissionDate = value; }
        }

        public RequestStatus RequestStatus
        {
            get { return requestStatus; }
            set { requestStatus = value; }
        }
        public Request()
        { }
            public Request(int registeredUserId, int volunteerId, int postId, RequestStatus requestStatus, DateTime requestSubmissionDate)
        {
            this.registeredUserId = registeredUserId;
            this.volunteerId = volunteerId;
            this.postId = postId;
            this.requestStatus = requestStatus;
            this.requestSubmissionDate = requestSubmissionDate;
        }
        public Request(int id, int registeredUserId, int volunteerId, int postId, RequestStatus requestStatus, DateTime requestSubmissionDate)
        {
            this.id = id;
            this.registeredUserId = registeredUserId;
            this.volunteerId = volunteerId;
            this.postId = postId;
            this.requestStatus = requestStatus;
            this.requestSubmissionDate = requestSubmissionDate;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
            id.ToString(),
            volunteerId.ToString(),
            registeredUserId.ToString(),
            postId.ToString(),
            requestStatus.ToString(),
            requestSubmissionDate.ToString("yyyy-MM-dd")
            };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            int id = int.Parse(csvValues[0]);
            int volunteerId = int.Parse(csvValues[1]);
            int registeredUserId = int.Parse(csvValues[2]);
            int postId = int.Parse(csvValues[3]);
            RequestStatus requestStatus = (RequestStatus)Enum.Parse(typeof(RequestStatus), csvValues[4]);
            DateTime requestSubmissionDate = DateTime.ParseExact(csvValues[5], "yyyy-MM-dd", CultureInfo.InvariantCulture);

        }
        
    }
}
