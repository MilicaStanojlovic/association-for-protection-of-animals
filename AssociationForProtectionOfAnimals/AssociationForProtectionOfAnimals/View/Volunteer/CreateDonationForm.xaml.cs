using AssociationForProtectionOfAnimals.Controller;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using AssociationForProtectionOfAnimals.DTO;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Windows;

namespace AssociationForProtectionOfAnimals.View.Volunteer
{
    public partial class CreateDonationForm : Window
    {
        public DonationDTO Donation { get; set; }
        private readonly DonationController _donationController;

        public CreateDonationForm()
        {
            InitializeComponent();
            Donation = new DonationDTO
            {
                DateOfDonation = DateTime.Today,
                TypeOfDonation = TypeOfDonation.Individual  
            };
            _donationController = Injector.CreateInstance<DonationController>();
            DataContext = this;

            typeOfDonationComboBox.ItemsSource = Enum.GetValues(typeof(TypeOfDonation));
            typeOfDonationComboBox.SelectedItem = Donation.TypeOfDonation; 
        }

        private void BrowsePdfFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                Donation.PdfFilePath = openFileDialog.FileName;
            }
        }

        private void SaveDonation_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(valueTextBox.Text, out int value))
            {
                Donation.Value = value;
                Donation.AuthorFirstName = firstNameTextBox.Text;
                Donation.AuthorLastName = lastNameTextBox.Text;
                Donation.DateOfDonation = dateOfDonationPicker.SelectedDate.GetValueOrDefault();
                Donation.PdfFilePath = pdfFilePathTextBox.Text;
                Donation.TypeOfDonation = (TypeOfDonation)typeOfDonationComboBox.SelectedItem;

                Donation donation = new()
                {
                    DateOfDonation = Donation.DateOfDonation,
                    AuthorFirstName = Donation.AuthorFirstName,
                    AuthorLastName = Donation.AuthorLastName,
                    Value = Donation.Value,
                    PdfFilePath = Donation.PdfFilePath,
                    TypeOfDonation = Donation.TypeOfDonation
                };

                _donationController.Add(donation);
                Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid number for Value.");
            }
        }



        private void ValueTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]+$"); // Allow only numbers
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
