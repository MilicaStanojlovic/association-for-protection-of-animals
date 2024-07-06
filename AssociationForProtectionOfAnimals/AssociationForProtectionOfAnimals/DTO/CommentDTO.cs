using AssociationForProtectionOfAnimals.Controller;
using AssociationForProtectionOfAnimals.Domain.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AssociationForProtectionOfAnimals.DTO
{
    public class CommentDTO : INotifyPropertyChanged, IDataErrorInfo
    {
        private int id;
        private int postId;
        private string personEmail;
        private string content;
        private DateTime dateOfComment;

        private string personName;

        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        public int PostId
        {
            get { return postId; }
            set { SetProperty(ref postId, value); }
        }

        public string PersonEmail
        {
            get { return personEmail; }
            set { SetProperty(ref personEmail, value); }
        }

        public string Content
        {
            get { return content; }
            set { SetProperty(ref content, value); }
        }

        public DateTime DateOfComment
        {
            get { return dateOfComment; }
            set { SetProperty(ref dateOfComment, value); }
        }

        public string PersonName
        {
            get { return personName; }
            set { SetProperty(ref personName, value); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    // Example validation
                    // case "AnimalName":
                    //     if (string.IsNullOrWhiteSpace(animalName))
                    //         return "Animal name cannot be empty";
                    //     break;
                    default:
                        return null;
                }
            }
        }

        public Comment ToComment()
        {
            return new Comment
            {
                Id = id,
                PostId = postId,
                PersonEmail = personEmail,
                Content = content,
                DateOfComment = dateOfComment
            };
        }

        public CommentDTO() { }

        public CommentDTO(Comment comment)
        {
            id = comment.Id;
            postId = comment.PostId;
            personEmail = comment.PersonEmail;
            content = comment.Content;
            dateOfComment = comment.DateOfComment;

            RegisteredUserController registeredUserController = Injector.CreateInstance<RegisteredUserController>();
            AdministratorController administratorController = Injector.CreateInstance<AdministratorController>();
            Person person = registeredUserController.GetRegisteredUserByEmail(personEmail);
            if (person == null) 
                person = administratorController.GetVolunteerByUsername(personEmail);

            personName = person.FirstName + " " + person.LastName;

        }
    }
}
