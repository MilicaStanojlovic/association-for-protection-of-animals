using AssociationForProtectionOfAnimals.Domain.Model;

namespace AssociationForProtectionOfAnimals.Domain.IRepository
{
    internal interface IPlaceRepo
    {
        Place GetPlaceById(int id);
        Place GetPlaceByNameAndPostalCode(Place place);
        Place AddPlace(Place place);
        Place? GetPlaceByName(string placeName);
    }
}
