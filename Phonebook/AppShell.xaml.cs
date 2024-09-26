
namespace Phonebook;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(ContactForm), typeof(ContactForm));
		Routing.RegisterRoute(nameof(EditContactPage), typeof(EditContactPage));
	}
}
