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
    public class TemporaryShelterRequest : Request, ISerializable
    {
        private DateTime accommodationDate;
        private DateTime returnDate;
        public DateTime AccommodationDate
        {
            get { return accommodationDate; }
            set { accommodationDate = value; }
        }
        public DateTime ReturnDate
        {
            get { return returnDate; }
            set { returnDate = value; }
        }
        //public DateTime ReturnDate { get; set; }
        public TemporaryShelterRequest() { }
        public TemporaryShelterRequest(int id,int registeredUserId, int volunteerId, int postId, RequestStatus status, DateTime requestSubmissionDate, DateTime accommodationDate, DateTime returnDate)
            : base(id, registeredUserId, volunteerId,postId, status, requestSubmissionDate)
        {
            this.accommodationDate = accommodationDate;
            this.returnDate = returnDate;
        }
        public TemporaryShelterRequest(int registeredUserId, int volunteerId, int postId, RequestStatus status, DateTime requestSubmissionDate, DateTime accommodationDate, DateTime returnDate)
            : base(registeredUserId, volunteerId,postId, status, requestSubmissionDate)
        {
            AccommodationDate = accommodationDate;
            ReturnDate = returnDate;
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
                AccommodationDate.ToString("yyyy-MM-dd"),
                ReturnDate.ToString("yyyy-MM-dd")
            };
            return csvValues;
        }

        public void FromCSV(string[] csvValues)
        {
            if (csvValues.Length != 8)
                throw new ArgumentException("Invalid number of requests values in CSV");

            id = int.Parse(csvValues[0]);
            volunteerId = int.Parse(csvValues[1]);
            registeredUserId = int.Parse(csvValues[2]);
            postId = int.Parse(csvValues[3]);
            requestStatus = (RequestStatus)Enum.Parse(typeof(RequestStatus), csvValues[4]);
            requestSubmissionDate = DateTime.ParseExact(csvValues[5], "yyyy-MM-dd", null);
            accommodationDate = DateTime.ParseExact(csvValues[6], "yyyy-MM-dd", null);
            returnDate = DateTime.ParseExact(csvValues[7], "yyyy-MM-dd", null);

            
        }
    }
}
