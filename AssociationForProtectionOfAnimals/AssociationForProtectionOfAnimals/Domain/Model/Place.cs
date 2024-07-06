using AssociationForProtectionOfAnimals.Storage.Serialization;

namespace AssociationForProtectionOfAnimals.Domain.Model
{
    public class Place : ISerializable
    {
        protected int id;
        protected string name;
        protected int postalCode;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        public Place()
        {
            name = "";
            postalCode = 0;
        }
        public Place(int id, string name, int postalCode)
        {
            this.id = id;
            this.name = name;
            this.postalCode = postalCode;
        }

        public Place(string name, int postalCode)
        {
            this.name = name;
            this.postalCode = postalCode;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                id.ToString(),
                name,
                postalCode.ToString()
            };
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 3)
            {
                throw new ArgumentException("Invalid number of values for CSV deserialization.");
            }
            id = int.Parse(values[0]);
            name = values[1];
            postalCode = int.Parse(values[2]);
        }
    }
}

