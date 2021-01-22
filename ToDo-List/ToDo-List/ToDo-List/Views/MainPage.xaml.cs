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
        ItemModel selectedItem = new ItemModel();

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
            if(selectedItem == item)
            {
                ToolbarItems.RemoveAt(0);
                selectedItem = null;
            }
            else if(ToolbarItems.Count == 0)
            {
                ToolbarItem btn = new ToolbarItem();
                btn.Text = "Usuń";
                btn.Clicked += DeleteClicked;
                ToolbarItems.Add(btn);
                selectedItem = item;
            }
            else
            {
                selectedItem = item;
            }
            itemsList.SelectedItem = null;
        }

        private void AddClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AddNewItem(Items));
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            Items.Remove(selectedItem);
            db.Delete(selectedItem);
            ToolbarItems.RemoveAt(0);
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
