using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.IUtility;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Repository;

namespace AssociationForProtectionOfAnimals.Domain.Utility
{
    public class SortByBreed : ISortStrategy
    {
        public IEnumerable<Post> Sort(IEnumerable<Post> posts)
        {
            AnimalRepo _animalRepository = (AnimalRepo)Injector.CreateInstance<IAnimalRepo>();
            List<Animal> animals = _animalRepository.GetAllAnimals();
            Dictionary<int, string> animalBreedDictionary = animals.ToDictionary(animal => animal.Id, animal => animal.Breed.Name);

            var sortedPosts = posts.OrderBy(post => animalBreedDictionary[post.AnimalId]);

            return sortedPosts;
        }

    }
}
