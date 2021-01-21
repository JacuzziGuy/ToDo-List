using System;
using Xamarin.Forms;
using ToDo_List.Models;
using System.Collections.ObjectModel;
using SQLite;
using ToDo_List.DB;
using Rg.Plugins.Popup.Services;

namespace ToDo_List.Views
{
    public partial class MainPage : ContentPage
    {
        #region Variables

        SQLiteConnection db = Constants.DataBasePath();
        public ObservableCollection<ItemModel> Items = new ObservableCollection<ItemModel>();

        #endregion

        public MainPage()
        {
            InitializeComponent();
            InitDB();
            InitList();
        }

        private void InitList()
        {
            itemsList.ItemsSource = Items;
            itemsList.ItemTapped += ItemTapped;
        }

        private void InitDB()
        {
            try
            {
                Items = new ObservableCollection<ItemModel>(db.Query<ItemModel>("SELECT * FROM ItemModel"));
            }
            catch
            {
                db.CreateTable<ItemModel>();
            }
        }

        private void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ItemModel item = e.Item as ItemModel;
            PopupNavigation.Instance.PushAsync(new Popup(item.Title, item.Text));
            itemsList.SelectedItem = null;
        }

        private void AddClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AddNewItem(Items));
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            ItemModel item = (sender as ImageButton).BindingContext as ItemModel;
            Items.Remove(item);
            db.Delete(item);
        }

        private void CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ItemModel item = cb.BindingContext as ItemModel;
            item.Checked = cb.IsChecked;
            db.Update(item);
        }

        private async void ScrolledList(object sender, ScrolledEventArgs e)
        {
            if (e.ScrollY <= 20)
            {
                await addButton.TranslateTo(0, 0, 100);
            }
            else
            {
                await addButton.TranslateTo(0, 200, 200);
            }
        }
    }
}
