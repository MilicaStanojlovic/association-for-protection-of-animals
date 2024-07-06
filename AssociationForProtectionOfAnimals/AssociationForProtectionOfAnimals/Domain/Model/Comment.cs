using AssociationForProtectionOfAnimals.Storage.Serialization;

namespace AssociationForProtectionOfAnimals.Domain.Model
{
    public class Comment : ISerializable
    {
        private int id;
        private int postId;
        private string personEmail;
        private string content;
        private DateTime dateOfComment;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int PostId
        {
            get { return postId; }
            set { postId = value; }
        }

        public string PersonEmail
        {
            get { return personEmail; }
            set { personEmail = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public DateTime DateOfComment
        {
            get { return dateOfComment; }
            set { dateOfComment = value; }
        }

        public Comment() { }

        public Comment(int id, int postId, string personEmail, string content, DateTime dateOfComment)
        {
            this.id = id;
            this.postId = postId;
            this.personEmail = personEmail;
            this.content = content;
            this.dateOfComment = dateOfComment;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                PostId.ToString(),
                PersonEmail,
                Content,
                DateOfComment.ToString("yyyy-MM-dd")
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 5)
                throw new ArgumentException("Invalid number of comment values in CSV");

            Id = int.Parse(values[0]);
            PostId = int.Parse(values[1]);
            personEmail = values[2];
            Content = values[3];
            DateOfComment = DateTime.ParseExact(values[4], "yyyy-MM-dd", null);
        }
    }
}
