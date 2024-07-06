using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using AssociationForProtectionOfAnimals.Controller;
using AssociationForProtectionOfAnimals.DTO;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Domain.IUtility;
using AssociationForProtectionOfAnimals.Domain.Utility;
using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.View.UnregisteredUser;
using AssociationForProtectionOfAnimals.View.Animal;
using AssociationForProtectionOfAnimals.View.RegisteredUser;

namespace AssociationForProtectionOfAnimals.View.Volunteer
{
    public partial class VolunteerPage : Window, IObserver
    {
        public ObservableCollection<PostDTO>? PublishedPosts { get; set; }
        public ObservableCollection<PostDTO>? UnpublishedPosts { get; set; }
        public ObservableCollection<DonationDTO> Donations { get; set; }
        public ObservableCollection<RegisteredUserDTO>? Users { get; set; }
        public ObservableCollection<RegisteredUserDTO>? ReqUsers { get; set; }


        public class ViewModel
        {
            public ObservableCollection<PostDTO>? PublishedPosts { get; set; }
            public ObservableCollection<PostDTO>? UnpublishedPosts { get; set; }
            public ObservableCollection<RegisteredUserDTO> Users { get; set; }
            public ObservableCollection<RegisteredUserDTO>? ReqUsers { get; set; }

            public ObservableCollection<AdoptionRequestDTO> Requests { get; set; }
            public ObservableCollection<AdoptionRequestDTO> AdoptionRequestDTOs { get; set; }
            public ObservableCollection<TemporaryShelterRequestDTO> TempShelterRequestDTOs { get; set; }
            public ObservableCollection<DonationDTO> Donations { get; set; }
            public ViewModel()
            {
                Users = new ObservableCollection<RegisteredUserDTO>();
                ReqUsers = new ObservableCollection<RegisteredUserDTO>();
                UnpublishedPosts = new ObservableCollection<PostDTO>();
                PublishedPosts = new ObservableCollection<PostDTO>();
                AdoptionRequestDTOs = new ObservableCollection<AdoptionRequestDTO>();
                TempShelterRequestDTOs = new ObservableCollection<TemporaryShelterRequestDTO>();
                Donations = new ObservableCollection<DonationDTO>();
            }
        }

        private readonly Domain.Model.RegisteredUser user;
        private readonly VolunteerController _volunteerController;
        private readonly RegisteredUserController _regUserController;
        private readonly PostController _postController;
        private readonly RequestController _requestController;
        private readonly DonationController _donationController;

        private readonly IPlaceRepo _placeRepo;

        public RegisteredUserDTO? SelectedUser { get; set; }
        public RegisteredUserDTO? ReqSelectedUser { get; set; }

        public PostDTO? SelectedPublishedPost { get; set; }
        public PostDTO? SelectedUnpublishedPost { get; set; }
        public AdoptionRequestDTO SelectedAdoptionRequest { get; set; }
        public TemporaryShelterRequestDTO SelectedTempRequest { get; set; }

        public ViewModel TableViewModel { get; set; }

        private bool isSearchButtonClicked = false;
        private int currentUserPage = 1;
        private string UsersortCriteria;
        private IUserSortStrategy sortStrategy = new SortByDatetime();

        private int currentPostPage = 1;
        private string postSortCriteria = "AnimalBreed";
        private ISortStrategy postSortStrategy = new SortByBreed();

        private DonationDTO selectedDonation;

        public VolunteerPage(Domain.Model.RegisteredUser user)
        {
            InitializeComponent();
            this.user = user;
            _volunteerController = Injector.CreateInstance<VolunteerController>();
            _postController = Injector.CreateInstance<PostController>();
            _regUserController = Injector.CreateInstance<RegisteredUserController>();
            _placeRepo = Injector.CreateInstance<IPlaceRepo>();
            _requestController = Injector.CreateInstance<RequestController>();
            _donationController = Injector.CreateInstance<DonationController>();

            TableViewModel = new ViewModel();
            DataContext = this;
            _volunteerController.Subscribe(this);
            _regUserController.Subscribe(this);

            firstAndLastName.Text = user.FirstName + " " + user.LastName;

            Update();
            UpdatePagination();
        }

