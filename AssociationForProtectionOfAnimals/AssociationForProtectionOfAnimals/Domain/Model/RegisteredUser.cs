using AssociationForProtectionOfAnimals.Domain.Model.Enums;

namespace AssociationForProtectionOfAnimals.Domain.Model
{
    public class RegisteredUser : Person
    {
        protected bool isBlackListed;

        public bool IsBlackListed
        {
            get { return isBlackListed; }
            set { isBlackListed = value; }
        }

        public RegisteredUser() : base()
        {
            isBlackListed = false;
        }

        public RegisteredUser(int id, string firstName, string lastName, Gender gender, DateTime dateOfBirth, string phoneNumber, string homeAddress, Place place, string idNumber, Account account, bool isBlackListed)
            : base(id, firstName, lastName, gender, dateOfBirth, phoneNumber, homeAddress, place, idNumber, account)
        {
            this.isBlackListed = isBlackListed;
        }

        public RegisteredUser(string firstName, string lastName, Gender gender, DateTime dateOfBirth, string phoneNumber, string homeAddress, Place place, string idNumber, Account account, bool isBlackListed)
            : base(firstName, lastName, gender, dateOfBirth, phoneNumber, homeAddress, place, idNumber, account)
        {
            this.isBlackListed = isBlackListed;
        }

        public override string[] ToCSV()
        {
            var baseData = new List<string>(base.ToCSV())
            {
                isBlackListed.ToString()
            };
            return baseData.ToArray();
        }

        public override void FromCSV(string[] values)
        {
            if (values.Length != 11)
            {
                throw new ArgumentException("Invalid number of values for CSV deserialization.");
            }

            base.FromCSV(values.Take(10).ToArray());

            this.isBlackListed = bool.Parse(values[10]);
        }
    }
}
