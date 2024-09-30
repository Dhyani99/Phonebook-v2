using Phonebook.Models;

namespace Phonebook;

public class ContactGroup : List<PersonContact>
{
    public string Title { get; private set; }

    public ContactGroup(string title, List<PersonContact> contacts) : base(contacts)
    {
        Title = title;
    }
}
