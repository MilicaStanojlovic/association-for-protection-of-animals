using AssociationForProtectionOfAnimals.Storage.Serialization;
using System;

namespace AssociationForProtectionOfAnimals.Domain.Model
{
    public class Animal : ISerializable
    {
        private int id;
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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }
        public double Height
        {
            get { return height; }
            set { height = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string FoundAddress
        {
            get { return foundAddress; }
            set { foundAddress = value; }
        }

        public string MedicalStatus
        {
            get { return medicalStatus; }
            set { medicalStatus = value; }
        }
        public Place Place
        {
            get { return place; }
            set { place = value; }
        }
        public Breed Breed
        {
            get { return breed; }
            set { breed = value; }
        }

        public Species Species
        {
            get { return species; }
            set { species = value; }
        }

        public Animal()
        {
            name = "";
            description = "";
            foundAddress = "";
            medicalStatus = "";
            breed = new Breed();
            species = new Species();
        }

        public Animal(int id, string name, int age, double weight, double height, string description, string foundAddress, string medicalStatus, Place place, Breed breed, Species species)
        {
            this.id = id;
            this.name = name;
            this.age = age;
            this.weight = weight;
            this.height = height;
            this.description = description;
            this.foundAddress = foundAddress;
            this.medicalStatus = medicalStatus;
            this.place = place;
            this.breed = breed;
            this.species = species;
        }

        public override string ToString()
        {
            return $"{name}, {age} years old";
        }

        public virtual string[] ToCSV()
        {
            return new string[]
            {
                id.ToString(),
                name,
                age.ToString(),
                weight.ToString(),
                height.ToString(),
                description,
                foundAddress,
                medicalStatus,
                place.Name,
                place.PostalCode.ToString(),
                breed.Name,
                breed.Description,
                species.Name,
                species.Description
            };
        }

        public virtual void FromCSV(string[] values)
        {
            if (values.Length != 14)
            {
                throw new ArgumentException("Invalid number of values for CSV deserialization.");
            }

            id = int.Parse(values[0]);
            name = values[1];
            age = int.Parse(values[2]);
            weight = double.Parse(values[3]);
            height = double.Parse(values[4]);
            description = values[5];
            foundAddress = values[6];
            medicalStatus = values[7];
            place = new Place(values[8], int.Parse(values[9]));
            breed = new Breed(values[10], values[11]);
            species = new Species(values[12], values[13]);
        }
    }
}
