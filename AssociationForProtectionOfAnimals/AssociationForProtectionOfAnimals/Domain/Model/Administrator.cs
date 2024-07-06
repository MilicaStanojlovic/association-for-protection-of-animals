using AssociationForProtectionOfAnimals.Domain.Model.Enums;

namespace AssociationForProtectionOfAnimals.Domain.Model
{
    public class Administrator : RegisteredUser
    {
        public Administrator() : base() { }

        public Administrator(int id, string firstName, string lastName, Gender gender, DateTime dateOfBirth, string phoneNumber, string homeAddress, Place place, string idNumber, Account account)
            : base(id, firstName, lastName, gender, dateOfBirth, phoneNumber, homeAddress, place, idNumber, account, false) { }

        public Administrator(string firstName, string lastName, Gender gender, DateTime dateOfBirth, string phoneNumber, string homeAddress, Place place, string idNumber, Account account)
            : base(firstName, lastName, gender, dateOfBirth, phoneNumber, homeAddress, place, idNumber, account, false) { }

        public override string[] ToCSV()
        {
            return base.ToCSV();
        }

        public override void FromCSV(string[] values)
        {
            base.FromCSV(values);
        }
    }
}
