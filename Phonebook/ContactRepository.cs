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
                List<PersonContact> contacts = GetAllContacts();
                var checkPhone = contacts.FirstOrDefault(c => c.PhoneNumber.Equals(contact.PhoneNumber));
                if (checkPhone != null)
                {
                    Shell.Current.DisplayAlert("Error", "Contact Already added", "OK");
                }
            }
            result = conn.Insert(contact);
            Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error!", $"Unable to add contact info: {ex.Message}", "OK");
        }
    }

    public List<PersonContact> GetAllContacts()
    {
        try
        {
            Init();
            List<PersonContact> contacts = conn.Table<PersonContact>().ToList();
            // List<PersonContact> contactsToDelete = contacts.Where(x=>string.IsNullOrEmpty(x.FirstName)).ToList();
            // foreach(PersonContact c in contactsToDelete){
            //     conn.Delete(c);
            // }
            return contacts.OrderBy(x => x.FirstName[0]).ToList();
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error!", $"Unable to get contacts: {ex.Message}", "OK");
        }

        return new List<PersonContact>();
    }

    public PersonContact GetContactById(int id)
    {
        List<PersonContact> contacts = GetAllContacts();
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
        }
        conn.Update(contactToUpdate);

    }
    public void DeleteContact(int id)
    {
        PersonContact contact = GetContactById(id);
        conn.Delete(contact);
    }

    public List<PersonContact> SearchContacts(string filter)
    {
        List<PersonContact> contacts = GetAllContacts();
        var filteredContacts = contacts.Where(c => !string.IsNullOrWhiteSpace(c.FirstName) && c.FirstName.ToLower().Contains(filter.ToLower())).ToList();

        if (filteredContacts == null || filteredContacts.Count <= 0)
        {
            filteredContacts = contacts.Where(c => !string.IsNullOrWhiteSpace(c.LastName) && c.LastName.ToLower().Contains(filter.ToLower())).ToList();
        }
        else return filteredContacts;

        if (filteredContacts == null || filteredContacts.Count <= 0)
        {
            filteredContacts = contacts.Where(c => !string.IsNullOrWhiteSpace(c.Email) && c.Email.ToLower().Contains(filter.ToLower())).ToList();
        }
        else return filteredContacts;


        if (filteredContacts == null || filteredContacts.Count <= 0)
        {
            filteredContacts = contacts.Where(c => c.PhoneNumber.Equals(filter)).ToList();
        }
        else return filteredContacts;

        return filteredContacts;


    }

}
