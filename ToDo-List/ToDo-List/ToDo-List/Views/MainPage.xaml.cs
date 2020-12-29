using System;
using Xamarin.Forms;
using ToDo_List.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace ToDo_List.Views
{
	public partial class MainPage : ContentPage
	{
		public ObservableCollection<ItemModel> Items { get; set; } = new ObservableCollection<ItemModel>();
		public MainPage()
		{
			InitializeComponent();
			Init();
		}
		private void Init()
		{
			entry.Text = "";
			itemsList.ItemsSource = Items;
			itemsList.ItemSelected += ItemsList_ItemSelected;
		}
		private void ItemsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			itemsList.SelectedItem = null;
		}
		private void SubmitClicked(object sender, EventArgs e)
		{
			if (entry.Text == "")
			{
				DisplayAlert("UWAGA!", "Uzupełnij pole!", "OK");
				return;
			}
			Items.Add(new ItemModel { Id = Items.Count, Name = entry.Text });
			entry.Text = "";
		}
		private void DeleteClicked(object sender, EventArgs e)
		{
			var button = (Button)sender;
			int index = Items.FirstOrDefault(x => x.Id == (int)button.CommandParameter).Id;
			//foreach (ItemModel x in Items)
			//{
			//	if (x.Id == (int)button.CommandParameter)
			//	{
			//		index = x.Id;
			//	}
			//}
			Items.RemoveAt(index);
		}
	}
}
