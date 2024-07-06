using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AssociationForProtectionOfAnimals.Controller;
using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.Repository;

namespace AssociationForProtectionOfAnimals.DTO
{
    public class PostDTO : INotifyPropertyChanged, IDataErrorInfo
    {
        private int id;
        private DateTime dateOfPosting;
        private DateTime dateOfUpdating;
        private PostStatus postStatus;
        private bool hasCurrentAdopter;
        private int animalId;
        private string author;
        private string adopter;
        private List<string> personLikedIds;

        private string adopterName;
        private string personName;
        private string animalBreed;
        private string animalName;
        private int animalYears;

        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        public DateTime DateOfPosting
        {
            get { return dateOfPosting; }
            set { SetProperty(ref dateOfPosting, value); }
        }

        public DateTime DateOfUpdating
        {
            get { return dateOfUpdating; }
            set { SetProperty(ref dateOfUpdating, value); }
        }

        public PostStatus PostStatus
        {
            get { return postStatus; }
            set { SetProperty(ref postStatus, value); }
        }

        public bool HasCurrentAdopter
        {
            get { return hasCurrentAdopter; }
            set { SetProperty(ref hasCurrentAdopter, value); }
        }

        public int AnimalId
        {
            get { return animalId; }
            set { SetProperty(ref animalId, value); }
        }

        public string Author
        {
            get { return author; }
            set { SetProperty(ref author, value); }
        }

        public string Adopter
        {
            get { return adopter; }
            set { SetProperty(ref adopter, value); }
        }

        public List<string> PersonLikedIds
        {
            get { return personLikedIds; }
            set { SetProperty(ref personLikedIds, value); }
        }

        public string AdopterName
        {
            get { return adopterName; }
            set { SetProperty(ref adopterName, value); }
        }

        public string PersonName
        {
            get { return personName; }
            set { SetProperty(ref personName, value); }
        }

        public string AnimalBreed
        {
            get { return animalBreed; }
            set { SetProperty(ref animalBreed, value); }
        }

        public string AnimalName
        {
            get { return animalName; }
            set { SetProperty(ref animalName, value); }
        }

        public int AnimalYears
        {
            get { return animalYears; }
            set { SetProperty(ref animalYears, value); }
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
                // Add any property-specific validation logic here
                switch (columnName)
                {
                    // Example validation
                    // case "AnimalName":
                    //     if (string.IsNullOrWhiteSpace(animalName))
                    //         return "Animal name cannot be empty";
                    //     break;
                    default:
                        return null;
                }
            }
        }

        public Post ToPost()
        {
            return new Post
            {
                Id = id,
                DateOfPosting = dateOfPosting,
                DateOfUpdating = dateOfUpdating,
                PostStatus = postStatus,
                HasCurrentAdopter = hasCurrentAdopter,
                AnimalId = animalId,
                Author = author,
                Adopter = adopter,
                PersonLikedIds = personLikedIds
            };
        }

        public PostDTO() { }

        public PostDTO(Post post)
        {
            id = post.Id;
            dateOfPosting = post.DateOfPosting;
            dateOfUpdating = post.DateOfUpdating;
            postStatus = post.PostStatus;
            hasCurrentAdopter = post.HasCurrentAdopter;
            animalId = post.AnimalId;
            author = post.Author;
            adopter = post.Adopter;
            personLikedIds = post.PersonLikedIds;

            Person person;
            if (!string.IsNullOrEmpty(adopter))
            {
                person = GetAdopter(adopter);
                adopterName = person.FirstName + " " + person.LastName;
            }
            else
                adopterName = "";

            person = GetAdopter(author);
            personName = person.FirstName + " " + person.LastName;
            Animal animal = GetAnimal(animalId);
            animalBreed = animal.Breed.Name;
            animalName = animal.Name;
        }

        public Person GetAdopter(string email)
        {
            RegisteredUserController registeredUserController = Injector.CreateInstance<RegisteredUserController>();
            AdministratorController administratorController = Injector.CreateInstance<AdministratorController>();
            Person person = registeredUserController.GetRegisteredUserByEmail(email);
            if (person == null)
                person = administratorController.GetVolunteerByUsername(email);

            return person;
        }

        public Animal GetAnimal(int animalId)
        {
            AnimalRepo animalRepository = (AnimalRepo)Injector.CreateInstance<IAnimalRepo>();
            Animal animal = animalRepository.GetAnimalById(animalId);

            return animal;
        }
    }
}
