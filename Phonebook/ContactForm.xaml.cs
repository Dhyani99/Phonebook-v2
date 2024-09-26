using Phonebook.Models;

namespace Phonebook;

public partial class ContactForm : ContentPage
{
	public ContactForm()
	{
		InitializeComponent();
	}

	public void OnAddButtonClicked(object sender, EventArgs args)
	{
		if (nameValidator.IsNotValid)
		{
			Shell.Current.DisplayAlert("Error", "First Name is required", "OK");
			return;
		}
		if (emailValidator.IsNotValid)
		{
			foreach (var error in emailValidator.Errors)
			{
				DisplayAlert("Error", error.ToString(), "OK");
			}
			return;
		}
		if (phoneValidator.IsNotValid)
		{
			foreach (var error in phoneValidator.Errors)
			{
				DisplayAlert("Error", error.ToString(), "OK");
			}
			return;
		}
		PersonContact contact = new PersonContact();
		contact.FirstName = fn.Text;
		contact.LastName = ln.Text;
		contact.Email = email.Text;
		contact.PhoneNumber = phoneNumber.Text;
		contact.Company = companyName.Text;
		App.ContactRepo.AddNewContact(contact);
	}

	public async void OnCancelButtonClicked(object sender, EventArgs args)
	{
		await Shell.Current.GoToAsync("..");
	}






}