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
            if (contact is null)
            {
                throw new Exception("Valid contact details required");
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
           
            return contacts.Where(x => !string.IsNullOrEmpty(x.FirstName)).OrderBy(x => x.FirstName[0]).ToList();
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


}
