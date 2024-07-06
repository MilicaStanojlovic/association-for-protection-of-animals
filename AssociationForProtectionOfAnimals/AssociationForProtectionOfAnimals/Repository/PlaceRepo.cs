using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Storage;

namespace AssociationForProtectionOfAnimals.Repository
{
    internal class PlaceRepo : Subject, IPlaceRepo
    {
        private readonly List<Place> _places;
        private readonly Storage<Place> _storage;

        public PlaceRepo()
        {
            _storage = new Storage<Place>("places.csv");
            _places = _storage.Load();
        }

        private int GenerateId()
        {
            if (_places.Count == 0) return 0;
            return _places.Last().Id + 1;
        }

        public Place GetPlaceById(int id)
        {
            return _places.Find(v => v.Id == id);
        }
        public Place GetPlaceByNameAndPostalCode(Place place)
        {
            return _places.FirstOrDefault(p => p.Name == place.Name && p.PostalCode == place.PostalCode);
        }

        public Place AddPlace(Place place)
        {
            place.Id = GenerateId();
            _places.Add(place);
            _storage.Save(_places);
            NotifyObservers();
            return place;
        }

        public Place? GetPlaceByName(string placeName)
        {
            return _places.FirstOrDefault(p => p.Name.Equals(placeName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
