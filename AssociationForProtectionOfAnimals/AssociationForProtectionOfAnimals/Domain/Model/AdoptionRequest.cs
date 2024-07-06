using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.Storage.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssociationForProtectionOfAnimals.Domain.Model
{
    public class AdoptionRequest : Request, ISerializable
    {
        private DateTime adoptionDate;
        public DateTime AdoptionDate
        {
            get { return adoptionDate; }
            set { adoptionDate = value; }
        }
        public AdoptionRequest() { }
        public AdoptionRequest(int id,int registeredUserId, int volunteerId, int postId, RequestStatus status, DateTime requestSubmissionDate, DateTime adoptionDate)
            : base(id, registeredUserId, volunteerId,postId, status, requestSubmissionDate)
        {
            this.adoptionDate = adoptionDate;
        }
        public AdoptionRequest(int registeredUserId, int volunteerId, int postId, RequestStatus status, DateTime requestSubmissionDate, DateTime adoptionDate)
            : base(registeredUserId, volunteerId, postId, status, requestSubmissionDate)
        {
            this.adoptionDate = adoptionDate;
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
                requestSubmissionDate.ToString("yyyy-MM-dd"),
                adoptionDate.ToString("yyyy-MM-dd")
            };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            if (csvValues.Length != 7)
                throw new ArgumentException("Invalid number of requests values in CSV");

            id = int.Parse(csvValues[0]);
            volunteerId = int.Parse(csvValues[1]);
            registeredUserId = int.Parse(csvValues[2]);
            postId = int.Parse(csvValues[3]);
            requestStatus = (RequestStatus)Enum.Parse(typeof(RequestStatus), csvValues[4]);
            requestSubmissionDate = DateTime.ParseExact(csvValues[5], "yyyy-MM-dd", null);
            adoptionDate = DateTime.ParseExact(csvValues[6], "yyyy-MM-dd", null);
        }
    }

}
