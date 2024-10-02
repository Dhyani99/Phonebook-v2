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
		List<ContactGroup> groupedContacts = App.ContactRepo.GetAllContacts();
		// var favorites = contacts.Where(c => c.IsFavorite).ToList();
		// var nonFavorites = contacts.Where(c => !c.IsFavorite).ToList();
		contactList.ItemsSource = groupedContacts;
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

	private async void searchBar_TextChanged(object sender, TextChangedEventArgs e)
	{
		List<ContactGroup> filteredContacts = App.ContactRepo.SearchContacts(searchBar.Text);

		if (filteredContacts.Any())
		{
			noRecordsLabel.IsVisible = false;
			contactList.IsVisible = true;
			contactList.ItemsSource = filteredContacts;
		}
		else
		{
			noRecordsLabel.IsVisible = true;
			contactList.IsVisible = false;
		}

	}

	private void OnFavoriteSwipeItemInvoked(object sender, EventArgs e)
	{
		var swipeItem = (SwipeItem)sender;
		var contact = (PersonContact)swipeItem.BindingContext;
		if (contact != null)
		{
			contact.IsFavorite = !contact.IsFavorite;
			App.ContactRepo.UpdateContact(contact.Id, contact);
		}

		List<ContactGroup> groupedContacts = App.ContactRepo.GetAllContacts();
		contactList.ItemsSource = null;
		contactList.ItemsSource = groupedContacts;

		contactList.BeginRefresh();
		contactList.EndRefresh();

	}

	private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
	{
		var swipeItem = (SwipeItem)sender;
		var contact = (PersonContact)swipeItem.BindingContext;
		bool answer = await Shell.Current.DisplayAlert("Delete contact", "Are you sure you want to delete the contact?", "OK", "Cancel");
		if (answer)
		{
			App.ContactRepo.DeleteContact(contact.Id);
		}
		List<ContactGroup> groupedContacts = App.ContactRepo.GetAllContacts();
		contactList.ItemsSource = groupedContacts;
	}

	// public async void OnDeleteButtonClicked(object sender, EventArgs args)
	// {
	// 	bool answer = await Shell.Current.DisplayAlert("Delete contact", "Are you sure you want to delete the contact?", "OK", "Cancel");
	// 	if (answer)
	// 	{
	// 		await Shell.Current.DisplayAlert("Hi","OK", "Cancel");
	// 		// App.ContactRepo.DeleteContact(contact.Id);
	// 		// await Shell.Current.GoToAsync("..");
	// 	}
	// }
}

