using ToDo_List.Views;
using Xamarin.Forms;

namespace ToDo_List
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
			MainPage = new NavigationPage(new MainPage());
		}
		protected override void OnStart()
		{
		}
		protected override void OnSleep()
		{
		}
		protected override void OnResume()
		{
		}
	}
}
