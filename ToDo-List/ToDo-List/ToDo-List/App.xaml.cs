using ToDo_List.Views;
using Xamarin.Forms;

namespace ToDo_List
{
	public partial class App : Application
	{
		public App()
		{
			App.Current.UserAppTheme = OSAppTheme.Light;
			InitializeComponent();
			MainPage = new NavigationPage(new MainPage());
		}
	}
}