        private void RefreshPage(object? sender, EventArgs e)
        {
            Update();
        }

        public void Update()
        {
            try
            {
                SetUsers();
                SetReqUsers();
                SetPosts();
                SetRequests();
                SetDonations();
                UpdatePagination();
                UpdatePostPagination();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            this.Close();
        }
        private void SetRequests()
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
            if (adoptionRequests != null)
            {
                foreach (Domain.Model.AdoptionRequest adoptRequest in adoptionRequests)
                    TableViewModel.AdoptionRequestDTOs.Add(new AdoptionRequestDTO(adoptRequest));
            }
        }
        private void SetUsers()
        {
            TableViewModel.Users.Clear();
            var users = _volunteerController.GetAllRegisteredUsers();

            if (users != null)
            {
                foreach (Domain.Model.RegisteredUser user in users)
                {
                    TableViewModel.Users.Add(new RegisteredUserDTO(user));
                }
            }
        }
        private void SetReqUsers()
        {
            TableViewModel.ReqUsers.Clear();
            var users = _volunteerController.GetAllRegistrationRequests();

            if (users != null)
            {
                foreach (Domain.Model.RegisteredUser user in users)
                {
                    TableViewModel.ReqUsers.Add(new RegisteredUserDTO(user));
                }
            }
        }

        private void SetPosts()
        {
            TableViewModel.UnpublishedPosts.Clear();
            TableViewModel.PublishedPosts.Clear();
            var publishedPosts = _postController.GetAllPublishedPosts();
            var unpublishedPosts = _postController.GetAllUnpublishedPosts();

            if (publishedPosts != null)
                foreach (Post post in publishedPosts)
                    TableViewModel.PublishedPosts.Add(new PostDTO(post));

            if (unpublishedPosts != null)
                foreach (Post post in unpublishedPosts)
                    TableViewModel.UnpublishedPosts.Add(new PostDTO(post));
        }

        private void SetDonations()
        {
            TableViewModel.Donations.Clear();
            var donations = _donationController.GetAllDonations();

            if (donations != null)
                foreach (Donation donation in donations)
                    TableViewModel.Donations.Add(new DonationDTO(donation));
        }

        private void SearchUsers_Click(object sender, RoutedEventArgs e)
        {
            UpdateSearch();
            UpdatePagination();
            isSearchButtonClicked = true;
        }

