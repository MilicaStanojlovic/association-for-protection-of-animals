using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using AssociationForProtectionOfAnimals.Controller;
using AssociationForProtectionOfAnimals.DTO;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Domain.IUtility;
using AssociationForProtectionOfAnimals.Domain.Utility;
using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.View.UnregisteredUser;
using AssociationForProtectionOfAnimals.View.Animal;

namespace AssociationForProtectionOfAnimals.View.Administrator
{
    public partial class AdminPage : Window, IObserver
    {
        public ObservableCollection<PostDTO>? Posts { get; set; }

        public ObservableCollection<RegisteredUserDTO>? Users { get; set; }
        public ObservableCollection<RegisteredUserDTO>? Volunteers { get; set; }

        public class ViewModel
        {
            public ObservableCollection<PostDTO> Posts { get; set; }
            public ObservableCollection<RegisteredUserDTO> Users { get; set; }
            public ObservableCollection<RegisteredUserDTO>? Volunteers { get; set; }

            public ViewModel()
            {
                Users = new ObservableCollection<RegisteredUserDTO>();
                Volunteers = new ObservableCollection<RegisteredUserDTO>();
                Posts = new ObservableCollection<PostDTO>();
            }
        }

        private readonly VolunteerController _volunteerController;
        private readonly AdministratorController _adminController;
        private readonly PostController _postController;
        private readonly IPlaceRepo _placeRepo;

        public RegisteredUserDTO? SelectedUser { get; set; }
        public RegisteredUserDTO? SelectedVolunteer { get; set; }
        public PostDTO? SelectedPost { get; set; }
        public ViewModel TableViewModel { get; set; }

        private bool isSearchButtonClicked = false;
        private bool isSearchVolunteerButtonClicked = false;
        private int currentUserPage = 1;
        private int currentVolunteerPage = 1;
        private string UserSortCriteria;
        private string VolunteerSortCriteria;

        private IUserSortStrategy sortStrategy = new SortByDatetime();
        private IUserSortStrategy sortVolunteerStrategy = new SortByDatetime();

        private int currentPostPage = 1;
        private string postSortCriteria = "AnimalBreed";
        private ISortStrategy sortPostStrategy = new SortByBreed();

        Domain.Model.Administrator administrator;


