using AssociationForProtectionOfAnimals.Controller;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.DTO;
using System.Windows;
using System.Windows.Controls;

namespace AssociationForProtectionOfAnimals.View.Administrator
{
    public partial class RegisterVolunteerForm : Window
    {
        public Gender[] genderValues => (Gender[])Enum.GetValues(typeof(Gender));

        public UserDTO user { get; set; }

        private RegisteredUserController registeredUserController;
        private AdministratorController administratorController;

        public RegisterVolunteerForm()
        {
            InitializeComponent();
            user = new UserDTO();
            user.Password = passwordBox.Password;
            registeredUserController = Injector.CreateInstance<RegisteredUserController>();
            administratorController = Injector.CreateInstance<AdministratorController>();
            DataContext = this;

            SetPlaceholders();
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (user.IsValid)
            {
                if (registeredUserController.IsUsernameUnique(user.Username))
                {
                    administratorController.Add(user.ToVolunteer());
                    Close();
                }
                else
                {
                    MessageBox.Show("Username is taken.");
                }
            }
            else
            {
                MessageBox.Show("User can not be created. Not all fields are valid.");
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                user.Password = passwordBox.Password;
            }
        }
        private void SetPlaceholders()
        {
            user.FirstName = "Name";
            user.LastName = "Surname";
            user.Username = "exampleUsername123";
            user.PhoneNumber = "0123456789";
            user.Password = "password123";
            user.HomeAddress = "address example";
            user.PlaceName = "place name example";
            user.PlacePostalCode = "12345";
            user.IdNumber = "1234567898765";
            passwordBox.Password = user.Password;

            firstNameTextBox.GotFocus += FirstNameTextBox_GotFocus;
            lastNameTextBox.GotFocus += LastNameTextBox_GotFocus;
            usernameTextBox.GotFocus += UsernameTextBox_GotFocus;
            phoneNumberTextBox.GotFocus += PhoneNumberTextBox_GotFocus;
            passwordBox.GotFocus += PasswordBox_GotFocus;
            addressTextBox.GotFocus += AddressTextBox_GotFocus;
            placeNameTextBox.GotFocus += PlaceNameTextBox_GotFocus;
            placePostalCodeTextBox.GotFocus += PlacePostalCodeTextBox_GotFocus;
            IdNumberTextBox.GotFocus += IdNumberTextBox_GotFocus;
        }

        private void PlacePostalCodeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            placePostalCodeTextBox.Text = string.Empty;
        }

        private void PlaceNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            placeNameTextBox.Text = string.Empty;
        }

        private void FirstNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            firstNameTextBox.Text = string.Empty;
        }

        private void LastNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            lastNameTextBox.Text = string.Empty;
        }

        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            usernameTextBox.Text = string.Empty;
        }

        private void PhoneNumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            phoneNumberTextBox.Text = string.Empty;
        }
        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordBox.Password = string.Empty;
        }

        private void AddressTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            addressTextBox.Text = string.Empty;
        }

        private void IdNumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            IdNumberTextBox.Text = string.Empty;
        }

    }
}
