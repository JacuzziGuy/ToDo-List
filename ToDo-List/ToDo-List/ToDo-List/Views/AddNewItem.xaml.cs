using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using ToDo_List.Models;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SQLite;
using ToDo_List.DB;

namespace ToDo_List.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewItem : PopupPage
    {
        private ObservableCollection<ItemModel> Items;
        SQLiteConnection db = Constants.DataBasePath();
        int importance = 1;
        public AddNewItem(ObservableCollection<ItemModel> items)
        {
            Items = items;
            InitializeComponent();
            InitXaml();
        }

        private void InitXaml()
        {
            input.Text = "";
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void AddClicked(object sender, EventArgs e)
        {
            var item = new ItemModel { Text = input.Text, Checked = false, Importance = importance };
            Items.Add(item);
            db.Insert(item);
            PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }

        private void ImportanceClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            btn1.BorderWidth = 0;
            btn2.BorderWidth = 0;
            btn3.BorderWidth = 0;
            btn4.BorderWidth = 0;
            btn5.BorderWidth = 0;
            btn6.BorderWidth = 0;
            button.BorderWidth = 2;
            importance = int.Parse(button.Text);
        }
    }
}