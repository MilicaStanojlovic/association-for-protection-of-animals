using AssociationForProtectionOfAnimals.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssociationForProtectionOfAnimals.DTO
{
    public class AdoptionRequestDTO : RequestDTO
    {
        Controller.RegisteredUserController userController = Injector.CreateInstance<Controller.RegisteredUserController>();

        private DateTime adoptionDate;
        private int registeredUserId;
        public DateTime AdoptionDate
        {
            get { return adoptionDate; }
            set { SetProperty(ref adoptionDate, value); }
        }
        public int RegisteredUserId
        {
            get { return registeredUserId; }
            set { registeredUserId = value; }
        }

        public AdoptionRequestDTO() { }

        public AdoptionRequest ToAdoptionRequest()
        {
            return new AdoptionRequest(Id, RegisteredUserId, VolunteerId,PostId, RequestStatus, RequestSubmissionDate, AdoptionDate);
        }
        public AdoptionRequestDTO(AdoptionRequest request)
        {
            id = request.Id;
            volunteerId = request.VolunteerId;
            postId = request.PostId;    
            requestStatus = request.RequestStatus;
            requestSubmissionDate = request.RequestSubmissionDate;
            adoptionDate = request.AdoptionDate;

            registeredUserId = request.RegisteredUserId;
            Account account = userController.GetAccountById(registeredUserId);
            username = account.Username;
        }
    }
}