        public AdminPage()
        {
            InitializeComponent();
            _volunteerController = Injector.CreateInstance<VolunteerController>();
            _placeRepo = Injector.CreateInstance<IPlaceRepo>();
            _adminController = Injector.CreateInstance<AdministratorController>();
            _postController = Injector.CreateInstance<PostController>();

            TableViewModel = new ViewModel();
            DataContext = this;
            _volunteerController.Subscribe(this);
            _adminController.Subscribe(this);

            administrator = _adminController.GetAdministrator();
            /*if (_adminController.GetAllVolunteers().Count != 0)
            {
                CreateVolunteerBtn.Visibility = Visibility.Collapsed;
            }*/

            firstAndLastName.Text = administrator.FirstName + " " + administrator.LastName;

            Update();
            UpdatePagination();
            UpdatePostPagination();
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
                SetVolunteers();
                UpdatePagination();
                UpdateVolunteerPagination();
                UpdatePostPagination();
                SetPosts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
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

        private void SetVolunteers()
        {
            TableViewModel.Volunteers.Clear();
            var volunteers = _adminController.GetAllVolunteers();

            if (volunteers != null)
            {
                foreach (Domain.Model.Volunteer volunteer in volunteers)
                {
                    TableViewModel.Volunteers.Add(new RegisteredUserDTO(volunteer));
                }
            }
        }

        private void SearchUsers_Click(object sender, RoutedEventArgs e)
        {
            UpdateSearch();
            UpdatePagination();
            isSearchButtonClicked = true;
        }

        private void SearchVolunteers_Click(object sender, RoutedEventArgs e)
        {
            UpdateVolunteerSearch();
            UpdateVolunteerPagination();
            isSearchVolunteerButtonClicked = true;
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
        public void UpdateVolunteerSearch()
        {
            try
            {
                TableViewModel.Volunteers.Clear();
                List<Domain.Model.Volunteer> volunteers = GetFilteredVolunteers();

                if (volunteers != null)
                {
                    foreach (Domain.Model.Volunteer volunteer in volunteers)
                    {
                        TableViewModel.Volunteers.Add(new RegisteredUserDTO(volunteer));
                    }
                }
                else
                {
                    MessageBox.Show("No volunteers found.");
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
            ResetUserSearchElements();
            UpdatePagination();
        }
        private void ResetVolunteers_Click(object sender, RoutedEventArgs e)
        {
            isSearchVolunteerButtonClicked = false;
            Update();
            ResetVolunteerSearchElements();
            UpdateVolunteerPagination();
        }

        private void ResetUserSearchElements()
        {
            firstNameTextBox.Text = string.Empty;
            lastNameTextBox.Text = string.Empty;
            placeTextBox.Text = string.Empty;
            dateOfBirthDatePicker.SelectedDate = null;
        }
        private void ResetVolunteerSearchElements()
        {
            volFirstNameTextBox.Text = string.Empty;
            volLastNameTextBox.Text = string.Empty;
            volPlaceTextBox.Text = string.Empty;
            volDateOfBirthDatePicker.SelectedDate = null;
        }

        private List<Domain.Model.RegisteredUser> GetFilteredUsers()
        {
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            Place place = _placeRepo.GetPlaceByName(placeTextBox.Text);
            DateTime? dateOfBirth = dateOfBirthDatePicker.SelectedDate;

            return _volunteerController.FindRegisteredUsersByCriteria(firstName, lastName, place, dateOfBirth);
        }

        private List<Domain.Model.Volunteer> GetFilteredVolunteers()
        {
            string firstName = volFirstNameTextBox.Text;
            string lastName = volLastNameTextBox.Text;
            Place place = _placeRepo.GetPlaceByName(volPlaceTextBox.Text);
            DateTime? dateOfBirth = volDateOfBirthDatePicker.SelectedDate;

            return _adminController.FindVolunteersByCriteria(firstName, lastName, place, dateOfBirth);
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

        private void VolunteerNextPage_Click(object sender, RoutedEventArgs e)
        {
            currentVolunteerPage++;
            VolunteerPreviousButton.IsEnabled = true;
            UpdateVolunteerPagination();
        }

        private void VolunteerPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentVolunteerPage > 1)
            {
                currentVolunteerPage--;
                VolunteerNextButton.IsEnabled = true;
                UpdateVolunteerPagination();
            }
            else if (currentVolunteerPage == 1)
            {
                VolunteerPreviousButton.IsEnabled = false;
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
        private void UpdateVolunteerPagination()
        {
            if (currentVolunteerPage == 1)
            {
                VolunteerPreviousButton.IsEnabled = false;
            }
            VolunteerPageNumberTextBlock.Text = $"{currentVolunteerPage}";

            try
            {
                TableViewModel.Volunteers.Clear();
                var filteredVolunteers = GetFilteredVolunteers();
                List<Domain.Model.Volunteer> volunteers = _adminController.GetAllVolunteers(currentVolunteerPage, 2, sortVolunteerStrategy, filteredVolunteers);
                List<Domain.Model.Volunteer> newVolunteers = _adminController.GetAllVolunteers(currentVolunteerPage + 1, 2, sortVolunteerStrategy, filteredVolunteers);

                if (newVolunteers.Count == 0)
                {
                    VolunteerNextButton.IsEnabled = false;
                }
                else
                {
                    VolunteerNextButton.IsEnabled = true;
                }

                if (filteredVolunteers != null)
                {
                    foreach (Domain.Model.Volunteer volunteer in volunteers)
                    {
                        TableViewModel.Volunteers.Add(new RegisteredUserDTO(volunteer));
                    }
                }
                else
                {
                    MessageBox.Show("No volunters found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void UserSortCriteriaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserSortCriteriaComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedContent = selectedItem.Content.ToString();
                switch (selectedContent)
                {
                    /*case "FirstName":
                        UsersortCriteria = "FirstName";
                        sortStrategy = new SortByFirstName();
                        break;
                    case "LastName":
                        UsersortCriteria = "LastName";
                        sortStrategy = new SortByLastName();
                        break;*/
                    case "DateOfBirth":
                        UserSortCriteria = "DateOfBirth";
                        sortStrategy = new SortByDatetime();
                        break;
                    /*case "IsBlackListed":
                        UsersortCriteria = "IsBlackListed";
                        sortStrategy = new SortByDatetime();
                        break;*/
                }
                UpdatePagination();
            }
        }

        private void VolunteerSortCriteriaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VolunteerSortCriteriaComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedContent = selectedItem.Content.ToString();
                switch (selectedContent)
                {
                    /*case "FirstName":
                        UsersortCriteria = "FirstName";
                        sortStrategy = new SortByFirstName();
                        break;
                    case "LastName":
                        UsersortCriteria = "LastName";
                        sortStrategy = new SortByLastName();
                        break;*/
                    case "DateOfBirth":
                        VolunteerSortCriteria = "DateOfBirth";
                        sortVolunteerStrategy = new SortByDatetime();
                        break;
                        /*case "IsBlackListed":
                            UsersortCriteria = "IsBlackListed";
                            sortStrategy = new SortByDatetime();
                            break;*/
                }
                UpdatePagination();
            }
        }

        private void Suggestion_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateVolunteer_Click(object sender, RoutedEventArgs e)
        {
            RegisterVolunteerForm form = new RegisterVolunteerForm();
            form.Show();
        }

        // --------------------------------------------- POST ------------------------------------------------

        private void SetPosts()
        {
            TableViewModel.Posts.Clear();
            var posts = _postController.GetAllPublishedPosts();

            if (posts != null)
                foreach (Post post in posts)
                    TableViewModel.Posts.Add(new PostDTO(post));
        }

        public void UpdatePostSearch()
        {
            try
            {
                TableViewModel.Posts.Clear();
                List<Post> posts = GetFilteredPosts();

                if (posts != null)
                    foreach (Post post in posts)
                        TableViewModel.Posts.Add(new PostDTO(post));
                else
                    MessageBox.Show("No courses found.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void CreatePost_Click(object sender, RoutedEventArgs e)
        {
            Animal.CreateAnimal createAnimal = new CreateAnimal(administrator.Id, this);
            createAnimal.Closed += RefreshPage;
            createAnimal.Show();
            Update();
        }

        private void UpdatePost_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPost == null)
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
            if (SelectedPost == null)
                MessageBox.Show("Please choose a post to view!");
            else
            {
                PostView postView = new PostView(SelectedPost.ToPost(), administrator, this);
                postView.Owner = this;
                this.Visibility = Visibility.Collapsed;
                postView.Show();
                Update();
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

        // ------------------------- PAGINATION -----------------------

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

        public void UpdatePostPagination()
        {
            if (currentPostPage == 1)
            {
                PostPreviousButton.IsEnabled = false;
            }
            PostPageNumberTextBlock.Text = $"{currentPostPage}";

            try
            {
                TableViewModel.Posts.Clear();
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
                        TableViewModel.Posts.Add(new PostDTO(post));
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
                        sortPostStrategy = new SortByDatetime();
                        break;
                    case "PostStatus":
                        postSortCriteria = "PostStatus";
                        sortPostStrategy = new SortByPostStatus();
                        break;
                    case "AnimalBreed":
                        postSortCriteria = "AnimalBreed";
                        sortPostStrategy = new SortByBreed();
                        break;
                    case "AnimalYears":
                        postSortCriteria = "AnimalYears";
                        sortPostStrategy = new SortByAge();
                        break;
                }
                UpdatePostPagination();
            }
        }
    }
}