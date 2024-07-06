using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.Storage.Serialization;

namespace AssociationForProtectionOfAnimals.Domain.Model
{
    public class Account : ISerializable
    {
        protected int id;
        protected string username;
        protected string password;
        protected AccountType type;
        protected AccountStatus status;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public AccountType Type
        {
            get { return type; }
            set { type = value; }
        }

        public AccountStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        public Account() { }

        public Account(int id, string username, string password, AccountType type, AccountStatus status)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.type = type;
            this.status = status;
        }
        public Account(string username, string password, AccountType type, AccountStatus status)
        {
            this.username = username;
            this.password = password;
            this.type = type;
            this.status = status;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                id.ToString(),
                username,
                password,
                type.ToString(),
                status.ToString()
            };
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 5)
            {
                throw new ArgumentException("Invalid number of values for CSV deserialization.");
            }

            id = int.Parse(values[0]);
            username = values[1];
            password = values[2];
            type = (AccountType)Enum.Parse(typeof(AccountType), values[3]);
            status = (AccountStatus)Enum.Parse(typeof(AccountStatus), values[4]);
        }
    }
}