        public void UpdateSearch()
        {
            try
            {
                TableViewModel.Users.Clear();
                List<Domain.Model.RegisteredUser> Users = GetFilteredUsers();

                if (Users != null)
                {
                    foreach (Domain.Model.RegisteredUser User in Users)
                    {
                        TableViewModel.Users.Add(new RegisteredUserDTO(User));
                    }
                }
                else
                {
                    MessageBox.Show("No Users found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void ResetUsers_Click(object sender, RoutedEventArgs e)
        {
            isSearchButtonClicked = false;
            Update();
            ResetSearchElements();
            UpdatePagination();
        }

        private void ResetSearchElements()
        {
            firstNameTextBox.Text = string.Empty;
            lastNameTextBox.Text = string.Empty;
            placeTextBox.Text = string.Empty;
            dateOfBirthDatePicker.SelectedDate = null;
        }

        private List<Domain.Model.RegisteredUser> GetFilteredUsers()
        {
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            Place place = _placeRepo.GetPlaceByName(placeTextBox.Text);
            DateTime? dateOfBirth = dateOfBirthDatePicker.SelectedDate;

            return _volunteerController.FindRegisteredUsersByCriteria(firstName, lastName, place, dateOfBirth);
        }

        private void UserNextPage_Click(object sender, RoutedEventArgs e)
        {
            currentUserPage++;
            UserPreviousButton.IsEnabled = true;
            UpdatePagination();
        }

        private void UserPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentUserPage > 1)
            {
                currentUserPage--;
                UserNextButton.IsEnabled = true;
                UpdatePagination();
            }
            else if (currentUserPage == 1)
            {
                UserPreviousButton.IsEnabled = false;
            }
        }

        private void UpdatePagination()
        {
            if (currentUserPage == 1)
            {
                UserPreviousButton.IsEnabled = false;
            }
            UserPageNumberTextBlock.Text = $"{currentUserPage}";

            try
            {
                TableViewModel.Users.Clear();
                var filteredUsers = GetFilteredUsers();
                List<Domain.Model.RegisteredUser> Users = _volunteerController.GetAllRegisteredUsers(currentUserPage, 2, sortStrategy, filteredUsers);
                List<Domain.Model.RegisteredUser> newUsers = _volunteerController.GetAllRegisteredUsers(currentUserPage + 1, 2, sortStrategy, filteredUsers);

                if (newUsers.Count == 0)
                {
                    UserNextButton.IsEnabled = false;
                }
                else
                {
                    UserNextButton.IsEnabled = true;
                }

                if (filteredUsers != null)
                {
                    foreach (Domain.Model.RegisteredUser User in Users)
                    {
                        TableViewModel.Users.Add(new RegisteredUserDTO(User));
                    }
                }
                else
                {
                    MessageBox.Show("No Users found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void UsersortCriteriaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersortCriteriaComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedContent = selectedItem.Content.ToString();
                switch (selectedContent)
                {
                    case "DateOfBirth":
                        UsersortCriteria = "DateOfBirth";
                        sortStrategy = new SortByDatetime();
                        break;
                }
                UpdatePagination();
            }
        }

        private void Suggestion_Click(object sender, RoutedEventArgs e)
        {
           
        }

        // -------------------------------------- POSTS -------------------------------------------

        private void CreatePost_Click(object sender, RoutedEventArgs e)
        {
            Animal.CreateAnimal createAnimal = new CreateAnimal(user.Id, this);
            createAnimal.Closed += RefreshPage;
            createAnimal.Show();
            Update();
        }

        private void UpdatePost_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPublishedPost == null)
                MessageBox.Show("Please choose a post to update!");
            else
            {
                /*UpdateTeacherForm updateTeacherForm = new UpdateTeacherForm(SelectedTeacher.Id);
                updateTeacherForm.Show();
                updateTeacherForm.Activate();*/
                Update();
            }
        }

        private void ViewPost_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPublishedPost == null)
                MessageBox.Show("Please choose a post to view!");
            else
            {
                PostView postView = new PostView(SelectedPublishedPost.ToPost(), user, this);
                postView.Owner = this;
                this.Visibility = Visibility.Collapsed;
                postView.Show();
                Update();
            }
        }

