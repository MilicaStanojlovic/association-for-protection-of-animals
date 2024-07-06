using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Storage;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Domain.IRepository;

namespace AssociationForProtectionOfAnimals.Repository
{
    public class AnimalRepo : Subject, IAnimalRepo
    {
        private readonly List<Animal> _animals;
        private readonly Storage<Animal> _storage;

        public AnimalRepo()
        {
            _storage = new Storage<Animal>("animals.csv");
            _animals = _storage.Load();

        }

        public int GenerateId()
        {
            if (_animals.Count == 0) return 1;
            return _animals.Last().Id + 1;
        }

        public Animal AddAnimal(Animal animal)
        {
            animal.Id = GenerateId();
            _animals.Add(animal);
            _storage.Save(_animals);
            NotifyObservers();
            return animal;
        }

        public Animal? UpdateAnimal(Animal animal)
        {
            Animal? oldAnimal = GetAnimalById(animal.Id);
            if (oldAnimal == null) return null;

            oldAnimal.Name = animal.Name;
            oldAnimal.Age = animal.Age;
            oldAnimal.Weight = animal.Weight;
            oldAnimal.Height = animal.Height;
            oldAnimal.Description = animal.Description;
            oldAnimal.FoundAddress = animal.FoundAddress;
            oldAnimal.MedicalStatus = animal.MedicalStatus;
            oldAnimal.Breed = animal.Breed;
            oldAnimal.Species = animal.Species;

            _storage.Save(_animals);
            NotifyObservers();
            return oldAnimal;
        }


        public Animal? RemoveAnimal(int id)
        {
            Animal? animal = GetAnimalById(id);
            if (animal == null) return null;

            _animals.Remove(animal);
            _storage.Save(_animals);
            NotifyObservers();
            return animal;
        }

        public Animal? GetAnimalById(int id)
        {
            return _animals.Find(v => v.Id == id);
        }

        public List<Animal> GetAllAnimals()
        {
            return _animals;
        }

        public List<Animal> GetAnimalByBreed(string breed)
        {
            return _animals.Where(animal => animal.Breed.Name == breed).ToList();
        }
    }
}
