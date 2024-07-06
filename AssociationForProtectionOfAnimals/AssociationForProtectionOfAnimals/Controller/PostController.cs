using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.Observer;
using AssociationForProtectionOfAnimals.View.UnregisteredUser;
using System.Windows.Input;

namespace AssociationForProtectionOfAnimals.Controller
{
    public class PostController
    {

        private readonly IPostRepository _posts;
        private readonly IAnimalRepo _animalRepository;

        public PostController()
        {
            _posts = Injector.CreateInstance<IPostRepository>();
            _animalRepository = Injector.CreateInstance<IAnimalRepo>();
        }

        public List<Post> GetAllPosts()
        {
            return _posts.GetAllPosts();
        }

        public List<Post> GetAllPublishedPosts()
        {
            List<Post> posts = new List<Post>();
            foreach (Post post in GetAllPosts())
                if (post.PostStatus!=PostStatus.Unpublished)
                    posts.Add(post);
            return posts;
        }

        public List<Post> GetAllUnpublishedPosts()
        {
            List<Post> posts = new List<Post>();
            foreach (Post post in GetAllPosts())
                if (post.PostStatus == PostStatus.Unpublished)
                    posts.Add(post);
            return posts;
        }

        public Post? GetById(int postId)
        {
            return _posts.GetById(postId);
        }

        public Post Add(Post post)
        {
            return _posts.Add(post);
        }

        public Post Update(Post post)
        {
            return _posts.Update(post);
        }

        public List<Post> FindPostsByCriteria(PostStatus? selectedPostStatus, string selectedBreed, DateTime? selectedStartDate, int? selectedMinYears, int? selectedMaxYears)
        {
            List<Post> posts = GetAllPosts();
            List<int> animalIds = new List<int>();

            if (!string.IsNullOrEmpty(selectedBreed))
            {
                List<Animal> animals = _animalRepository.GetAnimalByBreed(selectedBreed);
                animalIds = animals.Select(animal => animal.Id).ToList();
            }

            var filteredPosts = posts.Where(post =>
                (!selectedPostStatus.HasValue || post.PostStatus == selectedPostStatus) &&
                (!selectedStartDate.HasValue || post.DateOfPosting.Date >= selectedStartDate) &&
                (string.IsNullOrEmpty(selectedBreed) || animalIds.Contains(post.AnimalId)) &&
                (!selectedMinYears.HasValue || _animalRepository.GetAnimalById(post.AnimalId).Age >= selectedMinYears) &&
                (!selectedMaxYears.HasValue || _animalRepository.GetAnimalById(post.AnimalId).Age <= selectedMaxYears)
            ).ToList();

            return filteredPosts;
        }

        public void Subscribe(IObserver observer)
        {
            _posts.Subscribe(observer);
        }

        public List<Post> GetAllPosts(int page, int pageSize, string sortCriteria, List<Post> posts)
        {
            return _posts.GetAllPosts(page, pageSize, sortCriteria, posts);
        }

        public void LikePost(RegisteredUser user, Post post)
        {
            Post newPost = GetById(post.Id);
            newPost.PersonLikedIds.Add(user.Account.Username);
            Update(newPost);
        }
    }
}
