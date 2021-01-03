using System;
using Xamarin.Forms;
using ToDo_List.Models;
using System.Collections.ObjectModel;
using System.Linq;
using SQLite;
using ToDo_List.DB;

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
			InitPage(); 
		}
		private void InitPage()
		{
		}
		private void InitList()
		{
			entry.Text = "";
			itemsList.ItemsSource = Items;
			entry.Completed += Entry_Completed;
			itemsList.ItemSelected += ItemsList_ItemSelected;
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
		private void Entry_Completed(object sender, EventArgs e)
		{
			if(entry.Text != "")
			{
				AddItem();
				entry.Focus();
			}
		}
		private void ItemsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			itemsList.SelectedItem = null;
		}
		private void SubmitClicked(object sender, EventArgs e)
		{
			AddItem();
		}
		private void DeleteClicked(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			ItemModel item = Items.FirstOrDefault(x => x.Num == (int)button.CommandParameter);
			Items.Remove(item);
			db.Delete(item);
		}
		private void AddItem()
		{
			if (entry.Text == "")
			{
				DisplayAlert("UWAGA!", "Uzupełnij pole!", "OK");
				return;
			}
			ItemModel newItem = new ItemModel { Name = entry.Text, Num = Items.Count, Checked = false };
			Items.Add(newItem);
			db.Insert(newItem);
			entry.Text = "";
		}
		private void CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			CheckBox cb =  sender as CheckBox;
			ItemModel item = cb.BindingContext as ItemModel;
			item.Checked = cb.IsChecked;
			db.Update(item);
		}
	}
}
