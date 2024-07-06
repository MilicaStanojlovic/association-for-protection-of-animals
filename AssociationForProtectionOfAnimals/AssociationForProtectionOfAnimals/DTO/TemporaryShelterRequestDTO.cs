using AssociationForProtectionOfAnimals.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace AssociationForProtectionOfAnimals.DTO
{
    public class TemporaryShelterRequestDTO : RequestDTO
    {
        Controller.RegisteredUserController userController = Injector.CreateInstance<Controller.RegisteredUserController>();
        
        private DateTime accommodationDate;
        private DateTime returnDate;
        private int registeredUserId;

        public DateTime AccommodationDate
        {
            get { return accommodationDate; }
            set { SetProperty(ref accommodationDate, value); }
        }
        public DateTime ReturnDate
        {
            get { return returnDate; }
            set { SetProperty(ref returnDate, value); }
        }
        public int RegisteredUserId
        {
            get { return registeredUserId; }
            set { registeredUserId = value; }
        }

        public TemporaryShelterRequest ToTemporaryShelterRequest()
        {
            return new TemporaryShelterRequest(Id, RegisteredUserId, VolunteerId,PostId, RequestStatus,RequestSubmissionDate,AccommodationDate,ReturnDate);
        }
        public TemporaryShelterRequestDTO()
        {
            
        }

        public TemporaryShelterRequestDTO(TemporaryShelterRequest request)
        {
            id = request.Id;
            volunteerId = request.VolunteerId;
            postId = request.PostId;    
            requestStatus = request.RequestStatus;
            requestSubmissionDate = request.RequestSubmissionDate;
            accommodationDate = request.AccommodationDate;
            
            returnDate = request.ReturnDate;
            
            registeredUserId = request.RegisteredUserId;
            Account account = userController.GetAccountById(registeredUserId);
            username = account.Username;    
        }
    }
}
