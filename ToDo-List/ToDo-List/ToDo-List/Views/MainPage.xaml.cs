using System;
using Xamarin.Forms;
using ToDo_List.Models;
using System.Collections.ObjectModel;
using System.Linq;
using SQLite;

namespace ToDo_List.Views
{
	public partial class MainPage : ContentPage
	{
		SQLiteConnection db;
		public ObservableCollection<ItemModel> Items { get; set; } = new ObservableCollection<ItemModel>();
		public MainPage()
		{
			InitializeComponent();
			InitDB();
			Init();
		}
		private void Init()
		{
			entry.Text = "";
			itemsList.ItemsSource = Items;
			entry.Completed += Entry_Completed;
			itemsList.ItemSelected += ItemsList_ItemSelected;
		}
		private void InitDB()
		{
			db = new SQLiteConnection($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/ToDoList.sqlite");
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
			var button = (Button)sender;
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
			ItemModel newItem = new ItemModel { Name = entry.Text, Num = Items.Count};
			Items.Add(newItem);
			db.Insert(newItem);
			entry.Text = "";
		}
	}
}
