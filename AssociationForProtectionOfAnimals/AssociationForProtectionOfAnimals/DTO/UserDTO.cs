using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.Domain.Model;

namespace AssociationForProtectionOfAnimals.DTO
{
    public class UserDTO : INotifyPropertyChanged, IDataErrorInfo
    {
        public int id { get; set; }
        private string firstName;
        private string lastName;
        private Gender gender;
        private DateTime dateOfBirth = new DateTime(2000, 1, 1);
        private string phoneNumber;
        private string homeAddress;
        private string username;
        private string password;
        private string idNumber;
        private string placeName;
        private string placeZipCode;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (value != firstName)
                {
                    firstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (value != lastName)
                {
                    lastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }
        public Gender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                dateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
            }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }
        public string HomeAddress
        {
            get { return homeAddress; }
            set
            {
                homeAddress = value;
                OnPropertyChanged("HomeAddress");
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        public string IdNumber
        {
            get { return idNumber; }
            set
            {
                idNumber = value;
                OnPropertyChanged("IdNumber");
            }
        }

        public string PlaceName
        {
            get { return placeName; }
            set
            {
                placeName = value;
                OnPropertyChanged("PlaceName");
            }
        }

        public string PlacePostalCode
        {
            get { return placeZipCode; }
            set
            {
                placeZipCode = value;
                OnPropertyChanged("PlacePostalCode");
            }
        }

        public string? Error => null;

        private Regex _FirstNameRegex = new Regex(@"^[A-Za-z]+$");
        private Regex _LastNameRegex = new Regex(@"^[A-Za-z]+$");
        private Regex _PhoneNumberRegex = new Regex(@"^\d{9,15}$");
        private Regex _UsernameRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9._-]{1,14}[a-zA-Z0-9]$");
        private Regex _IdNumberRegex = new Regex(@"^\d{13}$");
        private Regex _PostalNumberRegex = new Regex(@"^\d{5}$");

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "FirstName":
                        if (string.IsNullOrEmpty(FirstName)) return "First name is required";
                        if (!_FirstNameRegex.Match(FirstName).Success) return "Format not good. Try again.";
                        break;
                    case "LastName":
                        if (string.IsNullOrEmpty(LastName)) return "Last name is required";
                        if (!_LastNameRegex.Match(LastName).Success) return "Format not good. Try again.";
                        break;
                    case "PhoneNumber":
                        if (string.IsNullOrEmpty(PhoneNumber)) return "Phone number is required";
                        if (!_PhoneNumberRegex.Match(PhoneNumber).Success) return "Format not good. Try again.";
                        break;
                    case "Username":
                        if (string.IsNullOrEmpty(Username)) return "Username is required";
                        if (!_UsernameRegex.Match(Username).Success) return "Format not good. Try again.";
                        break;
                    case "IdNumber":
                        if (string.IsNullOrEmpty(IdNumber)) return "IdNumber is required";
                        if (!_IdNumberRegex.Match(IdNumber).Success) return "Format not good. Try again.";
                        break;
                    case "Password":
                        if (string.IsNullOrEmpty(Password)) return "Password is required";
                        break;
                    case "DateOfBirth":
                        if (DateOfBirth < new DateTime(1900, 1, 1) || DateOfBirth > DateTime.Today)
                            return "Invalid Date of birth.";
                        break;
                    case "PlacePostalCode":
                        if (string.IsNullOrEmpty(PlacePostalCode)) return "PostalCode is required";
                        if (!_PostalNumberRegex.Match(PlacePostalCode).Success) return "Format not good. Try again.";
                        break;
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties = { "FirstName", "LastName", "PhoneNumber", "Username", "Password", "DateOfBirth", "IdNumber", "PlacePostalCode" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        public RegisteredUser ToRegisteredUser()
        {
            Account userAccount = new(username, password, AccountType.RegisteredUser, AccountStatus.WaitingForActivation);
            Place userPlace = new(placeName, int.Parse(placeZipCode));
            return new RegisteredUser(firstName, lastName, gender, dateOfBirth, phoneNumber, homeAddress, userPlace, idNumber, userAccount, false);
        }
        public Volunteer ToVolunteer()
        {
            Account userAccount = new(username, password, AccountType.Volunteer, AccountStatus.Active);
            Place userPlace = new(placeName, int.Parse(placeZipCode));
            return new Volunteer(firstName, lastName, gender, dateOfBirth, phoneNumber, homeAddress, userPlace, idNumber, userAccount);
        }

        public UserDTO()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public UserDTO(RegisteredUser user)
        {
            id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Gender = user.Gender;
            DateOfBirth = user.DateOfBirth;
            PhoneNumber = user.PhoneNumber;
            HomeAddress = user.HomeAddress;
            Username = user.Account.Username;
            Password = user.Account.Password;
            PlaceName = user.Place.Name;
            PlacePostalCode = user.Place.PostalCode.ToString();
        }

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
