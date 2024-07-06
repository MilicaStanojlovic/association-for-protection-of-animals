using AssociationForProtectionOfAnimals.Controller;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AssociationForProtectionOfAnimals.View.Adoption
{
    /// <summary>
    /// Interaction logic for TemporarilyAdoptForm.xaml
    /// </summary>
    public partial class TemporarilyAdoptForm : Window
    {
        private readonly RequestController _requestController;
        private readonly Domain.Model.RegisteredUser user;
        private readonly Domain.Model.Post post;
        private bool isVolunteer;
        public TemporarilyAdoptForm(Domain.Model.RegisteredUser user, Post post, bool isVolunteer)
        {
            InitializeComponent();
            _requestController = Injector.CreateInstance<RequestController>();
            this.user = user;   
            this.post = post;   
            this.isVolunteer = isVolunteer;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedAcommodationDate = AccommodationDatePicker.SelectedDate;
            DateTime? selectedReturnDate = ReturnDatePicker.SelectedDate;

            if (selectedAcommodationDate.HasValue && selectedReturnDate.HasValue)
            {
                DateTime acommodationDate = selectedAcommodationDate.Value;
                DateTime returnDate = selectedReturnDate.Value;

                if (acommodationDate > DateTime.Now && returnDate> DateTime.Now && returnDate > acommodationDate)
                {
                    if(isVolunteer)
                    {
                        TemporaryShelterRequest request = new TemporaryShelterRequest(user.Id, -1, post.Id, RequestStatus.Accepted, DateTime.Now, acommodationDate, returnDate);
                        _requestController.SendTemporaryShelterRequest(request);
                        MessageBox.Show("Animal temporarily adopted!");
                    }
                    else
                    {
                        TemporaryShelterRequest request = new TemporaryShelterRequest(user.Id, -1, post.Id, RequestStatus.WaitingForResponse, DateTime.Now, acommodationDate, returnDate);
                        _requestController.SendTemporaryShelterRequest(request);
                        MessageBox.Show("Request sent!");
                    }
                    

                }
                else
                {
                    MessageBox.Show("The selected date is not in the future.");
                }
            }
            else
            {
                MessageBox.Show("Please select a date.");
            }
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
