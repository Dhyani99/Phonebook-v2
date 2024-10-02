using SQLite;
using Phonebook.Models;
namespace Phonebook;

public class ContactRepository
{

    string _dbPath;

    private SQLiteConnection conn;

    private void Init()
    {
        if (conn != null)
        {
            return;
        }
        conn = new SQLiteConnection(_dbPath);
        //      conn.DropTable<PersonContact>();
        conn.CreateTable<PersonContact>();
    }

    public ContactRepository(string dbPath)
    {
        _dbPath = dbPath;
    }

    public void AddNewContact(PersonContact contact)
    {
        int result = 0;
        try
        {
            Init();
            if (contact != null)
            {
                List<PersonContact> contacts = GetAllContacts().SelectMany(g => g).ToList();
                var checkPhone = contacts.FirstOrDefault(c => c.PhoneNumber.Equals(contact.PhoneNumber));
                if (checkPhone != null)
                {
                    Shell.Current.DisplayAlert("Error", "Contact Already added", "OK");
                    return;
                }
                contact.IsFavorite = false;
            }

            result = conn.Insert(contact);
            Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error!", $"Unable to add contact info: {ex.Message}", "OK");
        }
    }

    public List<ContactGroup> GetAllContacts()
    {
        try
        {
            Init();
            List<PersonContact> contacts = conn.Table<PersonContact>().ToList();
            // List<PersonContact> contactsToDelete = contacts.Where(x=>string.IsNullOrEmpty(x.FirstName)).ToList();
            // foreach(PersonContact c in contactsToDelete){
            //     conn.Delete(c);
            // }

            // var contactGroups = contacts
            //                     .GroupBy(c => c.IsFavorite ? "Favorites" : "Contacts")
            //                     .Select(g => new ContactGroup(g.Key, g.ToList()))
            //                     .ToList();

            var favorites = contacts.Where(c => c.IsFavorite).OrderBy(c => c.FirstName).ToList();
            var nonFavorites = contacts.Where(c => !c.IsFavorite).OrderBy(c => c.FirstName).ToList();

            List<ContactGroup> contactGroups = new List<ContactGroup>();

            if (favorites.Any())
            {
                contactGroups.Add(new ContactGroup("Favorites", favorites));
            }
            if (nonFavorites.Any())
            {
                contactGroups.Add(new ContactGroup("All Contacts", nonFavorites));
            }

            return contactGroups;

        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error!", $"Unable to get contacts: {ex.Message}", "OK");
        }

        return new List<ContactGroup>();
    }

    public PersonContact GetContactById(int id)
    {
        Init();
        List<PersonContact> contacts = conn.Table<PersonContact>().ToList();
        return contacts.FirstOrDefault(contact => contact.Id == id);
    }

    public void UpdateContact(int id, PersonContact contact)
    {
        if (id != contact.Id)
        {
            return;
        }

        var contactToUpdate = GetContactById(id);
        if (contactToUpdate != null)
        {
            contactToUpdate.FirstName = contact.FirstName;
            contactToUpdate.LastName = contact.LastName;
            contactToUpdate.Company = contact.Company;
            contactToUpdate.Email = contact.Email;
            contactToUpdate.PhoneNumber = contact.PhoneNumber;
            contactToUpdate.IsFavorite = contact.IsFavorite;
            conn.Update(contactToUpdate);
        }

    }
    public void DeleteContact(int id)
    {
        PersonContact contact = GetContactById(id);
        conn.Delete(contact);
    }

    public List<ContactGroup> SearchContacts(string filter)
    {
        List<PersonContact> contacts = GetAllContacts().SelectMany(g => g).ToList();
        var filteredContacts = contacts.Where(c => !string.IsNullOrWhiteSpace(c.FirstName) && c.FirstName.ToLower().Contains(filter.ToLower())).ToList();

        if (filteredContacts == null || filteredContacts.Count <= 0)
        {
            filteredContacts = contacts.Where(c => !string.IsNullOrWhiteSpace(c.LastName) && c.LastName.ToLower().Contains(filter.ToLower())).ToList();
        }

        if (filteredContacts == null || filteredContacts.Count <= 0)
        {
            filteredContacts = contacts.Where(c => !string.IsNullOrWhiteSpace(c.Email) && c.Email.ToLower().Contains(filter.ToLower())).ToList();
        }

        if (filteredContacts == null || filteredContacts.Count <= 0)
        {
            filteredContacts = contacts.Where(c => c.PhoneNumber.Contains(filter)).ToList();
        }

        var favorites = filteredContacts.Where(c => c.IsFavorite).OrderBy(c => c.FirstName).ToList();
        var nonFavorites = filteredContacts.Where(c => !c.IsFavorite).OrderBy(c => c.FirstName).ToList();

        List<ContactGroup> filteredGroups = new List<ContactGroup>();
        if (favorites.Any())
        {
            filteredGroups.Add(new ContactGroup("Favorites", favorites));
        }

        if (nonFavorites.Any())
        {
            filteredGroups.Add(new ContactGroup("All Contacts", nonFavorites));
        }

        return filteredGroups;

    }

}
