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
        ViewCell lastCell;

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
            deleteButton.TranslateTo(-85, 0, 00);
            editButton.TranslateTo(-85, 0, 0);
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
            itemsList.SelectedItem = null;
            if (selectedItem == item)
            {
                title.Text = "Do zrobienia:";
                DisableTapButtons();
                selectedItem = null;
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
                else if (selectedItem.Text != "")
                {
                    title.Text = selectedItem.Text;
                }
                else
                {
                    title.Text = "Puste zadanie";
                }
                EnableTapButtons();
            }
        }

        private async void DisableTapButtons()
        {
            await deleteButton.TranslateTo(-85, 0, 200);
            await editButton.TranslateTo(-85, 0, 200);
        }

        private async void EnableTapButtons()
        {
            await deleteButton.TranslateTo(0, 0, 200);
            await editButton.TranslateTo(0, 0, 200);
        }

        private void AddClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AddNewItem(Items));
            title.Text = "Do zrobienia";
            DisableTapButtons();
            lastCell = null;
            selectedItem = null;
        }

        private void EditClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AddNewItem(Items, selectedItem));
            title.Text = "Do zrobienia:";
            DisableTapButtons();
            selectedItem = null;
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            Items.Remove(selectedItem);
            db.Delete(selectedItem);
            title.Text = "Do zrobienia:";
            DisableTapButtons();
            selectedItem = null;
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
                await addButton.TranslateTo(0, 0, 200);
            }
            else
            {
                await addButton.TranslateTo(0, 85, 200);
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

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var viewCell = sender as ViewCell;
            if (lastCell == viewCell)
            {
                viewCell.View.BackgroundColor = Color.Transparent;
                lastCell = null;
            }
            else if (lastCell != null)
            {
                lastCell.View.BackgroundColor = Color.Transparent;
                viewCell.View.BackgroundColor = Color.FromHex("#fffcde");
                lastCell = viewCell;
            }
            else
            {
                viewCell.View.BackgroundColor = Color.FromHex("#fffcde");
                lastCell = viewCell;
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
