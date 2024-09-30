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

	private async void searchBar_TextChanged(object sender, TextChangedEventArgs e){
		List<ContactGroup> groupedContacts = App.ContactRepo.SearchContacts(searchBar.Text);
		contactList.ItemsSource = groupedContacts;
	}

	private async void OnFavoriteSwipeItemInvoked(object sender, EventArgs e){
		var swipeItem = sender as SwipeItem;
    	var contact = swipeItem?.BindingContext as PersonContact;
		if(contact!=null){
			contact.IsFavorite = !contact.IsFavorite;
			App.ContactRepo.UpdateContact(contact.Id, contact);
		}
		List<ContactGroup> groupedContacts = App.ContactRepo.GetAllContacts();
		contactList.ItemsSource = groupedContacts;
	}
	
}

