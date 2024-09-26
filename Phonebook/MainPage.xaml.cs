using Phonebook.Models;
namespace Phonebook;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		List<PersonContact> contacts = App.ContactRepo.GetAllContacts();
		contactList.ItemsSource = contacts;
	}

	public async void OnCreateContactButtonClicked(object sender, EventArgs args)
	{
		await Shell.Current.GoToAsync(nameof(ContactForm));
	}

	private async void contacts_itemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		if (contactList.SelectedItem != null)
		{
			await Shell.Current.GoToAsync($"{nameof(EditContactPage)}?Id={((PersonContact)contactList.SelectedItem).Id}");
		}
	}

	private async void contacts_itemTapped(object sender, ItemTappedEventArgs e)
	{
		contactList.SelectedItem = null;
	}
	// public async void OnGetButtonClicked(object sender, EventArgs args)
	// {

	// 	List<PersonContact> contacts = await App.ContactRepo.GetAllContacts();
	// 	contactList.ItemsSource = contacts;
	// }
}

