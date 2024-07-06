using AssociationForProtectionOfAnimals.Controller;
using AssociationForProtectionOfAnimals.Domain.Model;
using System.Windows;

namespace AssociationForProtectionOfAnimals.View.UnregisteredUser
{
    public partial class ReadCommentView: Window
    { 

        private Post post;
        private Comment comment;
        private Domain.Model.RegisteredUser? user;
        private CommentController _commentController;

        public ReadCommentView(Post post, Domain.Model.RegisteredUser? user, Comment comment)
        {
            InitializeComponent();
            DataContext = this;

            _commentController = Injector.CreateInstance<CommentController>();

            this.user = user;
            this.post = post;
            this.comment = comment;

            AddCommentInfo();
        }

        private void AddCommentInfo()
        {
            firstNameTextBlock.Text = user.FirstName;
            lastNameTextBlock.Text = user.LastName;
            emailTextBlock.Text = user.Account.Username;
            dateOfCommentTextBlock.Text = comment.DateOfComment.ToString("yyyy-MM-dd");
            contentTextBox.Text = comment.Content;
        }
    }
}
