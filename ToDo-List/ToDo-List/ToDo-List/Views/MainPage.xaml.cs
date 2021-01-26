using System;
using Xamarin.Forms;
using ToDo_List.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using SQLite;
using ToDo_List.DB;
using Rg.Plugins.Popup.Services;
using System.Linq;
using System.Threading.Tasks;

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
            deleteButton.ScaleTo(0, 0);
            deleteButton.IsVisible = false;
            editButton.ScaleTo(0, 0);
            editButton.IsVisible = false;
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
                await deleteButton.ScaleTo(0, 200);
                await Task.Delay(200);
                deleteButton.IsVisible = false;
                await editButton.ScaleTo(0, 200);
                await Task.Delay(200);
                editButton.IsVisible = false;
                addButton.IsVisible = true;
                await addButton.ScaleTo(1, 200);
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
                deleteButton.IsVisible = true;
                await deleteButton.ScaleTo(1, 200);
                await Task.Delay(200);
                await addButton.ScaleTo(0, 200);
                await Task.Delay(200);
                addButton.IsVisible = false;
                editButton.IsVisible = true;
                await editButton.ScaleTo(1, 200);
            }
        }

        private void AddClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AddNewItem(Items));
        }

        private void EditClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AddNewItem(Items, selectedItem));
        }

        private async void DeleteClicked(object sender, EventArgs e)
        {
            Items.Remove(selectedItem);
            db.Delete(selectedItem);
            title.Text = "Do zrobienia:";
            await deleteButton.ScaleTo(0, 200);
            await Task.Delay(200);
            deleteButton.IsVisible = false;
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
            if (e.ScrollY <= 1)
            {
                addButton.IsVisible = true;
                await addButton.ScaleTo(1, 200);
            }
            else
            {
                await addButton.ScaleTo(0, 200);
                await Task.Delay(200);
                addButton.IsVisible = false;
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
