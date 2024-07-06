using AssociationForProtectionOfAnimals.Domain.Model;

namespace AssociationForProtectionOfAnimals.Domain.IRepository
{
    public interface IAnimalRepo
    {
        int GenerateId();
        Animal AddAnimal(Animal animal);
        Animal? UpdateAnimal(Animal animal);
        Animal? RemoveAnimal(int id);
        Animal? GetAnimalById(int id);
        List<Animal> GetAllAnimals();
        List<Animal> GetAnimalByBreed(string breed);
    }
}