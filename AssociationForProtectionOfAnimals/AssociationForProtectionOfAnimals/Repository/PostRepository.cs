using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.Storage;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.IRepository;
using System.Windows.Input;

namespace AssociationForProtectionOfAnimals.Repository
{
    public class PostRepository : Subject, IPostRepository
    {
        private readonly List<Post> _posts;
        private readonly Storage<Post> _storage;

        public PostRepository()
        {
            _storage = new Storage<Post>("posts.csv");
            _posts = _storage.Load();
        }

        private int GenerateId()
        {
            if (_posts.Count == 0) return 0;
            return _posts.Last().Id + 1;
        }

        public Post Add(Post post)
        {
            post.Id = GenerateId();
            _posts.Add(post);
            _storage.Save(_posts);
            NotifyObservers();
            return post;
        }

        public Post? Update(Post post)
        {
            Post? oldPost = GetById(post.Id);
            if (oldPost == null) return null;

            oldPost.Id = post.Id;
            oldPost.DateOfPosting = post.DateOfPosting;
            oldPost.DateOfUpdating = post.DateOfUpdating;
            oldPost.PostStatus = post.PostStatus;
            oldPost.HasCurrentAdopter = post.HasCurrentAdopter;
            oldPost.AnimalId = post.AnimalId;
            oldPost.Author = post.Author;
            oldPost.Adopter = post.Adopter;

            _storage.Save(_posts);
            NotifyObservers();
            return oldPost;
        }
        public Post? Remove(int id)
        {
            Post? post = GetById(id);
            if (post == null) return null;

            _posts.Remove(post);
            _storage.Save(_posts);
            NotifyObservers();
            return post;
        }

        public Post? GetById(int id)
        {
            return _posts.Find(v => v.Id == id);
        }

        public List<Post>? GetPostByPersonPosted(string personPostedEmail)
        {
            return _posts.Where(post => post.Author == personPostedEmail).ToList();
        }

        public List<Post> GetAllPosts()
        {
            return _posts;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public List<Post> GetAllPosts(int page, int pageSize, string sortCriteria, List<Post> postsToPaginate)
        {
            IEnumerable<Post> posts = postsToPaginate;

            AnimalRepo _animalRepository = (AnimalRepo)Injector.CreateInstance<IAnimalRepo>();

            List<Animal> animals = _animalRepository.GetAllAnimals();
            Dictionary<int, string> animalBreedDictionary = animals.ToDictionary(animal => animal.Id, animal => animal.Breed.Name);
            Dictionary<int, int> animalAgeDictionary = animals.ToDictionary(animal => animal.Id, animal => animal.Age);

            switch (sortCriteria)
            {
                case "DateOfPosting":
                    posts = posts.OrderBy(x => x.DateOfPosting);
                    break;
                case "PostStatus":
                    posts = posts.OrderBy(x => x.PostStatus);
                    break;
                case "AnimalBreed":
                    posts = posts.OrderBy(post => animalBreedDictionary.ContainsKey(post.AnimalId) ? animalBreedDictionary[post.AnimalId] : string.Empty);
                    break;
                case "AnimalYears":
                    posts = posts.OrderBy(post => animalAgeDictionary.ContainsKey(post.AnimalId) ? animalAgeDictionary[post.AnimalId] : int.MaxValue);
                    break;
            }

            posts = posts.Skip((page - 1) * pageSize).Take(pageSize);

            return posts.ToList();
        }

    }
}