using System;
using Xamarin.Forms;
using ToDo_List.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using SQLite;
using ToDo_List.DB;
using Rg.Plugins.Popup.Services;
using System.Linq;

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
            Items = SortItems(Items);
            itemsList.ItemsSource = Items;
            itemsList.ItemTapped += ItemTapped;
            deleteButton.TranslateTo(0, 200, 0);
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

        private async void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ItemModel item = e.Item as ItemModel;
            itemsList.SelectedItem = null;
            if (selectedItem == item)
            {
                title.Text = "Do zrobienia:";
                selectedItem = null;
                await deleteButton.TranslateTo(0, 200, 400);
            }
            else
            {
                selectedItem = item;
                if (selectedItem.Text.Length >= 20)
                {
                    title.Text = "";
                    for (int i = 0; i < 20; i++)
                    {
                        title.Text += selectedItem.Text[i];
                    }
                    title.Text += "...";
                }
                else
                    title.Text = selectedItem.Text;
                await deleteButton.TranslateTo(0, 0, 200);
            }
        }

        private void AddClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AddNewItem(Items));
        }

        private async void DeleteClicked(object sender, EventArgs e)
        {
            Items.Remove(selectedItem);
            db.Delete(selectedItem);
            title.Text = "Do zrobienia:";
            await deleteButton.TranslateTo(0, 200, 400);
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
        public static ObservableCollection<ItemModel> SortItems(ObservableCollection<ItemModel> orderList)
        {
            ObservableCollection<ItemModel> temp = new ObservableCollection<ItemModel>(orderList.OrderBy(x => x.Importance));
            orderList.Clear();
            foreach (ItemModel e in temp)
                orderList.Add(e);
            return orderList;
        }
    }
}
