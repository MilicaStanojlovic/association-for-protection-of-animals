using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using AssociationForProtectionOfAnimals.Domain.Model;

namespace AssociationForProtectionOfAnimals.DTO
{
    public class AnimalDTO : INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }
        private string name;
        private int age;
        private double weight;
        private double height;
        private string description;
        private string foundAddress;
        private string medicalStatus;
        private Place place;
        private Breed breed;
        private Species species;

        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public int Age
        {
            get { return age; }
            set
            {
                if (value != age)
                {
                    age = value;
                    OnPropertyChanged("Age");
                }
            }
        }

        public double Weight
        {
            get { return weight; }
            set
            {
                if (value != weight)
                {
                    weight = value;
                    OnPropertyChanged("Weight");
                }
            }
        }

        public double Height
        {
            get { return height; }
            set
            {
                if (value != height)
                {
                    height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public string FoundAddress
        {
            get { return foundAddress; }
            set
            {
                if (value != foundAddress)
                {
                    foundAddress = value;
                    OnPropertyChanged("FoundAddress");
                }
            }
        }

        public string MedicalStatus
        {
            get { return medicalStatus; }
            set
            {
                if (value != medicalStatus)
                {
                    medicalStatus = value;
                    OnPropertyChanged("MedicalStatus");
                }
            }
        }

        public Place Place
        {
            get { return place; }
            set
            {
                if (value != place)
                {
                    place = value;
                    OnPropertyChanged("Place");
                }
            }
        }

        public Breed Breed
        {
            get { return breed; }
            set
            {
                if (value != breed)
                {
                    breed = value;
                    OnPropertyChanged("Breed");
                }
            }
        }

        public Species Species
        {
            get { return species; }
            set
            {
                if (value != species)
                {
                    species = value;
                    OnPropertyChanged("Species");
                }
            }
        }

        public string? Error => null;

        private Regex _NameRegex = new Regex(@"^[A-Za-z\s]+$");
        private Regex _AgeWeightHeightRegex = new Regex(@"^\d+(\.\d{1,2})?$");

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(Name)) return "Name is required";
                        if (!_NameRegex.Match(Name).Success) return "Invalid format for Name";
                        break;
                    case "Age":
                        if (Age <= 0) return "Age must be greater than zero";
                        break;
                    case "Weight":
                        if (Weight <= 0) return "Weight must be greater than zero";
                        break;
                    case "Height":
                        if (Height <= 0) return "Height must be greater than zero";
                        break;
                    case "Description":
                        if (string.IsNullOrEmpty(Description)) return "Description is required";
                        break;
                    case "FoundAddress":
                        if (string.IsNullOrEmpty(foundAddress)) return "foundAddress is required";
                        break;
                    case "MedicalStatus":
                        if (string.IsNullOrEmpty(MedicalStatus)) return "Medical Status is required";
                        break;
                    case "Place":
                        if (Place == null) return "Place is required";
                        break;
                    case "Breed":
                        if (Breed == null) return "Breed is required";
                        break;
                    case "Species":
                        if (Species == null) return "Species is required";
                        break;
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties =
        {
            "Name", "Age", "Weight", "Height", "Description",
            "FoundAddress", "MedicalStatus", "Place", "Breed", "Species"
        };

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

        public Animal ToAnimal()
        {
            return new Animal(Id, Name, Age, Weight, Height, Description, FoundAddress, MedicalStatus, Place, Breed, Species);
        }

        public AnimalDTO()
        {
            place = new Place();
            breed = new Breed();
            species = new Species();
        }

        public AnimalDTO(Animal animal)
        {
            Id = animal.Id;
            Name = animal.Name;
            Age = animal.Age;
            Weight = animal.Weight;
            Height = animal.Height;
            Description = animal.Description;
            FoundAddress = animal.FoundAddress;
            MedicalStatus = animal.MedicalStatus;
            Place = animal.Place;
            Breed = animal.Breed;
            Species = animal.Species;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}