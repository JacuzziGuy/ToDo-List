using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using ToDo_List.Models;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SQLite;
using ToDo_List.DB;
using System.Linq;

namespace ToDo_List.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewItem : PopupPage
    {
        #region Variables

        SQLiteConnection db = Constants.DataBasePath();
        private ObservableCollection<ItemModel> Items;

        //Selected item
        private ItemModel Item;

        //After we clicked save, it doesn't allow to add multiple items from a single task
        bool clicked = false;

        //Importance of a selected / new task
        int importance = 1;

        #endregion

        //Constructor for adding new items
        public AddNewItem(ObservableCollection<ItemModel> items)
        {
            Items = items;
            InitializeComponent();
            InitXaml();
        }

        //Constructor for editing selected item
        public AddNewItem(ObservableCollection<ItemModel> items, ItemModel item)
        {
            Items = items;
            Item = item;
            InitializeComponent();
            InitEdit();
        }
       
        //Initializing xaml variables if we edit a selected taks
        private void InitEdit()
        {
            importance = Item.Importance;
            title.Text = "Edytuj zadanie";
            input.Placeholder = "Puste zadanie...";
            input.Text = Item.Text;
            btn1.BorderWidth = 0;
            btn2.BorderWidth = 0;
            btn3.BorderWidth = 0;
            btn4.BorderWidth = 0;
            btn5.BorderWidth = 0;
            btn6.BorderWidth = 0;
            switch (importance)
            {
                case 1:
                    btn1.BorderWidth = 2;
                    break;
                case 2:
                    btn2.BorderWidth = 2;
                    break;
                case 3:
                    btn3.BorderWidth = 2;
                    break;
                case 4:
                    btn4.BorderWidth = 2;
                    break;
                case 5:
                    btn5.BorderWidth = 2;
                    break;
                case 6:
                    btn6.BorderWidth = 2;
                    break;
            }
        }

        //Initializing xaml variables if we add a new task
        private void InitXaml()
        {
            input.Text = "";
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void SaveClicked(object sender, EventArgs e)
        {
            if(Item == null && !clicked)
            {
                var item = new ItemModel { Text = input.Text, Checked = false, Importance = importance };
                Items.Add(item);
                db.Insert(item);
                Items = MainPage.SortItems(Items);
                clicked = true;
                PopupNavigation.Instance.PopAsync();
            }
            else if(!clicked)
            {
                int index = Items.IndexOf(Item);
                Item.Text = input.Text;
                Item.Importance = importance;
                Items[index] = Item;
                db.Update(Item);
                Items = MainPage.SortItems(Items);
                clicked = true;
                PopupNavigation.Instance.PopAsync();
            }
        }

        //Making the popup not close after tapping on the background
        protected override bool OnBackgroundClicked()
        {
            return false;
        }

        //Setting priority of the task
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