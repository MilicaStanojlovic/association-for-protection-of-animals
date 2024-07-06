
using AssociationForProtectionOfAnimals.Domain.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using AssociationForProtectionOfAnimals.DTO;
using System.Collections.ObjectModel;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Controller;
using AssociationForProtectionOfAnimals.Domain.IUtility;
using AssociationForProtectionOfAnimals.Domain.Utility;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using System.Windows.Controls;
using System.Windows.Input;
namespace AssociationForProtectionOfAnimals.View.Adoption
{
    /// <summary>
    /// Interaction logic for VolunteerRequests.xaml
    /// </summary>
    public partial class VolunteerRequests: Window, IObserver
    {
        
        public class ViewModel
        {
            public ObservableCollection<AdoptionRequestDTO> AdoptionRequestDTOs { get; set; }
            public ObservableCollection<TemporaryShelterRequestDTO> TempShelterRequestDTOs { get; set; }

            public ViewModel()
            {
                AdoptionRequestDTOs = new ObservableCollection<AdoptionRequestDTO>();
                TempShelterRequestDTOs = new ObservableCollection<TemporaryShelterRequestDTO>();
            }

        }

        public ViewModel TableViewModel { get; set; }
        public AdoptionRequestDTO SelectedAdoptionRequest { get; set; }
        public TemporaryShelterRequestDTO SelectedTempRequest { get; set; }
        private RegisteredUserController regUserController { get; set; }
        private VolunteerController volunteerController { get; set; }
        private PostController postController { get; set; }

        private RequestController requestController { get; set; }

        private int userId { get; set; }
        private bool isSearchButtonClicked = false;

        public VolunteerRequests(int userId)
        {
            InitializeComponent();
            TableViewModel = new ViewModel();
            regUserController = Injector.CreateInstance<RegisteredUserController>();
            volunteerController = Injector.CreateInstance<VolunteerController>();
            postController = Injector.CreateInstance<PostController>();
            requestController = Injector.CreateInstance<RequestController>();   

            this.userId = userId;


            DataContext = this;
            //teacherController.Subscribe(this);
            Update();
        }

        public void Update()
        {
            try
            {
                TableViewModel.AdoptionRequestDTOs.Clear();
                TableViewModel.TempShelterRequestDTOs.Clear();

                var tempShelterRequests = GetFilteredTempShelterRequests();
                var adoptionRequests = GetFilteredAdoptionRequests();
                
                if (tempShelterRequests != null)
                {
                    foreach (Domain.Model.TemporaryShelterRequest tempRequest in tempShelterRequests)
                        TableViewModel.TempShelterRequestDTOs.Add(new TemporaryShelterRequestDTO(tempRequest));
                }
                else if(adoptionRequests != null)
                {
                    foreach (Domain.Model.AdoptionRequest adoptRequest in adoptionRequests)
                        TableViewModel.AdoptionRequestDTOs.Add(new AdoptionRequestDTO(adoptRequest));
                }
                else
                {
                    MessageBox.Show("No requests found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        ////////////////////////////////////////
        private List<Domain.Model.TemporaryShelterRequest> GetFilteredTempShelterRequests()
        {

            List<TemporaryShelterRequest> finalRequests = new List<TemporaryShelterRequest>();
            List<TemporaryShelterRequest> temporaryShelterRequests = requestController.GetAllTemporaryShelterRequests();

            foreach(TemporaryShelterRequest temp in temporaryShelterRequests)
            {
                if (temp.RequestStatus == RequestStatus.WaitingForResponse)
                {
                    finalRequests.Add(temp);    
                }
            }
            return finalRequests;
        }
        private List<AdoptionRequest> GetFilteredAdoptionRequests()
        {

            List<AdoptionRequest> finalRequests = new List<Domain.Model.AdoptionRequest>();
            List<AdoptionRequest> adoptionRequests = requestController.GetAllAdoptionRequests();

            foreach (AdoptionRequest req in adoptionRequests)
            {
                if (req.RequestStatus == RequestStatus.WaitingForResponse)
                {
                    finalRequests.Add(req);
                }
            }
            return finalRequests;
            return finalRequests;
        }

        private void acceptAdoptionRequest_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAdoptionRequest == null)
            {
                MessageBox.Show("Please choose request to accept!");
            }
            else
            {
                int postId = SelectedAdoptionRequest.PostId;
                Post post = postController.GetById(postId);
               
                if (post.PostStatus == PostStatus.Adopted || post.PostStatus == PostStatus.TemporarilyAdopted || post.PostStatus == PostStatus.UnderTreatment)
                {
                    MessageBox.Show("Animal cannot be adopted!");
                }
                else
                {
                    post.PostStatus = PostStatus.Adopted;
                    postController.Update(post);
                    SelectedAdoptionRequest.RequestStatus=RequestStatus.Accepted;
                    requestController.Update(SelectedAdoptionRequest.ToAdoptionRequest());
                    MessageBox.Show("Request accepted!");
                }
            }

            Close();
        }

        private void denyAdoptionRequest_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAdoptionRequest == null)
            {
                MessageBox.Show("Please choose request to deny!");
            }
            else
            {
                
                SelectedAdoptionRequest.RequestStatus = RequestStatus.Denied;
                requestController.Update(SelectedAdoptionRequest.ToAdoptionRequest());
                MessageBox.Show("Request rejected!");
                
            }

            Close();
        }

        private void acceptTempRequest_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTempRequest == null)
            {
                MessageBox.Show("Please choose request to accept!");
            }
            else
            {
                int postId = SelectedTempRequest.PostId;
                Post post = postController.GetById(postId);

                if (post.PostStatus == PostStatus.Adopted || post.PostStatus == PostStatus.TemporarilyAdopted || post.PostStatus == PostStatus.UnderTreatment)
                {
                    MessageBox.Show("Animal cannot be adopted!");
                }
                else
                {
                    post.PostStatus = PostStatus.TemporarilyAdopted;
                    postController.Update(post);
                    SelectedAdoptionRequest.RequestStatus = RequestStatus.Accepted;
                    requestController.Update(SelectedTempRequest.ToTemporaryShelterRequest());
                    MessageBox.Show("Request accepted!");
                }
            }

            Close();
        }

        private void denyTempRequest_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTempRequest == null)
            {
                MessageBox.Show("Please choose request to deny!");
            }
            else
            {

                SelectedTempRequest.RequestStatus = RequestStatus.Denied;
                requestController.Update(SelectedTempRequest.ToTemporaryShelterRequest());
                MessageBox.Show("Request rejected!");

            }

            Close();
        }
    }
}