        public void UpdatePostSearch()
        {
            try
            {
                TableViewModel.PublishedPosts.Clear();
                List<Post> posts = GetFilteredPosts();

                if (posts != null)
                    foreach (Post post in posts)
                        TableViewModel.PublishedPosts.Add(new PostDTO(post));
                else
                    MessageBox.Show("No courses found.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void SearchPosts_Click(object sender, RoutedEventArgs e)
        {
            UpdatePostSearch();
            UpdatePostPagination();
            isSearchButtonClicked = true;
        }

        private void ResetPosts_Click(object sender, EventArgs e)
        {
            isSearchButtonClicked = false;
            Update();
            ResetPostSearchElements();
            UpdatePostPagination();
        }

        private void ResetPostSearchElements()
        {
            postStatusComboBox.SelectedItem = null;
            animalBreedComboBox.SelectedItem = null;
            postStartDateDatePicker.SelectedDate = null;
            minAnimalYearsTextBox.Text = "";
            maxAnimalYearsTextBox.Text = "";
        }

        private void PostNextPage_Click(object sender, RoutedEventArgs e)
        {

            currentPostPage++;
            PostPreviousButton.IsEnabled = true;
            UpdatePostPagination();

        }

        private void PostPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPostPage > 1)
            {
                currentPostPage--;
                PostNextButton.IsEnabled = true;
                UpdatePostPagination();
            }
            else if (currentPostPage == 1)
            {
                PostPreviousButton.IsEnabled = false;
            }
        }

        private void AcceptRequest_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUnpublishedPost == null)
                MessageBox.Show("Please choose a post to update!");
            else
            {
                _volunteerController.AcceptPostRequest(SelectedUnpublishedPost.ToPost());
                Update();
            }
        }
        private void RejectRequest_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUnpublishedPost == null)
                MessageBox.Show("Please choose a post to update!");
            else
            {
                _volunteerController.RejectPostRequest(SelectedUnpublishedPost.ToPost());
                Update();
            }
        }

        public void UpdatePostPagination()
        {
            if (currentPostPage == 1)
            {
                PostPreviousButton.IsEnabled = false;
            }
            PostPageNumberTextBlock.Text = $"{currentPostPage}";

            try
            {
                TableViewModel.PublishedPosts.Clear();
                var filteredPosts = GetFilteredPosts();

                List<Post> posts = _postController.GetAllPosts(currentPostPage, 4, postSortCriteria, filteredPosts);
                List<Post> newPosts = _postController.GetAllPosts(currentPostPage + 1, 4, postSortCriteria, filteredPosts);

                if (newPosts.Count == 0)
                    PostNextButton.IsEnabled = false;
                else
                    PostNextButton.IsEnabled = true;
                if (filteredPosts != null)
                {
                    foreach (Post post in posts)
                        TableViewModel.PublishedPosts.Add(new PostDTO(post));
                }
                else
                {
                    MessageBox.Show("No exam terms found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private List<Post>? GetFilteredPosts()
        {
            PostStatus? selectedPostStatus = PostStatus.NULL;
            string? selectedBreed = null;
            DateTime? selectedStartDate = DateTime.MinValue;
            int selectedMinYears = 0;
            int selectedMaxYears = 0;

            if (!string.IsNullOrEmpty(minAnimalYearsTextBox.Text))
            {
                if (int.TryParse(minAnimalYearsTextBox.Text, out int duration))
                {
                    selectedMinYears = duration;
                }
            }

            if (!string.IsNullOrEmpty(maxAnimalYearsTextBox.Text))
            {
                if (int.TryParse(maxAnimalYearsTextBox.Text, out int duration))
                {
                    selectedMaxYears = duration;
                }
            }

            if (postStatusComboBox.SelectedItem != null)
                selectedPostStatus = (PostStatus)postStatusComboBox.SelectedItem;

            if (animalBreedComboBox.SelectedItem != null)
                selectedBreed = animalBreedComboBox.SelectedItem.ToString();

            if (postStartDateDatePicker.SelectedDate.HasValue)
                selectedStartDate = (DateTime)postStartDateDatePicker.SelectedDate;

            return GetPostsForDisplay(selectedPostStatus, selectedBreed, selectedStartDate, selectedMinYears, selectedMaxYears);
        }

        private List<Post> GetPostsForDisplay(PostStatus? selectedPostStatus, string selectedBreed, DateTime? selectedStartDate, int selectedMinYears, int selectedMaxYears)
        {
            List<Post> finalPosts = new();

            if (isSearchButtonClicked)
            {
                List<Post> allFilteredPosts = _postController.FindPostsByCriteria(selectedPostStatus, selectedBreed, selectedStartDate, selectedMinYears, selectedMaxYears);

                foreach (Post post in allFilteredPosts)
                    finalPosts.Add(post);
            }
            else
            {
                foreach (Post post in _postController.GetAllPublishedPosts())
                    finalPosts.Add(post);
            }

            return finalPosts;
        }

        private void PostSortCriteriaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (postSortCriteriaComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedContent = selectedItem.Content.ToString();
                switch (selectedContent)
                {
                    case "DateOfPosting":
                        postSortCriteria = "DateOfPosting";
                        postSortStrategy = new SortByDatetime();
                        break;
                    case "PostStatus":
                        postSortCriteria = "PostStatus";
                        postSortStrategy = new SortByPostStatus();
                        break;
                    case "AnimalBreed":
                        postSortCriteria = "AnimalBreed";
                        postSortStrategy = new SortByBreed();
                        break;
                    case "AnimalYears":
                        postSortCriteria = "AnimalYears";
                        postSortStrategy = new SortByAge();
                        break;
                }
                UpdatePostPagination();
            }
        }

        // ======================================== REQUESTS ================================================
        private List<Domain.Model.TemporaryShelterRequest> GetFilteredTempShelterRequests()
        {

            List<TemporaryShelterRequest> finalRequests = new List<TemporaryShelterRequest>();
            List<TemporaryShelterRequest> temporaryShelterRequests = _requestController.GetAllTemporaryShelterRequests();

            foreach (TemporaryShelterRequest temp in temporaryShelterRequests)
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
            List<AdoptionRequest> adoptionRequests = _requestController.GetAllAdoptionRequests();

            foreach (AdoptionRequest req in adoptionRequests)
            {
                if (req.RequestStatus == RequestStatus.WaitingForResponse)
                {
                    finalRequests.Add(req);
                }
            }
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
                Post post = _postController.GetById(postId);

                if (post.PostStatus == PostStatus.Adopted || post.PostStatus == PostStatus.TemporarilyAdopted || post.PostStatus == PostStatus.UnderTreatment)
                {
                    MessageBox.Show("Animal cannot be adopted!");
                }
                else
                {
                    post.PostStatus = PostStatus.Adopted;
                    _postController.Update(post);
                    SelectedAdoptionRequest.RequestStatus = RequestStatus.Accepted;
                    SelectedAdoptionRequest.VolunteerId = user.Id;
                    _requestController.Update(SelectedAdoptionRequest.ToAdoptionRequest());
                    MessageBox.Show("Request accepted!");
                    Update();
                }
            }

            //Close();
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
                SelectedAdoptionRequest.VolunteerId = user.Id;
                _requestController.Update(SelectedAdoptionRequest.ToAdoptionRequest());
                MessageBox.Show("Request rejected!");
                Update();

            }

            //Close();
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
                Post post = _postController.GetById(postId);

                if (post.PostStatus == PostStatus.Adopted || post.PostStatus == PostStatus.TemporarilyAdopted || post.PostStatus == PostStatus.UnderTreatment)
                {
                    MessageBox.Show("Animal cannot be adopted!");
                }
                else
                {
                    post.PostStatus = PostStatus.TemporarilyAdopted;
                    _postController.Update(post);
                    SelectedTempRequest.RequestStatus = RequestStatus.Accepted;
                    SelectedTempRequest.VolunteerId = user.Id;
                    _requestController.Update(SelectedTempRequest.ToTemporaryShelterRequest());
                    MessageBox.Show("Request accepted!");
                    Update();
                }
            }

            //Close();
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
                SelectedTempRequest.VolunteerId = user.Id;
                _requestController.Update(SelectedTempRequest.ToTemporaryShelterRequest());
                MessageBox.Show("Request rejected!");
                Update();

            }

            //Close();
        }

        // -------------------------------- DONATION -----------------------------

        private void CreateDonationButton_Click(object sender, RoutedEventArgs e)
        {
            CreateDonationForm createDonationForm = new CreateDonationForm();
            createDonationForm.Closed += RefreshPage;
            createDonationForm.Show();
        }

        // -------------------------- REGISTRATION REQUESTS ------------------------

        private void AcceptRegistrationRequest_Click(object sender, RoutedEventArgs e)
        {
            _volunteerController.AcceptRegistration(ReqSelectedUser.ToRegisteredUser());
            Update();
        }

        private void RejectRegistrationRequest_Click(object sender, RoutedEventArgs e)
        {
            _volunteerController.DenyRegistration(ReqSelectedUser.ToRegisteredUser());
            Update();
        }
    }
}