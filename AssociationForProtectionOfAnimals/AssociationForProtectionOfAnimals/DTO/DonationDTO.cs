using System.ComponentModel;
using System.Runtime.CompilerServices;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;

namespace AssociationForProtectionOfAnimals.DTO
{
    public class DonationDTO : INotifyPropertyChanged, IDataErrorInfo
    {
        private int id;
        private DateTime dateOfDonation;
        private int value;
        private string authorFirstName;
        private string pdfFilePath;
        private string authorLastName;
        private TypeOfDonation typeOfDonation;

        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        public DateTime DateOfDonation
        {
            get { return dateOfDonation; }
            set { SetProperty(ref dateOfDonation, value); }
        }

        public int Value
        {
            get { return value; }
            set { SetProperty(ref this.value, value); }
        }

        public string AuthorFirstName
        {
            get { return authorFirstName; }
            set { SetProperty(ref authorFirstName, value); }
        }

        public string PdfFilePath
        {
            get { return pdfFilePath; }
            set { SetProperty(ref pdfFilePath, value); }
        }

        public string AuthorLastName
        {
            get { return authorLastName; }
            set { SetProperty(ref authorLastName, value); }
        }

        public TypeOfDonation TypeOfDonation
        {
            get { return typeOfDonation; }
            set { SetProperty(ref typeOfDonation, value); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Value):
                        if (Value < 0)
                            return "Value cannot be negative";
                        break;
                    default:
                        break;
                }
                return null;
            }
        }

        public Donation ToDonation()
        {
            return new Donation
            {
                Id = id,
                DateOfDonation = dateOfDonation,
                Value = value,
                AuthorFirstName = authorFirstName,
                AuthorLastName = authorLastName,
                PdfFilePath = pdfFilePath,
                TypeOfDonation = typeOfDonation
            };
        }

        public DonationDTO() { }

        public DonationDTO(Donation donation)
        {
            id = donation.Id;
            dateOfDonation = donation.DateOfDonation;
            value = donation.Value;
            authorFirstName = donation.AuthorFirstName;
            authorLastName = donation.AuthorLastName;
            pdfFilePath = donation.PdfFilePath;
            typeOfDonation = donation.TypeOfDonation;
        }
    }
}
