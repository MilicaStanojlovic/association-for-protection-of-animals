using AssociationForProtectionOfAnimals.Storage.Serialization;

namespace AssociationForProtectionOfAnimals.Domain.Model
{
    public class Breed : ISerializable
    {
        protected string name;
        protected string description;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public Breed()
        {
            name = "";
            description = "";
        }

        public Breed(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public string[] ToCSV()
        {
            return new string[]
            {
                name,
                description
            };
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 2)
            {
                throw new ArgumentException("Invalid number of values for CSV deserialization.");
            }

            name = values[0];
            description = values[1];
        }
    }
}
