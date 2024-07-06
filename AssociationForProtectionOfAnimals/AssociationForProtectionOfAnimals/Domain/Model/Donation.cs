using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.Storage.Serialization;

namespace AssociationForProtectionOfAnimals.Domain.Model
{
    public class Donation : ISerializable
    {
        private int id;
        private DateTime dateOfDonation;
        private int value;
        private string authorFirstName;
        private string authorLastName;
        private string pdfFilePath;
        private TypeOfDonation typeOfDonation; 

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int Value
        {
            get { return value; }
            set { value = this.value; }
        }

        public string AuthorFirstName
        {
            get { return authorFirstName; }
            set { authorFirstName = value; }
        }

        public string AuthorLastName
        {
            get { return authorLastName; }
            set { authorLastName = value; }
        }

        public DateTime DateOfDonation
        {
            get { return dateOfDonation; }
            set { dateOfDonation = value; }
        }

        public string PdfFilePath
        {
            get { return pdfFilePath; }
            set { pdfFilePath = value; }
        }

        public TypeOfDonation TypeOfDonation
        {
            get { return typeOfDonation; }  
            set { typeOfDonation = value; }
        }

        public Donation() { }

        public Donation(int id, int value, string authorFirstName, string authorLastName, DateTime dateOfDonation, string pdfFilePath, TypeOfDonation typeOfDonation)
        {
            this.id = id;
            this.value = value;
            this.authorFirstName = authorFirstName;
            this.authorLastName = authorLastName;
            this.dateOfDonation = dateOfDonation;
            this.pdfFilePath = pdfFilePath;
            this.typeOfDonation = typeOfDonation;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                DateOfDonation.ToString("yyyy-MM-dd"),
                AuthorFirstName,
                AuthorLastName,
                Value.ToString(),
                PdfFilePath,
                TypeOfDonation.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 7)  
                throw new ArgumentException("Invalid number of donation values in CSV");

            Id = int.Parse(values[0]);
            DateOfDonation = DateTime.ParseExact(values[1], "yyyy-MM-dd", null);
            AuthorFirstName = values[2];
            AuthorLastName = values[3];
            Value = int.Parse(values[4]);
            PdfFilePath = values[5];
            TypeOfDonation = Enum.Parse<TypeOfDonation>(values[6]);
        }
    }
}
