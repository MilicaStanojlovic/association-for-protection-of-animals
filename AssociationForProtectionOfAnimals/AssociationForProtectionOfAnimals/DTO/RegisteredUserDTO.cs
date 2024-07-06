using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssociationForProtectionOfAnimals.DTO
{
    public class RegisteredUserDTO
    {
        private int id { get; }
        private string firstName;
        private string lastName;
        private Gender gender;
        private DateTime dateOfBirth = new DateTime(2000, 1, 1);
        private string phoneNumber;
        private string homeAddress;
        private Account account;
        private string idNumber;
        private Place place;
        private bool isBlackListed;

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
        public string IdNumber
        {
            get { return idNumber; }
            set
            {
                idNumber = value;
                OnPropertyChanged("IdNumber");
            }
        }
        public Account Account
        {
            get { return account; }
            set
            {
                account = value;
                OnPropertyChanged("Account");
            }
        }
        public Place Place
        {
            get { return place; }
            set
            {
                place = value;
                OnPropertyChanged("Place");
            }
        }
        public bool IsBlackListed
        {
            get { return isBlackListed; }
            set
            {
                isBlackListed = value;
                OnPropertyChanged("IsBlackListed");
            }
        }

        public RegisteredUser ToRegisteredUser()
        {
            return new RegisteredUser(firstName, lastName, gender, dateOfBirth, phoneNumber, homeAddress, place, idNumber, account, false);
        }

        public RegisteredUserDTO()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public RegisteredUserDTO(RegisteredUser user)
        {
            id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Gender = user.Gender;
            DateOfBirth = user.DateOfBirth;
            PhoneNumber = user.PhoneNumber;
            HomeAddress = user.HomeAddress;
            Place = user.Place;
            Account = user.Account;
            IsBlackListed = user.IsBlackListed;
            IdNumber = user.IdNumber;
        }

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
