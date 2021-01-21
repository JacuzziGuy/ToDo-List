using System;
using Xamarin.Forms.Xaml;
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
        public AddNewItem(ObservableCollection<ItemModel> items)
        {
            Items = items;
            InitializeComponent();
            InitXaml();
        }
        private void InitXaml()
        {
            input.Completed += Input_Completed;
            input.Text = "";
        }

        private void Input_Completed(object sender, EventArgs e)
        {
            if (input.Text != null && input.Text != "")
            {
                var item = new ItemModel { Text = input.Text, Checked = false };
                Items.Add(item);
                db.Insert(item);
                input.Text = "";
                input.Focus();
<<<<<<< HEAD
            }        
=======
            }
>>>>>>> 9f09e2c7aee0da67627c998f42094d69558435a6
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
        private void AddClicked(object sender, EventArgs e)
        {
            if (input.Text != null && input.Text != "")
            {
                var item = new ItemModel { Text = input.Text, Checked = false };
                Items.Add(item);
                db.Insert(item);
                PopupNavigation.Instance.PopAsync();
            }
            else
            {
                DisplayAlert("UWAGA!", "Pole nie może być puste", "OK");
            }
        }
        protected override bool OnBackgroundClicked()
        {
            return false;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            input.Focus();
        }
    }
}