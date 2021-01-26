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
        private ObservableCollection<ItemModel> Items;
        private ItemModel Item;
        bool clicked = false;
        SQLiteConnection db = Constants.DataBasePath();
        int importance = 1;
        public AddNewItem(ObservableCollection<ItemModel> items)
        {
            Items = items;
            InitializeComponent();
            InitXaml();
        }
        public AddNewItem(ObservableCollection<ItemModel> items, ItemModel item)
        {
            Items = items;
            Item = item;
            InitializeComponent();
            InitEdit();
        }
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
                Items.Remove(Item);
                db.Delete(Item);
                Item = new ItemModel { Text = input.Text, Checked = false, Importance = importance };
                Items.Add(Item);
                db.Insert(Item);
                Items = MainPage.SortItems(Items);
                clicked = true;
                PopupNavigation.Instance.PopAsync();
            }
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