using System.Windows;
using System.Windows.Input;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Controller;
using AssociationForProtectionOfAnimals.View.Animal;
using AssociationForProtectionOfAnimals.View.UnregisteredUser;
using AssociationForProtectionOfAnimals.View.Volunteer;
using AssociationForProtectionOfAnimals.View.RegisteredUser;
using AssociationForProtectionOfAnimals.View.Administrator;

namespace AssociationForProtectionOfAnimals
{
    public partial class MainWindow : Window
    {
        private RegisteredUserController registeredUserController { get; set; }
        private AdministratorController adminController { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            adminController = Injector.CreateInstance<AdministratorController>();
            registeredUserController = Injector.CreateInstance<RegisteredUserController>();

            SetPlaceholders();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Password;

            if (HasUserLoggedIn(username, password) || HasVolunteerLoggedIn(username, password) || HasAdminLoggedIn(username, password))
            {
                this.Close();
                return;
            }
            
             MessageBox.Show("User does not exist.");
        } 

        private bool HasUserLoggedIn(string username, string password)
        {
            foreach (RegisteredUser user in registeredUserController.GetEveryUser())
            {
                if (user.Account.Username == username && user.Account.Password == password)
                {
                    if (user.Account.Status== Domain.Model.Enums.AccountStatus.Active)
                    {
                        RegisteredUserPage welcomePage = new RegisteredUserPage(user);
                        welcomePage.Show();
                    }
                    else if(user.Account.Status== Domain.Model.Enums.AccountStatus.WaitingForActivation)
                    {
                        MessageBox.Show("Account registration request sent, waiting for volunteer's response");

                    }
                    else if (user.Account.Status == Domain.Model.Enums.AccountStatus.Denied)
                    {
                        MessageBox.Show("Your registration request has been denied. Please try sending another one.");
                    }
                    return true;
                }
            }
            return false;
        }

        private bool HasVolunteerLoggedIn(string username, string password)
        {
            foreach (Volunteer volunteer in adminController.GetAllVolunteers())
            {
                if (volunteer.Account.Username == username && volunteer.Account.Password == password)
                {
                    VolunteerPage volunteerPage = new VolunteerPage(volunteer);
                    volunteerPage.Show();
                    return true;
                }
            }
            return false;
        }

        private bool HasAdminLoggedIn(string username, string password)
        {
            Administrator director = adminController.GetAdministrator();

            if (director.Account.Username == username && director.Account.Password == password)
            {
                AdminPage directorPage = new AdminPage();
                directorPage.Show();
                return true;
            }
            return false;
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            View.UnregisteredUser.RegistrationForm registrationForm = new View.UnregisteredUser.RegistrationForm();
            registrationForm.Show();
        }

        private void Unregistered_Click(object sender, RoutedEventArgs e)
        {
            UnregisteredUserPage unregisteredUserPage = new UnregisteredUserPage();
            unregisteredUserPage.Show();
        }

        private void SetPlaceholders()
        {
            UsernamePlaceholder.Visibility = Visibility.Visible;
            PasswordPlaceholder.Visibility = Visibility.Visible;

            Username.GotFocus += UsernameTextBox_GotFocus;
            Password.GotFocus += PasswordBox_GotFocus;

            Username.LostFocus += UsernameTextBox_LostFocus;
            Password.LostFocus += PasswordBox_LostFocus;

            UsernamePlaceholder.MouseDown += Placeholder_MouseDown;
            PasswordPlaceholder.MouseDown += Placeholder_MouseDown;
        }

        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UsernamePlaceholder.Visibility = Visibility.Collapsed;
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Username.Text))
                UsernamePlaceholder.Visibility = Visibility.Visible;
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Password.Password))
                PasswordPlaceholder.Visibility = Visibility.Visible;
        }

        private void Placeholder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender == UsernamePlaceholder)
            {
                UsernamePlaceholder.Visibility = Visibility.Collapsed;
                Username.Focus();
            }
            else if (sender == PasswordPlaceholder)
            {
                PasswordPlaceholder.Visibility = Visibility.Collapsed;
                Password.Focus();
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordPlaceholder != null)
                PasswordPlaceholder.Visibility = string.IsNullOrEmpty(Password.Password) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}

