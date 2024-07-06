using AssociationForProtectionOfAnimals.Controller;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.DTO;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AssociationForProtectionOfAnimals.View.Animal
{
    public partial class CreateAnimal : Window
    {
        public AnimalDTO animal { get; set; }

        private RegisteredUserController RegisteredUserController;
        private VolunteerController VolunteerController;
        private int userId;
        private Window window;

        public CreateAnimal(int userId, Window window)
        {
            InitializeComponent();
            animal = new AnimalDTO();
            RegisteredUserController = Injector.CreateInstance<RegisteredUserController>();
            VolunteerController = Injector.CreateInstance<VolunteerController>();
            this.userId = userId;
            this.window = window;

            DataContext = this;

            SetPlaceholders();
        }

        private void btnAddAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (animal.IsValid)
            {
                if (IsRegisteredUserPage())
                    RegisteredUserController.AddAnimal(animal.ToAnimal(), userId);
                else
                    VolunteerController.AddAnimal(animal.ToAnimal(), userId);
                Close();
            }
            else
            {
                MessageBox.Show("Animal can not be added. Not all fields are valid.");
            }
        }

        private bool IsRegisteredUserPage()
        {
            return window is AssociationForProtectionOfAnimals.View.RegisteredUser.RegisteredUserPage;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetPlaceholders()
        {
            animal.Name = "Rex";
            animal.Age = 2;
            animal.Weight = 4;
            animal.Height = 50;
            animal.FoundAddress = "Found Address";
            animal.Description = "Description";
            animal.MedicalStatus = "Medical Status";
            animal.Species = new Species("Bulldog", "Some description");
            animal.Breed = new Breed("Dog", "Some description");
            animal.Place = new Place("Zajecar", 19000);
            

            nameTextBox.GotFocus += NameTextBox_GotFocus;
            ageTextBox.GotFocus += AgeTextBox_GotFocus;
            weightTextBox.GotFocus += WeightTextBox_GotFocus;
            heightTextBox.GotFocus += HeightTextBox_GotFocus;
            foundAddressTextBox.GotFocus += FoundAddressTextBox_GotFocus;
            descriptionTextBox.GotFocus += DescriptionTextBox_GotFocus;
            medicalStatusTextBox.GotFocus += MedicalStatusTextBox_GotFocus;
            speciesTextBox.GotFocus += SpeciesTextBox_GotFocus;
            breedTextBox.GotFocus += BreedTextBox_GotFocus;
            placeTextBox.GotFocus += PlaceTextBox_GotFocus;
        }

        private void PlaceTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            placeTextBox.Text = string.Empty;
        }
        private void NameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            nameTextBox.Text = string.Empty;
        }

        private void SpeciesTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            speciesTextBox.Text = string.Empty;
        }

        private void BreedTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            breedTextBox.Text = string.Empty;
        }

        private void AgeTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ageTextBox.Text = string.Empty;
        }

        private void WeightTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            weightTextBox.Text = string.Empty;
        }

        private void HeightTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            heightTextBox.Text = string.Empty;
        }

        private void FoundAddressTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            foundAddressTextBox.Text = string.Empty;
        }

        private void DescriptionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            descriptionTextBox.Text = string.Empty;
        }

        private void MedicalStatusTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            medicalStatusTextBox.Text = string.Empty;
        }
    }
}