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


        private List<ItemModel> selectedItems = new List<ItemModel>();

        #endregion

        public MainPage()
        {
            InitializeComponent();
            InitDB();
            InitList();
        }

        //Initializing items list
        private void InitList()
        {
            Items = SortItems(Items);
            itemsList.ItemsSource = Items;
            itemsList.ItemTapped += ItemTapped;
            deleteButton.TranslateTo(-85, 0, 00);
            editButton.TranslateTo(-85, 0, 0);
        }

        //Initializing DataBase
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

        //What happens after we tap an element
        private void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ItemModel item = e.Item as ItemModel;
            itemsList.SelectedItem = null;
            if (selectedItems.Any(x => x == item))
            {
                selectedItems.Remove(item);
                if (selectedItems.Count == 0)
                {
                    title.Text = "Do zrobienia:";
                    DisableTapButtons();
                    EnableAddButton();
                }
                else if (selectedItems.Count == 1)
                {
                    SetTitle();
                    EnableEditButton();
                }
                else
                {
                    title.Text = selectedItems.Count.ToString();
                }
            }
            else
            {
                selectedItems.Add(item);
                DisableAddButton();
                if (selectedItems.Count == 1)
                {
                    SetTitle();
                    EnableTapButtons();
                }
                else
                {
                    DisableEditButton();
                    title.Text = selectedItems.Count.ToString();
                }
            }
        }

        //Hiding add button
        private async void DisableAddButton()
        {
            await addButton.TranslateTo(0, 85, 200);
        }

        //Showing add button
        private async void EnableAddButton()
        {
            await addButton.TranslateTo(0, 0, 200);
        }

        //Hiding the edit and delete button
        private async void DisableTapButtons()
        {
            await deleteButton.TranslateTo(-85, 0, 200);
            await editButton.TranslateTo(-85, 0, 200);
        }

        //Showing the edit and delete button on screen
        private async void EnableTapButtons()
        {
            await deleteButton.TranslateTo(0, 0, 200);
            await editButton.TranslateTo(0, 0, 200);
        }

        //Showing edit button
        private async void EnableEditButton()
        {
            await editButton.TranslateTo(0, 0, 200);
        }

        //Hiding edit button
        private async void DisableEditButton()
        {
            await editButton.TranslateTo(-85, 0, 200);
        }

        //Setting the title of the page as a name of the selected item
        private void SetTitle()
        {
            if (selectedItems[0].Text.Length >= 20)
            {
                title.Text = "";
                for (int i = 0; i < 20; i++)
                {
                    title.Text += selectedItems[0].Text[i];
                }
                title.Text += "...";
            }
            else if (selectedItems[0].Text != "")
            {
                title.Text = selectedItems[0].Text;
            }
            else
            {
                title.Text = "Puste zadanie";
            }
        }

        private void AddClicked(object sender, EventArgs e)
        {
            if (selectedItems.Count == 0)
                PopupNavigation.Instance.PushAsync(new AddNewItem(Items));
        }

        private void EditClicked(object sender, EventArgs e)
        {
            if (selectedItems.Count == 1)
                PopupNavigation.Instance.PushAsync(new AddNewItem(Items, selectedItems[0]));
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in selectedItems)
                {
                    Items.Remove(item);
                    db.Delete(item);
                }
                title.Text = "Do zrobienia:";
                DisableTapButtons();
                selectedItems.Clear();
            }
            catch { }
        }

        //Saving checked
        private void CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            ItemModel item = cb.BindingContext as ItemModel;
            item.Checked = cb.IsChecked;
            db.Update(item);
        }

        //Hiding the button after scrolling the list
        private async void ScrolledList(object sender, ScrolledEventArgs e)
        {
            if (e.ScrollY <= 1 && selectedItems.Count == 0)
            {
                await addButton.TranslateTo(0, 0, 200);
            }
            else
            {
                await addButton.TranslateTo(0, 85, 200);
            }
        }

        //Sorting items by priority
        public static ObservableCollection<ItemModel> SortItems(ObservableCollection<ItemModel> orderList)
        {
            ObservableCollection<ItemModel> temp = new ObservableCollection<ItemModel>(orderList.OrderBy(x => x.Importance));
            orderList.Clear();
            foreach (ItemModel e in temp)
                orderList.Add(e);
            return orderList;
        }

        //Changing the background color of a tapped element
        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var viewCell = sender as ViewCell;
            if (viewCell.View.BackgroundColor == Color.FromHex("#fffcde"))
            {
                viewCell.View.BackgroundColor = Color.Transparent;
            }
            else
            {
                viewCell.View.BackgroundColor = Color.FromHex("#fffcde");
            }
        }
    }
}
