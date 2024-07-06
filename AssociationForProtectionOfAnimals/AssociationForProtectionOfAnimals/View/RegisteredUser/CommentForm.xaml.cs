using AssociationForProtectionOfAnimals.Controller;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.DTO;
using System.ComponentModel;
using System.Windows;

namespace AssociationForProtectionOfAnimals.View.RegisteredUser
{
    public partial class CommentForm : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private CommentDTO? _comment;
        public CommentDTO? Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        private RegisteredUserDTO? _user;
        public RegisteredUserDTO? User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private Post post;
        private Domain.Model.RegisteredUser? user;
        private CommentController _commentController;
        private PostController _postController;

        public CommentForm(Post post, Domain.Model.RegisteredUser? user)
        {
            InitializeComponent();
            DataContext = this;

            Comment = new CommentDTO { Content = string.Empty };
            User = new RegisteredUserDTO(user);

            _commentController = Injector.CreateInstance<CommentController>();
            _postController = Injector.CreateInstance<PostController>();

            this.user = user;
            this.post = post;

            OnPropertyChanged(nameof(User));
            OnPropertyChanged(nameof(Comment));
            OnPropertyChanged(nameof(DateOfComment));
        }

        public string DateOfComment => DateTime.Now.Date.ToString("yyyy-MM-dd");

        public void CommentPost_Click(object sender, RoutedEventArgs e)
        {
            Comment comment = new Comment
            {
                PostId = post.Id,
                PersonEmail = user.Account.Username,
                Content = Comment.Content,
                DateOfComment = DateTime.ParseExact(DateOfComment, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
            };
            _commentController.Add(comment);
            this.Close();
        }
    }
}
