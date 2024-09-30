using System.ComponentModel.DataAnnotations;
using SQLite;
using MaxLengthAttribute = SQLite.MaxLengthAttribute;

namespace Phonebook.Models;

[Table("contacts")]
public class PersonContact
{

    [PrimaryKey, AutoIncrement]
    public int Id {get; set;}

    [MaxLength(100)]
    public string FirstName {get; set;}

    [MaxLength(100)]
    public string LastName {get; set;}

    public string Company {get; set;}

    [Required(ErrorMessage ="Email should not be empty")]
    public string Email {get; set;}

    [MaxLength(10)]
    public string PhoneNumber{get; set;}

    public bool IsFavorite{get; set;}
}
