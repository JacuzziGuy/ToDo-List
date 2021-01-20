using System;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace ToDo_List.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Popup : PopupPage
    {
        public Popup(string Title, string Text)
        {
            InitializeComponent();
            title.Text = Title;
            text.Text = Text;
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}