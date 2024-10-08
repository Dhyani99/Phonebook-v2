using System.Diagnostics;
using Phonebook.Models;

namespace Phonebook;

[QueryProperty(nameof(Id), "Id")]
public partial class EditContactPage : ContentPage
{
	private PersonContact contact;
	public EditContactPage()
	{
		InitializeComponent();
	}

	public string Id
	{
		set
		{

			contact = App.ContactRepo.GetContactById(int.Parse(value));
			if (contact != null)
			{
				fn.Text = contact.FirstName;
				ln.Text = contact.LastName;
				email.Text = contact.Email;
				companyName.Text = contact.Company;
				phoneNumber.Text = contact.PhoneNumber;

			}


		}
	}
	
	public void OnUpdateButtonClicked(object sender, EventArgs args)
	{
		if(nameValidator.IsNotValid){
			Shell.Current.DisplayAlert("Error","First Name is required","OK");
			return;
		}
		if(emailValidator.IsNotValid){
			foreach(var error in emailValidator.Errors){
				DisplayAlert("Error", error.ToString(), "OK");
			}
			return;
		}
		if(phoneValidator.IsNotValid){
			foreach(var error in phoneValidator.Errors){
				DisplayAlert("Error", error.ToString(), "OK");
			}
			return;
		}
		contact.FirstName=fn.Text;
		contact.LastName=ln.Text;
		contact.Email=email.Text ;
		contact.Company=companyName.Text;
		contact.PhoneNumber=phoneNumber.Text;
		App.ContactRepo.UpdateContact(contact.Id, contact);
		Shell.Current.GoToAsync("..");
	}


}