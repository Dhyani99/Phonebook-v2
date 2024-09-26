namespace Phonebook;

public partial class App : Application
{
	public static ContactRepository ContactRepo { get; private set; }
	public App(ContactRepository repo)
	{
		InitializeComponent();

		MainPage = new AppShell();
		ContactRepo = repo;
	}
}
