using AssociationForProtectionOfAnimals.Controller;
using AssociationForProtectionOfAnimals.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AssociationForProtectionOfAnimals.Domain.Model;
using AssociationForProtectionOfAnimals.Domain.Model.Enums;
using System.Windows.Controls.Primitives;

namespace AssociationForProtectionOfAnimals.View.Adoption
{
    /// <summary>
    /// Interaction logic for AdoptForm.xaml
    /// </summary>
    public partial class AdoptForm : Window
    {
        private readonly RequestController _requestController;
        private readonly Domain.Model.RegisteredUser user;
        private readonly Domain.Model.Post post;
        private bool isVolunteer;
        public AdoptForm(Domain.Model.RegisteredUser user, Post post, bool isVolunteer)
        {
            InitializeComponent();
            this.user = user;
            this.post = post;
            this.isVolunteer = isVolunteer; 
            _requestController = Injector.CreateInstance<RequestController>();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = AdoptionDatePicker.SelectedDate;

            if (selectedDate.HasValue)
            {
                DateTime adoptionDate = selectedDate.Value;

                if (adoptionDate > DateTime.Now)
                {
                    if (isVolunteer)
                    {
                        AdoptionRequest request = new AdoptionRequest(user.Id, -1, post.Id, RequestStatus.Accepted, DateTime.Now, adoptionDate);
                        _requestController.SendAdoptionRequest(request);
                        MessageBox.Show("Animal adopted!");
                    }
                    else
                    {
                        AdoptionRequest request = new AdoptionRequest(user.Id, -1, post.Id, RequestStatus.WaitingForResponse, DateTime.Now, adoptionDate);
                        _requestController.SendAdoptionRequest(request);
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
