using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.Domain.Model;

namespace AssociationForProtectionOfAnimals.Domain.Model
{
    public class Volunteer : RegisteredUser
    {
        public Volunteer() : base() { }

        public Volunteer(int id, string firstName, string lastName, Gender gender, DateTime dateOfBirth, string phoneNumber, string homeAddress, Place place, string idNumber, Account account)
            : base(id, firstName, lastName, gender, dateOfBirth, phoneNumber, homeAddress, place, idNumber, account, false) { }

        public Volunteer(string firstName, string lastName, Gender gender, DateTime dateOfBirth, string phoneNumber, string homeAddress, Place place, string idNumber, Account account)
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
