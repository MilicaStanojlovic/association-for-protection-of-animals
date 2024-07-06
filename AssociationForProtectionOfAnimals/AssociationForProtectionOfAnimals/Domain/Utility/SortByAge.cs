using AssociationForProtectionOfAnimals.Domain.IUtility;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Repository;


namespace AssociationForProtectionOfAnimals.Domain.Utility
{
    public class SortByAge : ISortStrategy
    {
        public IEnumerable<Post> Sort(IEnumerable<Post> posts)
        {
            AnimalRepo _animalRepository = (AnimalRepo)Injector.CreateInstance<IAnimalRepo>();
            List<Animal> animals = _animalRepository.GetAllAnimals();
            Dictionary<int, int> animalAgeDictionary = animals.ToDictionary(animal => animal.Id, animal => animal.Age);

            var sortedPosts = posts.OrderBy(post => animalAgeDictionary[post.AnimalId]);

            return sortedPosts;
        }
    }
}
